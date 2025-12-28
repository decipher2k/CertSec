//Copyright 2025 Dennis Michael Heine
using System;
using System.Security.Cryptography.X509Certificates;

namespace CertSec.Models
{
    [Serializable]
    public class CertificateInfo
    {
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Thumbprint { get; set; }
        public string Subject { get; set; }
        public string Issuer { get; set; }
        public DateTime NotBefore { get; set; }
        public DateTime NotAfter { get; set; }
        public string PublicKey { get; set; }
        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }
        public bool IsTrusted { get; set; }
        public string SerialNumber { get; set; }
        public string IpAddress { get; set; }

        public static CertificateInfo FromX509Certificate(X509Certificate2 cert, string hostname, int port)
        {
            return new CertificateInfo
            {
                Hostname = hostname,
                Port = port,
                Thumbprint = cert.Thumbprint,
                Subject = cert.Subject,
                Issuer = cert.Issuer,
                NotBefore = cert.NotBefore,
                NotAfter = cert.NotAfter,
                PublicKey = Convert.ToBase64String(cert.GetPublicKey()),
                SerialNumber = cert.SerialNumber,
                FirstSeen = DateTime.UtcNow,
                LastSeen = DateTime.UtcNow,
                IsTrusted = true,
                IpAddress = string.Empty
            };
        }

        public bool Matches(X509Certificate2 cert)
        {
            return Thumbprint == cert.Thumbprint &&
                   PublicKey == Convert.ToBase64String(cert.GetPublicKey()) &&
                   SerialNumber == cert.SerialNumber;
        }
    }
}
