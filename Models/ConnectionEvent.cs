//Copyright 2025 Dennis Michael Heine
using System;

namespace CertSec.Models
{
    [Serializable]
    public class ConnectionEvent
    {
        public DateTime Timestamp { get; set; }
        public string ProcessName { get; set; }
        public int ProcessId { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string CertificateThumbprint { get; set; }
        public ConnectionStatus Status { get; set; }
        public string Message { get; set; }
    }

    public enum ConnectionStatus
    {
        Allowed,
        Blocked,
        NewCertificate,
        CertificateChanged,
        CertificateExpired,
        ValidationFailed
    }
}
