// Copyright 2025 Dennis Michael Heine
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace CertSec.Services
{
    public class ProcessMonitor
    {
        private static readonly HashSet<string> BrowserProcessNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "chrome", "firefox", "msedge", "iexplore", "opera", "brave", "vivaldi", "safari"
        };

        public static bool IsBrowserProcess(int processId)
        {
            try
            {
                using (Process process = Process.GetProcessById(processId))
                {
                    return IsBrowserProcess(process.ProcessName);
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool IsBrowserProcess(string processName)
        {
            if (string.IsNullOrEmpty(processName))
                return false;

            // Remove .exe extension if present
            string cleanName = processName.ToLower().Replace(".exe", "");

            return BrowserProcessNames.Any(browser => cleanName.Contains(browser));
        }

        public static string GetProcessName(int processId)
        {
            try
            {
                using (Process process = Process.GetProcessById(processId))
                {
                    return process.ProcessName;
                }
            }
            catch
            {
                return "unknown";
            }
        }

        public static List<ProcessInfo> GetActiveHttpsProcesses()
        {
            List<ProcessInfo> processes = new List<ProcessInfo>();

            try
            {
                // Get all TCP connections on port 443
                var connections = GetTcpConnections();

                foreach (var conn in connections)
                {
                    if (conn.RemotePort == 443 && !IsBrowserProcess(conn.ProcessId))
                    {
                        processes.Add(new ProcessInfo
                        {
                            ProcessId = conn.ProcessId,
                            ProcessName = GetProcessName(conn.ProcessId),
                            RemoteAddress = conn.RemoteAddress,
                            RemotePort = conn.RemotePort
                        });
                    }
                }
            }
            catch
            {
                // Error getting connections
            }

            return processes;
        }

        private static List<TcpConnectionInfo> GetTcpConnections()
        {
            List<TcpConnectionInfo> connections = new List<TcpConnectionInfo>();

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "netstat",
                    Arguments = "-ano",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                };

                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string line in lines)
                    {
                        if (line.Trim().StartsWith("TCP", StringComparison.OrdinalIgnoreCase))
                        {
                            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            
                            if (parts.Length >= 5)
                            {
                                try
                                {
                                    string remoteAddress = parts[2];
                                    string[] remoteAddressParts = remoteAddress.Split(':');
                                    
                                    if (remoteAddressParts.Length == 2)
                                    {
                                        connections.Add(new TcpConnectionInfo
                                        {
                                            RemoteAddress = remoteAddressParts[0],
                                            RemotePort = int.Parse(remoteAddressParts[1]),
                                            ProcessId = int.Parse(parts[4])
                                        });
                                    }
                                }
                                catch
                                {
                                    // Skip invalid lines
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // Error parsing netstat output
            }

            return connections;
        }
    }

    public class ProcessInfo
    {
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string RemoteAddress { get; set; }
        public int RemotePort { get; set; }
    }

    internal class TcpConnectionInfo
    {
        public string RemoteAddress { get; set; }
        public int RemotePort { get; set; }
        public int ProcessId { get; set; }
    }
}
