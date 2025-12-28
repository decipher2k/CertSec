//Copyright 2025 Dennis Michael Heine
namespace CertSec
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStartStop = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.listViewConnections = new System.Windows.Forms.ListView();
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colProcess = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageMonitor = new System.Windows.Forms.TabPage();
            this.tabPageCertificates = new System.Windows.Forms.TabPage();
            this.listViewCertificates = new System.Windows.Forms.ListView();
            this.colCertHost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCertPort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCertThumbprint = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCertIssuer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCertExpiry = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCertTrusted = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRefreshCerts = new System.Windows.Forms.Button();
            this.btnRemoveCert = new System.Windows.Forms.Button();
            this.btnToggleTrust = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.txtProxyPort = new System.Windows.Forms.TextBox();
            this.lblProxyPort = new System.Windows.Forms.Label();
            this.lblProxyInfo = new System.Windows.Forms.Label();
            this.chkAutoRedirect = new System.Windows.Forms.CheckBox();
            this.lblRedirectStatus = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPageMonitor.SuspendLayout();
            this.tabPageCertificates.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(12, 12);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(100, 30);
            this.btnStartStop.TabIndex = 0;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(118, 19);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(95, 13);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Status: Gestoppt";
            // 
            // listViewConnections
            // 
            this.listViewConnections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTime,
            this.colProcess,
            this.colHost,
            this.colStatus,
            this.colMessage});
            this.listViewConnections.FullRowSelect = true;
            this.listViewConnections.GridLines = true;
            this.listViewConnections.HideSelection = false;
            this.listViewConnections.Location = new System.Drawing.Point(6, 6);
            this.listViewConnections.Name = "listViewConnections";
            this.listViewConnections.Size = new System.Drawing.Size(756, 360);
            this.listViewConnections.TabIndex = 2;
            this.listViewConnections.UseCompatibleStateImageBehavior = false;
            this.listViewConnections.View = System.Windows.Forms.View.Details;
            // 
            // colTime
            // 
            this.colTime.Text = "Zeit";
            this.colTime.Width = 120;
            // 
            // colProcess
            // 
            this.colProcess.Text = "Prozess";
            this.colProcess.Width = 120;
            // 
            // colHost
            // 
            this.colHost.Text = "Host";
            this.colHost.Width = 200;
            // 
            // colStatus
            // 
            this.colStatus.Text = "Status";
            this.colStatus.Width = 100;
            // 
            // colMessage
            // 
            this.colMessage.Text = "Nachricht";
            this.colMessage.Width = 200;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageMonitor);
            this.tabControl1.Controls.Add(this.tabPageCertificates);
            this.tabControl1.Location = new System.Drawing.Point(12, 80);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(776, 398);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPageMonitor
            // 
            this.tabPageMonitor.Controls.Add(this.listViewConnections);
            this.tabPageMonitor.Location = new System.Drawing.Point(4, 22);
            this.tabPageMonitor.Name = "tabPageMonitor";
            this.tabPageMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMonitor.Size = new System.Drawing.Size(768, 372);
            this.tabPageMonitor.TabIndex = 0;
            this.tabPageMonitor.Text = "Verbindungsmonitor";
            this.tabPageMonitor.UseVisualStyleBackColor = true;
            // 
            // tabPageCertificates
            // 
            this.tabPageCertificates.Controls.Add(this.btnClearAll);
            this.tabPageCertificates.Controls.Add(this.btnToggleTrust);
            this.tabPageCertificates.Controls.Add(this.btnRemoveCert);
            this.tabPageCertificates.Controls.Add(this.btnRefreshCerts);
            this.tabPageCertificates.Controls.Add(this.listViewCertificates);
            this.tabPageCertificates.Location = new System.Drawing.Point(4, 22);
            this.tabPageCertificates.Name = "tabPageCertificates";
            this.tabPageCertificates.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCertificates.Size = new System.Drawing.Size(768, 372);
            this.tabPageCertificates.TabIndex = 1;
            this.tabPageCertificates.Text = "Zertifikate";
            this.tabPageCertificates.UseVisualStyleBackColor = true;
            // 
            // listViewCertificates
            // 
            this.listViewCertificates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCertificates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCertHost,
            this.colCertPort,
            this.colCertThumbprint,
            this.colCertIssuer,
            this.colCertExpiry,
            this.colCertTrusted});
            this.listViewCertificates.FullRowSelect = true;
            this.listViewCertificates.GridLines = true;
            this.listViewCertificates.HideSelection = false;
            this.listViewCertificates.Location = new System.Drawing.Point(6, 6);
            this.listViewCertificates.Name = "listViewCertificates";
            this.listViewCertificates.Size = new System.Drawing.Size(756, 320);
            this.listViewCertificates.TabIndex = 0;
            this.listViewCertificates.UseCompatibleStateImageBehavior = false;
            this.listViewCertificates.View = System.Windows.Forms.View.Details;
            // 
            // colCertHost
            // 
            this.colCertHost.Text = "Host";
            this.colCertHost.Width = 180;
            // 
            // colCertPort
            // 
            this.colCertPort.Text = "Port";
            this.colCertPort.Width = 50;
            // 
            // colCertThumbprint
            // 
            this.colCertThumbprint.Text = "Fingerabdruck";
            this.colCertThumbprint.Width = 120;
            // 
            // colCertIssuer
            // 
            this.colCertIssuer.Text = "Aussteller";
            this.colCertIssuer.Width = 180;
            // 
            // colCertExpiry
            // 
            this.colCertExpiry.Text = "Ablaufdatum";
            this.colCertExpiry.Width = 100;
            // 
            // colCertTrusted
            // 
            this.colCertTrusted.Text = "Vertrauenswürdig";
            this.colCertTrusted.Width = 110;
            // 
            // btnRefreshCerts
            // 
            this.btnRefreshCerts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefreshCerts.Location = new System.Drawing.Point(6, 332);
            this.btnRefreshCerts.Name = "btnRefreshCerts";
            this.btnRefreshCerts.Size = new System.Drawing.Size(100, 30);
            this.btnRefreshCerts.TabIndex = 1;
            this.btnRefreshCerts.Text = "Aktualisieren";
            this.btnRefreshCerts.UseVisualStyleBackColor = true;
            this.btnRefreshCerts.Click += new System.EventHandler(this.btnRefreshCerts_Click);
            // 
            // btnRemoveCert
            // 
            this.btnRemoveCert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveCert.Location = new System.Drawing.Point(112, 332);
            this.btnRemoveCert.Name = "btnRemoveCert";
            this.btnRemoveCert.Size = new System.Drawing.Size(100, 30);
            this.btnRemoveCert.TabIndex = 2;
            this.btnRemoveCert.Text = "Entfernen";
            this.btnRemoveCert.UseVisualStyleBackColor = true;
            this.btnRemoveCert.Click += new System.EventHandler(this.btnRemoveCert_Click);
            // 
            // btnToggleTrust
            // 
            this.btnToggleTrust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnToggleTrust.Location = new System.Drawing.Point(218, 332);
            this.btnToggleTrust.Name = "btnToggleTrust";
            this.btnToggleTrust.Size = new System.Drawing.Size(120, 30);
            this.btnToggleTrust.TabIndex = 3;
            this.btnToggleTrust.Text = "Vertrauen ändern";
            this.btnToggleTrust.UseVisualStyleBackColor = true;
            this.btnToggleTrust.Click += new System.EventHandler(this.btnToggleTrust_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearAll.Location = new System.Drawing.Point(344, 332);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(100, 30);
            this.btnClearAll.TabIndex = 4;
            this.btnClearAll.Text = "Alle löschen";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // txtProxyPort
            // 
            this.txtProxyPort.Location = new System.Drawing.Point(310, 16);
            this.txtProxyPort.Name = "txtProxyPort";
            this.txtProxyPort.Size = new System.Drawing.Size(60, 20);
            this.txtProxyPort.TabIndex = 4;
            this.txtProxyPort.Text = "8888";
            // 
            // lblProxyPort
            // 
            this.lblProxyPort.AutoSize = true;
            this.lblProxyPort.Location = new System.Drawing.Point(240, 19);
            this.lblProxyPort.Name = "lblProxyPort";
            this.lblProxyPort.Size = new System.Drawing.Size(64, 13);
            this.lblProxyPort.TabIndex = 5;
            this.lblProxyPort.Text = "Proxy-Port:";
            // 
            // lblProxyInfo
            // 
            this.lblProxyInfo.AutoSize = true;
            this.lblProxyInfo.Location = new System.Drawing.Point(12, 50);
            this.lblProxyInfo.Name = "lblProxyInfo";
            this.lblProxyInfo.Size = new System.Drawing.Size(500, 13);
            this.lblProxyInfo.TabIndex = 6;
            this.lblProxyInfo.Text = "Hinweis: Aktivieren Sie die automatische Umleitung für transparentes Monitoring.";
            // 
            // chkAutoRedirect
            // 
            this.chkAutoRedirect.AutoSize = true;
            this.chkAutoRedirect.Location = new System.Drawing.Point(390, 18);
            this.chkAutoRedirect.Name = "chkAutoRedirect";
            this.chkAutoRedirect.Size = new System.Drawing.Size(187, 17);
            this.chkAutoRedirect.TabIndex = 7;
            this.chkAutoRedirect.Text = "Automatische Traffic-Umleitung";
            this.chkAutoRedirect.UseVisualStyleBackColor = true;
            this.chkAutoRedirect.CheckedChanged += new System.EventHandler(this.chkAutoRedirect_CheckedChanged);
            // 
            // lblRedirectStatus
            // 
            this.lblRedirectStatus.AutoSize = true;
            this.lblRedirectStatus.Location = new System.Drawing.Point(583, 19);
            this.lblRedirectStatus.Name = "lblRedirectStatus";
            this.lblRedirectStatus.Size = new System.Drawing.Size(0, 13);
            this.lblRedirectStatus.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 490);
            this.Controls.Add(this.lblRedirectStatus);
            this.Controls.Add(this.chkAutoRedirect);
            this.Controls.Add(this.lblProxyInfo);
            this.Controls.Add(this.lblProxyPort);
            this.Controls.Add(this.txtProxyPort);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnStartStop);
            this.Name = "Form1";
            this.Text = "CertSec - SSL Zertifikat Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageMonitor.ResumeLayout(false);
            this.tabPageCertificates.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ListView listViewConnections;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colProcess;
        private System.Windows.Forms.ColumnHeader colHost;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.ColumnHeader colMessage;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageMonitor;
        private System.Windows.Forms.TabPage tabPageCertificates;
        private System.Windows.Forms.ListView listViewCertificates;
        private System.Windows.Forms.ColumnHeader colCertHost;
        private System.Windows.Forms.ColumnHeader colCertPort;
        private System.Windows.Forms.ColumnHeader colCertThumbprint;
        private System.Windows.Forms.ColumnHeader colCertIssuer;
        private System.Windows.Forms.ColumnHeader colCertExpiry;
        private System.Windows.Forms.ColumnHeader colCertTrusted;
        private System.Windows.Forms.Button btnRefreshCerts;
        private System.Windows.Forms.Button btnRemoveCert;
        private System.Windows.Forms.Button btnToggleTrust;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.TextBox txtProxyPort;
        private System.Windows.Forms.Label lblProxyPort;
        private System.Windows.Forms.Label lblProxyInfo;
        private System.Windows.Forms.CheckBox chkAutoRedirect;
        private System.Windows.Forms.Label lblRedirectStatus;
    }
}

