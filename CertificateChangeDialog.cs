//Copyright 2025 Dennis Michael Heine
using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using CertSec.Models;
using CertSec.Services;

namespace CertSec
{
    public partial class CertificateChangeDialog : Form
    {
        private CertificateInfo _oldCert;
        private X509Certificate2 _newCert;
        private string _hostname;
        private int _port;
        private string _processName;

        public bool AcceptNewCertificate { get; private set; }

        public CertificateChangeDialog(
            CertificateInfo oldCert, 
            X509Certificate2 newCert, 
            string hostname, 
            int port, 
            string processName)
        {
            InitializeComponent();
            
            _oldCert = oldCert;
            _newCert = newCert;
            _hostname = hostname;
            _port = port;
            _processName = processName;

            AcceptNewCertificate = false;

            LoadCertificateDetails();
            SetupWarningIcon();
        }

        private void SetupWarningIcon()
        {
            pictureBoxWarning.Image = SystemIcons.Warning.ToBitmap();
        }

        private void LoadCertificateDetails()
        {
            lblHostname.Text = $"Verbindung: {_hostname}:{_port}";
            lblProcess.Text = $"Prozess: {_processName}";

            string currentIp = DnsResolver.GetMultipleIpAddresses(_hostname);

            lblOldThumbprint.Text = $"Fingerabdruck: {_oldCert.Thumbprint}";
            lblOldIssuer.Text = $"Aussteller: {TruncateString(_oldCert.Issuer, 60)}";
            lblOldExpiry.Text = $"Ablaufdatum: {_oldCert.NotAfter.ToLocalTime():dd.MM.yyyy HH:mm}";
            
            if (!string.IsNullOrEmpty(_oldCert.IpAddress))
            {
                lblOldIP.Text = $"IP-Adresse: {_oldCert.IpAddress}";
                
                if (_oldCert.IpAddress != currentIp && currentIp != "Unknown")
                {
                    lblOldIP.Text += $" (Aktuell: {currentIp})";
                    lblOldIP.ForeColor = Color.Red;
                    lblOldIP.Font = new Font(lblOldIP.Font, FontStyle.Bold);
                }
            }
            else
            {
                lblOldIP.Text = $"IP-Adresse: Nicht gespeichert (Aktuell: {currentIp})";
            }

            lblNewThumbprint.Text = $"Fingerabdruck: {_newCert.Thumbprint}";
            lblNewIssuer.Text = $"Aussteller: {TruncateString(_newCert.Issuer, 60)}";
            lblNewExpiry.Text = $"Ablaufdatum: {_newCert.NotAfter.ToLocalTime():dd.MM.yyyy HH:mm}";
            lblNewIP.Text = $"IP-Adresse: {currentIp}";

            HighlightDifferences();
        }

        private void HighlightDifferences()
        {
            if (_oldCert.Thumbprint != _newCert.Thumbprint)
            {
                lblNewThumbprint.ForeColor = Color.Red;
                lblNewThumbprint.Font = new Font(lblNewThumbprint.Font, FontStyle.Bold);
            }

            if (_oldCert.Issuer != _newCert.Issuer)
            {
                lblNewIssuer.ForeColor = Color.Red;
                lblNewIssuer.Font = new Font(lblNewIssuer.Font, FontStyle.Bold);
            }

            if (_oldCert.NotAfter != _newCert.NotAfter)
            {
                lblNewExpiry.ForeColor = Color.Orange;
                lblNewExpiry.Font = new Font(lblNewExpiry.Font, FontStyle.Bold);
            }
        }

        private string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            if (value.Length <= maxLength)
                return value;

            return value.Substring(0, maxLength - 3) + "...";
        }

        private void btnBlock_Click(object sender, EventArgs e)
        {
            AcceptNewCertificate = false;
            
            var result = MessageBox.Show(
                "Die Verbindung wird blockiert und das alte Zertifikat bleibt gespeichert.\n\n" +
                "Zukünftige Verbindungsversuche zu diesem Server werden weiterhin blockiert, " +
                "bis Sie das Zertifikat manuell entfernen oder aktualisieren.\n\n" +
                "Möchten Sie fortfahren?",
                "Verbindung blockieren",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "?? WARNUNG ??\n\n" +
                "Sie sind dabei, das neue Zertifikat zu akzeptieren!\n\n" +
                "Wenn dies ein Angriff ist, gewähren Sie dem Angreifer Zugriff auf Ihre " +
                "verschlüsselte Kommunikation.\n\n" +
                "Akzeptieren Sie das neue Zertifikat NUR, wenn:\n" +
                "• Sie sicher sind, dass der Zertifikatswechsel legitim ist\n" +
                "• Sie den Administrator des Servers kontaktiert haben\n" +
                "• Sie die Fingerabdrücke verifiziert haben\n\n" +
                "Möchten Sie das neue Zertifikat wirklich akzeptieren?",
                "Zertifikat aktualisieren - BESTÄTIGUNG ERFORDERLICH",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            );

            if (result == DialogResult.Yes)
            {
                var finalConfirm = MessageBox.Show(
                    "LETZTE WARNUNG!\n\n" +
                    "Durch das Akzeptieren dieses Zertifikats könnten Sie einem Angreifer " +
                    "ermöglichen, Ihre verschlüsselte Kommunikation abzufangen.\n\n" +
                    "Sind Sie ABSOLUT SICHER, dass Sie fortfahren möchten?",
                    "Finale Bestätigung",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Stop,
                    MessageBoxDefaultButton.Button2
                );

                if (finalConfirm == DialogResult.Yes)
                {
                    AcceptNewCertificate = true;
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }
    }
}
