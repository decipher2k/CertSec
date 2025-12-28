// Copyright 2025 Dennis Michael Heine
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CertSec.Services;
using CertSec.Models;

namespace CertSec
{
    public partial class Form1 : Form
    {
        private ProxyService _proxyService;
        private CertificateStore _certStore;
        private TrafficRedirector _trafficRedirector;
        private bool _isRunning = false;

        public Form1()
        {
            InitializeComponent();
            _certStore = new CertificateStore();
            LoadCertificates();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Check if running as administrator
            if (!TrafficRedirector.IsRunningAsAdmin())
            {
                chkAutoRedirect.Enabled = false;
                lblRedirectStatus.Text = "(Erfordert Admin-Rechte)";
                lblRedirectStatus.ForeColor = Color.Orange;
            }

            // Automatically start proxy on load
            StartProxy();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (!_isRunning)
            {
                StartProxy();
            }
            else
            {
                StopProxy();
            }
        }

        private void StartProxy()
        {
            try
            {
                int port;
                if (!int.TryParse(txtProxyPort.Text, out port) || port < 1024 || port > 65535)
                {
                    MessageBox.Show("Bitte geben Sie einen gültigen Port zwischen 1024 und 65535 ein.", "Ungültiger Port", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _proxyService = new ProxyService(_certStore, port);
                _proxyService.ConnectionEventOccurred += ProxyService_ConnectionEventOccurred;
                _proxyService.CertificateChangeDetected += ProxyService_CertificateChangeDetected;
                _proxyService.Start();

                _isRunning = true;
                btnStartStop.Text = "Stoppen";
                lblStatus.Text = $"Status: Läuft (Port {port})";
                lblStatus.ForeColor = Color.Green;
                txtProxyPort.Enabled = false;
                
                // Enable and activate auto-redirect if admin
                if (TrafficRedirector.IsRunningAsAdmin())
                {
                    chkAutoRedirect.Enabled = true;
                    
                    // Temporarily remove event handler to avoid confirmation dialog
                    chkAutoRedirect.CheckedChanged -= chkAutoRedirect_CheckedChanged;
                    chkAutoRedirect.Checked = true;
                    chkAutoRedirect.CheckedChanged += chkAutoRedirect_CheckedChanged;
                    
                    // Activate traffic redirection without confirmation
                    ActivateTrafficRedirection(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Starten des Proxy: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StopProxy()
        {
            try
            {
                // Deactivate traffic redirection first
                if (_trafficRedirector != null && _trafficRedirector.IsActive())
                {
                    chkAutoRedirect.Checked = false;
                }

                if (_proxyService != null)
                {
                    _proxyService.ConnectionEventOccurred -= ProxyService_ConnectionEventOccurred;
                    _proxyService.CertificateChangeDetected -= ProxyService_CertificateChangeDetected;
                    _proxyService.Stop();
                    _proxyService = null;
                }

                _isRunning = false;
                btnStartStop.Text = "Start";
                lblStatus.Text = "Status: Gestoppt";
                lblStatus.ForeColor = SystemColors.ControlText;
                txtProxyPort.Enabled = true;
                chkAutoRedirect.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Stoppen des Proxy: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkAutoRedirect_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isRunning)
            {
                if (sender != null) // Only show message if user manually changed it
                {
                    chkAutoRedirect.Checked = false;
                    MessageBox.Show("Bitte starten Sie zuerst den Proxy-Service.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            if (!TrafficRedirector.IsRunningAsAdmin())
            {
                chkAutoRedirect.Checked = false;
                if (sender != null) // Only show message if user manually changed it
                {
                    MessageBox.Show(
                        "Für die automatische Traffic-Umleitung sind Administrator-Rechte erforderlich.\n\n" +
                        "Bitte starten Sie die Anwendung als Administrator neu.",
                        "Administrator-Rechte erforderlich",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
                return;
            }

            if (chkAutoRedirect.Checked)
            {
                // Show confirmation only if manually changed by user
                bool showConfirmation = (sender != null);
                ActivateTrafficRedirection(showConfirmation);
            }
            else
            {
                DeactivateTrafficRedirection();
            }
        }

        private void ActivateTrafficRedirection(bool showConfirmation = true)
        {
            try
            {
                int port;
                int.TryParse(txtProxyPort.Text, out port);

                _trafficRedirector = new TrafficRedirector(port);
                
                DialogResult result = DialogResult.Yes;
                
                if (showConfirmation)
                {
                    result = MessageBox.Show(
                        "Die automatische Traffic-Umleitung wird jetzt aktiviert.\n\n" +
                        "Dies wird:\n" +
                        "• System-weite Proxy-Einstellungen ändern\n" +
                        "• Windows Firewall-Regeln erstellen\n" +
                        "• HTTPS-Traffic von Nicht-Browser-Apps umleiten\n\n" +
                        "Möchten Sie fortfahren?",
                        "Traffic-Umleitung aktivieren",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );
                }

                if (result == DialogResult.Yes)
                {
                    _trafficRedirector.Activate();
                    lblRedirectStatus.Text = "✓ Aktiv";
                    lblRedirectStatus.ForeColor = Color.Green;
                    
                    if (showConfirmation)
                    {
                        MessageBox.Show(
                            "Traffic-Umleitung erfolgreich aktiviert!\n\n" +
                            "HTTPS-Verbindungen (Port 443) von Nicht-Browser-Anwendungen\n" +
                            "werden jetzt automatisch über den Proxy umgeleitet.",
                            "Erfolg",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                }
                else
                {
                    chkAutoRedirect.Checked = false;
                }
            }
            catch (Exception ex)
            {
                chkAutoRedirect.Checked = false;
                lblRedirectStatus.Text = "✗ Fehler";
                lblRedirectStatus.ForeColor = Color.Red;
                MessageBox.Show(
                    $"Fehler beim Aktivieren der Traffic-Umleitung:\n\n{ex.Message}",
                    "Fehler",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void DeactivateTrafficRedirection()
        {
            try
            {
                if (_trafficRedirector != null)
                {
                    _trafficRedirector.Deactivate();
                    _trafficRedirector = null;
                    lblRedirectStatus.Text = "";
                    
                    MessageBox.Show(
                        "Traffic-Umleitung wurde deaktiviert.\n\n" +
                        "System-Proxy-Einstellungen und Firewall-Regeln wurden entfernt.",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Fehler beim Deaktivieren der Traffic-Umleitung:\n\n{ex.Message}",
                    "Fehler",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ProxyService_ConnectionEventOccurred(object sender, ConnectionEvent evt)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, ConnectionEvent>(ProxyService_ConnectionEventOccurred), sender, evt);
                return;
            }

            var item = new ListViewItem(evt.Timestamp.ToLocalTime().ToString("HH:mm:ss"));
            item.SubItems.Add(evt.ProcessName);
            item.SubItems.Add($"{evt.Hostname}:{evt.Port}");
            item.SubItems.Add(evt.Status.ToString());
            item.SubItems.Add(evt.Message);

            switch (evt.Status)
            {
                case ConnectionStatus.Allowed:
                    item.ForeColor = Color.Green;
                    break;
                case ConnectionStatus.Blocked:
                    item.ForeColor = Color.Red;
                    item.BackColor = Color.LightPink;
                    break;
                case ConnectionStatus.CertificateChanged:
                    item.ForeColor = Color.Red;
                    item.BackColor = Color.LightPink;
                    // Dialog is now handled by ProxyService_CertificateChangeDetected
                    break;
                case ConnectionStatus.NewCertificate:
                    item.ForeColor = Color.Blue;
                    break;
                case ConnectionStatus.CertificateExpired:
                    item.ForeColor = Color.Orange;
                    break;
            }

            listViewConnections.Items.Insert(0, item);

            if (listViewConnections.Items.Count > 1000)
            {
                listViewConnections.Items.RemoveAt(listViewConnections.Items.Count - 1);
            }
        }

        private void ProxyService_CertificateChangeDetected(object sender, CertificateChangeEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, CertificateChangeEventArgs>(ProxyService_CertificateChangeDetected), sender, e);
                return;
            }

            using (var dialog = new CertificateChangeDialog(
                e.OldCertificate,
                e.NewCertificate,
                e.Hostname,
                e.Port,
                e.ProcessName))
            {
                var result = dialog.ShowDialog(this);
                
                if (result == DialogResult.OK && dialog.AcceptNewCertificate)
                {
                    e.UserAccepted = true;
                }
                else
                {
                    e.UserAccepted = false;
                }
            }
        }

        private void ShowNotification(ConnectionEvent evt)
        {
            if (evt.Status == ConnectionStatus.Blocked)
            {
                MessageBox.Show(
                    $"SICHERHEITSWARNUNG!\n\n" +
                    $"Verbindung: {evt.Hostname}:{evt.Port}\n" +
                    $"Prozess: {evt.ProcessName}\n" +
                    $"Status: {evt.Status}\n\n" +
                    $"{evt.Message}\n\n" +
                    $"Die Verbindung wurde blockiert.",
                    "CertSec - Sicherheitswarnung",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void btnRefreshCerts_Click(object sender, EventArgs e)
        {
            LoadCertificates();
        }

        private void LoadCertificates()
        {
            listViewCertificates.Items.Clear();

            var certs = _certStore.GetAllCertificates();
            foreach (var cert in certs.OrderBy(c => c.Hostname).ThenBy(c => c.Port))
            {
                var item = new ListViewItem(cert.Hostname);
                item.SubItems.Add(cert.Port.ToString());
                item.SubItems.Add(cert.Thumbprint.Substring(0, Math.Min(16, cert.Thumbprint.Length)) + "...");
                item.SubItems.Add(cert.Issuer);
                item.SubItems.Add(cert.NotAfter.ToShortDateString());
                item.SubItems.Add(cert.IsTrusted ? "Ja" : "Nein");
                item.Tag = cert;

                if (!cert.IsTrusted)
                {
                    item.BackColor = Color.LightGray;
                }
                else if (cert.NotAfter < DateTime.Now)
                {
                    item.BackColor = Color.LightYellow;
                }

                listViewCertificates.Items.Add(item);
            }
        }

        private void btnRemoveCert_Click(object sender, EventArgs e)
        {
            if (listViewCertificates.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bitte wählen Sie ein Zertifikat aus.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var cert = listViewCertificates.SelectedItems[0].Tag as CertificateInfo;
            if (cert != null)
            {
                var result = MessageBox.Show(
                    $"Möchten Sie das Zertifikat für {cert.Hostname}:{cert.Port} wirklich entfernen?",
                    "Bestätigung",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    _certStore.RemoveCertificate(cert.Hostname, cert.Port);
                    LoadCertificates();
                }
            }
        }

        private void btnToggleTrust_Click(object sender, EventArgs e)
        {
            if (listViewCertificates.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bitte wählen Sie ein Zertifikat aus.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var cert = listViewCertificates.SelectedItems[0].Tag as CertificateInfo;
            if (cert != null)
            {
                _certStore.UpdateTrustStatus(cert.Hostname, cert.Port, !cert.IsTrusted);
                LoadCertificates();
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Möchten Sie wirklich alle gespeicherten Zertifikate löschen?",
                "Bestätigung",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                _certStore.Clear();
                LoadCertificates();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_trafficRedirector != null && _trafficRedirector.IsActive())
            {
                var result = MessageBox.Show(
                    "Die Traffic-Umleitung ist noch aktiv.\n\n" +
                    "Möchten Sie sie deaktivieren, bevor Sie die Anwendung schließen?",
                    "Traffic-Umleitung aktiv",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                else if (result == DialogResult.Yes)
                {
                    try
                    {
                        _trafficRedirector.Deactivate();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Fehler beim Deaktivieren: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            if (_isRunning)
            {
                StopProxy();
            }
        }
    }
}
