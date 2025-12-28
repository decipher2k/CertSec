//Copyright 2025 Dennis Michael Heine
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using CertSec.Models;

namespace CertSec.Services
{
    public class CertificateStore
    {
        private static readonly string AppDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "CertSec"
        );

        private static readonly string CertificateDbPath = Path.Combine(AppDataPath, "certificates.db");
        private static readonly string EventLogPath = Path.Combine(AppDataPath, "events.log");

        private Dictionary<string, CertificateInfo> _certificates;
        private readonly object _lock = new object();

        public CertificateStore()
        {
            _certificates = new Dictionary<string, CertificateInfo>();
            EnsureDirectoryExists();
            Load();
        }

        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(AppDataPath))
            {
                Directory.CreateDirectory(AppDataPath);
            }
        }

        private string GetKey(string hostname, int port)
        {
            return $"{hostname.ToLowerInvariant()}:{port}";
        }

        public void AddOrUpdateCertificate(CertificateInfo certInfo)
        {
            lock (_lock)
            {
                string key = GetKey(certInfo.Hostname, certInfo.Port);
                if (_certificates.ContainsKey(key))
                {
                    _certificates[key].LastSeen = DateTime.UtcNow;
                }
                else
                {
                    _certificates[key] = certInfo;
                }
                Save();
            }
        }

        public CertificateInfo GetCertificate(string hostname, int port)
        {
            lock (_lock)
            {
                string key = GetKey(hostname, port);
                return _certificates.ContainsKey(key) ? _certificates[key] : null;
            }
        }

        public bool HasCertificate(string hostname, int port)
        {
            lock (_lock)
            {
                string key = GetKey(hostname, port);
                return _certificates.ContainsKey(key);
            }
        }

        public List<CertificateInfo> GetAllCertificates()
        {
            lock (_lock)
            {
                return _certificates.Values.ToList();
            }
        }

        public void RemoveCertificate(string hostname, int port)
        {
            lock (_lock)
            {
                string key = GetKey(hostname, port);
                if (_certificates.ContainsKey(key))
                {
                    _certificates.Remove(key);
                    Save();
                }
            }
        }

        public void UpdateTrustStatus(string hostname, int port, bool isTrusted)
        {
            lock (_lock)
            {
                string key = GetKey(hostname, port);
                if (_certificates.ContainsKey(key))
                {
                    _certificates[key].IsTrusted = isTrusted;
                    Save();
                }
            }
        }

        public void LogEvent(ConnectionEvent evt)
        {
            lock (_lock)
            {
                try
                {
                    string logEntry = $"[{evt.Timestamp:yyyy-MM-dd HH:mm:ss}] {evt.ProcessName} ({evt.ProcessId}) -> {evt.Hostname}:{evt.Port} | {evt.Status} | {evt.Message}\n";
                    File.AppendAllText(EventLogPath, logEntry);
                }
                catch (Exception)
                {
                    // Ignore logging errors
                }
            }
        }

        private void Save()
        {
            try
            {
                using (FileStream fs = new FileStream(CertificateDbPath, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, _certificates);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save certificate database: " + ex.Message, ex);
            }
        }

        private void Load()
        {
            try
            {
                if (File.Exists(CertificateDbPath))
                {
                    using (FileStream fs = new FileStream(CertificateDbPath, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        _certificates = (Dictionary<string, CertificateInfo>)formatter.Deserialize(fs);
                    }
                }
            }
            catch (Exception)
            {
                _certificates = new Dictionary<string, CertificateInfo>();
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _certificates.Clear();
                Save();
            }
        }
    }
}
