//Copyright 2025 Dennis Michael Heine
using System;
using System.Security.Cryptography.X509Certificates;
using CertSec.Models;

namespace CertSec.Services
{
    public class CertificateValidator
    {
        private readonly CertificateStore _certStore;

        public CertificateValidator(CertificateStore certStore)
        {
            _certStore = certStore;
        }

        public ValidationResult ValidateCertificate(X509Certificate2 certificate, string hostname, int port, string processName)
        {
            var result = new ValidationResult();
            result.Event = new ConnectionEvent
            {
                Timestamp = DateTime.UtcNow,
                ProcessName = processName,
                ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id,
                Hostname = hostname,
                Port = port,
                CertificateThumbprint = certificate.Thumbprint
            };

            if (certificate.NotAfter < DateTime.Now)
            {
                result.IsValid = false;
                result.Event.Status = ConnectionStatus.CertificateExpired;
                result.Event.Message = "Certificate has expired";
                _certStore.LogEvent(result.Event);
                return result;
            }

            if (!_certStore.HasCertificate(hostname, port))
            {
                var certInfo = CertificateInfo.FromX509Certificate(certificate, hostname, port);
                
                // Resolve and store IP address
                certInfo.IpAddress = DnsResolver.GetMultipleIpAddresses(hostname);
                
                _certStore.AddOrUpdateCertificate(certInfo);

                result.IsValid = true;
                result.Event.Status = ConnectionStatus.NewCertificate;
                result.Event.Message = "New certificate learned (TOFU)";
                _certStore.LogEvent(result.Event);
                return result;
            }

            var storedCert = _certStore.GetCertificate(hostname, port);

            if (!storedCert.IsTrusted)
            {
                result.IsValid = false;
                result.Event.Status = ConnectionStatus.Blocked;
                result.Event.Message = "Certificate manually marked as untrusted";
                _certStore.LogEvent(result.Event);
                return result;
            }

            if (!storedCert.Matches(certificate))
            {
                result.IsValid = false;
                result.Event.Status = ConnectionStatus.CertificateChanged;
                result.Event.Message = $"Certificate changed! Expected: {storedCert.Thumbprint}, Got: {certificate.Thumbprint}";
                result.OldCertificate = storedCert;
                result.NewCertificate = certificate;
                _certStore.LogEvent(result.Event);
                return result;
            }

            storedCert.LastSeen = DateTime.UtcNow;
            _certStore.AddOrUpdateCertificate(storedCert);

            result.IsValid = true;
            result.Event.Status = ConnectionStatus.Allowed;
            result.Event.Message = "Certificate validated successfully";
            _certStore.LogEvent(result.Event);

            return result;
        }

        public void UpdateCertificate(X509Certificate2 newCertificate, string hostname, int port)
        {
            var certInfo = CertificateInfo.FromX509Certificate(newCertificate, hostname, port);
            
            // Resolve and store IP address
            certInfo.IpAddress = DnsResolver.GetMultipleIpAddresses(hostname);
            
            // Update the certificate
            _certStore.AddOrUpdateCertificate(certInfo);
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public ConnectionEvent Event { get; set; }
        public CertificateInfo OldCertificate { get; set; }
        public X509Certificate2 NewCertificate { get; set; }
    }
}
