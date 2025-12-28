//Copyright 2025 Dennis Michael Heine
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CertSec.Models;

namespace CertSec.Services
{
    public class ProxyService
    {
        private TcpListener _listener;
        private CertificateStore _certStore;
        private CertificateValidator _validator;
        private bool _isRunning;
        private CancellationTokenSource _cancellationTokenSource;
        private int _proxyPort;

        private static readonly HashSet<string> BrowserProcesses = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "chrome", "firefox", "msedge", "iexplore", "opera", "brave", "vivaldi", "safari"
        };

        public event EventHandler<ConnectionEvent> ConnectionEventOccurred;
        public event EventHandler<CertificateChangeEventArgs> CertificateChangeDetected;

        public ProxyService(CertificateStore certStore, int port = 8888)
        {
            _certStore = certStore;
            _validator = new CertificateValidator(certStore);
            _proxyPort = port;
        }

        public void Start()
        {
            if (_isRunning)
                return;

            _cancellationTokenSource = new CancellationTokenSource();
            _listener = new TcpListener(IPAddress.Loopback, _proxyPort);
            _listener.Start();
            _isRunning = true;

            Task.Run(() => AcceptClientsAsync(_cancellationTokenSource.Token));
        }

        public void Stop()
        {
            if (!_isRunning)
                return;

            _isRunning = false;
            _cancellationTokenSource?.Cancel();
            _listener?.Stop();
        }

        private async Task AcceptClientsAsync(CancellationToken cancellationToken)
        {
            while (_isRunning && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => HandleClientAsync(client, cancellationToken), cancellationToken);
                }
                catch (Exception)
                {
                    if (!_isRunning)
                        break;
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
        {
            try
            {
                using (client)
                using (NetworkStream clientStream = client.GetStream())
                {
                    byte[] buffer = new byte[8192];
                    int bytesRead = await clientStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

                    if (bytesRead == 0)
                        return;

                    string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    if (request.StartsWith("CONNECT ", StringComparison.OrdinalIgnoreCase))
                    {
                        await HandleConnectRequestAsync(clientStream, request, cancellationToken);
                    }
                    else
                    {
                        await HandleHttpRequestAsync(clientStream, request, buffer, bytesRead, cancellationToken);
                    }
                }
            }
            catch (Exception)
            {
                // Connection handling error
            }
        }

        private async Task HandleConnectRequestAsync(NetworkStream clientStream, string request, CancellationToken cancellationToken)
        {
            string[] lines = request.Split(new[] { "\r\n" }, StringSplitOptions.None);
            string[] parts = lines[0].Split(' ');

            if (parts.Length < 2)
                return;

            string[] hostPort = parts[1].Split(':');
            string hostname = hostPort[0];
            int port = hostPort.Length > 1 ? int.Parse(hostPort[1]) : 443;

            // Check User-Agent or assume browser if we can't determine
            string userAgent = ExtractUserAgent(request);
            bool isBrowser = IsBrowserUserAgent(userAgent);

            // Check if it's a Microsoft domain
            bool isMicrosoftDomain = IsMicrosoftDomain(hostname);

            if (isBrowser || isMicrosoftDomain)
            {
                // Browser traffic or Microsoft domains: forward directly without monitoring
                await ForwardConnectionDirectlyAsync(clientStream, hostname, port, cancellationToken);
                return;
            }

            // Non-browser traffic: monitor with certificate pinning
            byte[] successResponse = Encoding.ASCII.GetBytes("HTTP/1.1 200 Connection Established\r\n\r\n");
            await clientStream.WriteAsync(successResponse, 0, successResponse.Length, cancellationToken);
            await clientStream.FlushAsync(cancellationToken);

            await MonitorSslConnectionAsync(clientStream, hostname, port, "Non-Browser App", cancellationToken);
        }

        private bool IsMicrosoftDomain(string hostname)
        {
            if (string.IsNullOrEmpty(hostname))
                return false;

            string hostnameLower = hostname.ToLower();

            // Check if it's microsoft.com or any subdomain
            if (hostnameLower.Equals("microsoft.com", StringComparison.OrdinalIgnoreCase) ||
                hostnameLower.EndsWith(".microsoft.com", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // Also check other common Microsoft domains
            string[] microsoftDomains = new[]
            {
                "windows.com",
                "windowsupdate.com",
                "update.microsoft.com",
                "microsoftonline.com",
                "live.com",
                "outlook.com",
                "office.com",
                "office365.com",
                "azure.com",
                "microsoftazure.com",
                "visualstudio.com",
                "xbox.com",
                "skype.com",
                "msn.com",
                "bing.com"
            };

            foreach (string domain in microsoftDomains)
            {
                if (hostnameLower.Equals(domain, StringComparison.OrdinalIgnoreCase) ||
                    hostnameLower.EndsWith("." + domain, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private async Task MonitorSslConnectionAsync(NetworkStream clientStream, string hostname, int port, string processName, CancellationToken cancellationToken)
        {
            TcpClient serverClient = null;
            try
            {
                serverClient = new TcpClient();
                await serverClient.ConnectAsync(hostname, port);

                using (NetworkStream serverStream = serverClient.GetStream())
                using (SslStream sslServerStream = new SslStream(serverStream, false, ValidateServerCertificate))
                {
                    await sslServerStream.AuthenticateAsClientAsync(hostname);

                    X509Certificate2 serverCert = new X509Certificate2(sslServerStream.RemoteCertificate);

                    var validationResult = _validator.ValidateCertificate(serverCert, hostname, port, processName);

                    ConnectionEventOccurred?.Invoke(this, validationResult.Event);

                    if (!validationResult.IsValid)
                    {
                        // Fire certificate change event if it's a certificate mismatch
                        if (validationResult.Event.Status == ConnectionStatus.CertificateChanged)
                        {
                            var changeArgs = new CertificateChangeEventArgs
                            {
                                OldCertificate = validationResult.OldCertificate,
                                NewCertificate = validationResult.NewCertificate,
                                Hostname = hostname,
                                Port = port,
                                ProcessName = processName,
                                ValidationResult = validationResult
                            };

                            CertificateChangeDetected?.Invoke(this, changeArgs);

                            // If user accepted the certificate, allow the connection
                            if (changeArgs.UserAccepted)
                            {
                                _validator.UpdateCertificate(validationResult.NewCertificate, hostname, port);
                                
                                // Continue with connection
                                await Task.WhenAll(
                                    ForwardDataAsync(clientStream, sslServerStream, cancellationToken),
                                    ForwardDataAsync(sslServerStream, clientStream, cancellationToken)
                                );
                                return;
                            }
                        }

                        // Block the connection
                        return;
                    }

                    await Task.WhenAll(
                        ForwardDataAsync(clientStream, sslServerStream, cancellationToken),
                        ForwardDataAsync(sslServerStream, clientStream, cancellationToken)
                    );
                }
            }
            catch (Exception)
            {
                // Connection error
            }
            finally
            {
                serverClient?.Close();
            }
        }

        private async Task HandleHttpRequestAsync(NetworkStream clientStream, string request, byte[] initialBuffer, int initialBytesRead, CancellationToken cancellationToken)
        {
            // Extract host from HTTP request
            string host = ExtractHost(request);
            if (string.IsNullOrEmpty(host))
                return;

            string[] hostPort = host.Split(':');
            string hostname = hostPort[0];
            int port = hostPort.Length > 1 ? int.Parse(hostPort[1]) : 80;

            try
            {
                using (TcpClient serverClient = new TcpClient())
                {
                    await serverClient.ConnectAsync(hostname, port);
                    using (NetworkStream serverStream = serverClient.GetStream())
                    {
                        await serverStream.WriteAsync(initialBuffer, 0, initialBytesRead, cancellationToken);

                        await Task.WhenAll(
                            ForwardDataAsync(clientStream, serverStream, cancellationToken),
                            ForwardDataAsync(serverStream, clientStream, cancellationToken)
                        );
                    }
                }
            }
            catch (Exception)
            {
                // Forward error
            }
        }

        private async Task ForwardConnectionDirectlyAsync(NetworkStream clientStream, string hostname, int port, CancellationToken cancellationToken)
        {
            try
            {
                using (TcpClient serverClient = new TcpClient())
                {
                    await serverClient.ConnectAsync(hostname, port);

                    byte[] successResponse = Encoding.ASCII.GetBytes("HTTP/1.1 200 Connection Established\r\n\r\n");
                    await clientStream.WriteAsync(successResponse, 0, successResponse.Length, cancellationToken);
                    await clientStream.FlushAsync(cancellationToken);

                    using (NetworkStream serverStream = serverClient.GetStream())
                    {
                        await Task.WhenAll(
                            ForwardDataAsync(clientStream, serverStream, cancellationToken),
                            ForwardDataAsync(serverStream, clientStream, cancellationToken)
                        );
                    }
                }
            }
            catch (Exception)
            {
                // Direct forward error
            }
        }

        private async Task ForwardDataAsync(Stream source, Stream destination, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[8192];
            try
            {
                int bytesRead;
                while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                {
                    await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                    await destination.FlushAsync(cancellationToken);
                }
            }
            catch (Exception)
            {
                // Stream closed or error
            }
        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private string ExtractHost(string request)
        {
            string[] lines = request.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                if (line.StartsWith("Host:", StringComparison.OrdinalIgnoreCase))
                {
                    return line.Substring(5).Trim();
                }
            }
            return null;
        }

        private string GetCallingProcessName()
        {
            try
            {
                // This is a simplified approach - in production would need more sophisticated process tracking
                return Process.GetCurrentProcess().ProcessName;
            }
            catch
            {
                return "unknown";
            }
        }

        private bool IsBrowserProcess(string processName)
        {
            return BrowserProcesses.Any(browser => processName.Contains(browser));
        }

        private string ExtractUserAgent(string request)
        {
            string[] lines = request.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                if (line.StartsWith("User-Agent:", StringComparison.OrdinalIgnoreCase))
                {
                    return line.Substring(11).Trim();
                }
            }
            return string.Empty;
        }

        private bool IsBrowserUserAgent(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return false;

            string userAgentLower = userAgent.ToLower();

            // Check for common browser user agents
            string[] browserSignatures = new[]
            {
                "mozilla", "chrome", "firefox", "safari", "edge", "edg/",
                "opera", "brave", "vivaldi", "seamonkey", "internet explorer", "msie"
            };

            foreach (string signature in browserSignatures)
            {
                if (userAgentLower.Contains(signature))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class CertificateChangeEventArgs : EventArgs
    {
        public CertificateInfo OldCertificate { get; set; }
        public X509Certificate2 NewCertificate { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string ProcessName { get; set; }
        public ValidationResult ValidationResult { get; set; }
        public bool UserAccepted { get; set; }
    }
}
