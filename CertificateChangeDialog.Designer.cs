//Copyright 2025 Dennis Michael Heine
namespace CertSec
{
    partial class CertificateChangeDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblWarningTitle = new System.Windows.Forms.Label();
            this.pictureBoxWarning = new System.Windows.Forms.PictureBox();
            this.lblWarningMessage = new System.Windows.Forms.Label();
            this.groupBoxOldCert = new System.Windows.Forms.GroupBox();
            this.lblOldThumbprint = new System.Windows.Forms.Label();
            this.lblOldIssuer = new System.Windows.Forms.Label();
            this.lblOldExpiry = new System.Windows.Forms.Label();
            this.lblOldIP = new System.Windows.Forms.Label();
            this.groupBoxNewCert = new System.Windows.Forms.GroupBox();
            this.lblNewThumbprint = new System.Windows.Forms.Label();
            this.lblNewIssuer = new System.Windows.Forms.Label();
            this.lblNewExpiry = new System.Windows.Forms.Label();
            this.lblNewIP = new System.Windows.Forms.Label();
            this.btnBlock = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblHostname = new System.Windows.Forms.Label();
            this.lblProcess = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWarning)).BeginInit();
            this.groupBoxOldCert.SuspendLayout();
            this.groupBoxNewCert.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWarningTitle
            // 
            this.lblWarningTitle.AutoSize = true;
            this.lblWarningTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarningTitle.ForeColor = System.Drawing.Color.Red;
            this.lblWarningTitle.Location = new System.Drawing.Point(70, 20);
            this.lblWarningTitle.Name = "lblWarningTitle";
            this.lblWarningTitle.Size = new System.Drawing.Size(421, 24);
            this.lblWarningTitle.TabIndex = 0;
            this.lblWarningTitle.Text = "?? SICHERHEITSWARNUNG - ANGRIFF?";
            // 
            // pictureBoxWarning
            // 
            this.pictureBoxWarning.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxWarning.Name = "pictureBoxWarning";
            this.pictureBoxWarning.Size = new System.Drawing.Size(48, 48);
            this.pictureBoxWarning.TabIndex = 1;
            this.pictureBoxWarning.TabStop = false;
            // 
            // lblWarningMessage
            // 
            this.lblWarningMessage.Location = new System.Drawing.Point(12, 55);
            this.lblWarningMessage.Name = "lblWarningMessage";
            this.lblWarningMessage.Size = new System.Drawing.Size(560, 60);
            this.lblWarningMessage.TabIndex = 2;
            this.lblWarningMessage.Text = "Das SSL-Zertifikat für diese Verbindung hat sich geändert!\r\n\r\nDies könnte ein Ze" +
    "ichen für einen Man-in-the-Middle-Angriff sein. Überprüfen Sie die Details sorg" +
    "fältig, bevor Sie das Zertifikat akzeptieren.";
            // 
            // groupBoxOldCert
            // 
            this.groupBoxOldCert.Controls.Add(this.lblOldIP);
            this.groupBoxOldCert.Controls.Add(this.lblOldExpiry);
            this.groupBoxOldCert.Controls.Add(this.lblOldIssuer);
            this.groupBoxOldCert.Controls.Add(this.lblOldThumbprint);
            this.groupBoxOldCert.Location = new System.Drawing.Point(12, 160);
            this.groupBoxOldCert.Name = "groupBoxOldCert";
            this.groupBoxOldCert.Size = new System.Drawing.Size(560, 140);
            this.groupBoxOldCert.TabIndex = 3;
            this.groupBoxOldCert.TabStop = false;
            this.groupBoxOldCert.Text = "Vorheriges Zertifikat (Gespeichert)";
            // 
            // lblOldThumbprint
            // 
            this.lblOldThumbprint.AutoSize = true;
            this.lblOldThumbprint.Location = new System.Drawing.Point(10, 25);
            this.lblOldThumbprint.Name = "lblOldThumbprint";
            this.lblOldThumbprint.Size = new System.Drawing.Size(85, 13);
            this.lblOldThumbprint.TabIndex = 0;
            this.lblOldThumbprint.Text = "Fingerabdruck: ";
            // 
            // lblOldIssuer
            // 
            this.lblOldIssuer.AutoSize = true;
            this.lblOldIssuer.Location = new System.Drawing.Point(10, 50);
            this.lblOldIssuer.Name = "lblOldIssuer";
            this.lblOldIssuer.Size = new System.Drawing.Size(59, 13);
            this.lblOldIssuer.TabIndex = 1;
            this.lblOldIssuer.Text = "Aussteller: ";
            // 
            // lblOldExpiry
            // 
            this.lblOldExpiry.AutoSize = true;
            this.lblOldExpiry.Location = new System.Drawing.Point(10, 75);
            this.lblOldExpiry.Name = "lblOldExpiry";
            this.lblOldExpiry.Size = new System.Drawing.Size(76, 13);
            this.lblOldExpiry.TabIndex = 2;
            this.lblOldExpiry.Text = "Ablaufdatum: ";
            // 
            // lblOldIP
            // 
            this.lblOldIP.AutoSize = true;
            this.lblOldIP.Location = new System.Drawing.Point(10, 100);
            this.lblOldIP.Name = "lblOldIP";
            this.lblOldIP.Size = new System.Drawing.Size(68, 13);
            this.lblOldIP.TabIndex = 3;
            this.lblOldIP.Text = "IP-Adresse: ";
            // 
            // groupBoxNewCert
            // 
            this.groupBoxNewCert.Controls.Add(this.lblNewIP);
            this.groupBoxNewCert.Controls.Add(this.lblNewExpiry);
            this.groupBoxNewCert.Controls.Add(this.lblNewIssuer);
            this.groupBoxNewCert.Controls.Add(this.lblNewThumbprint);
            this.groupBoxNewCert.Location = new System.Drawing.Point(12, 310);
            this.groupBoxNewCert.Name = "groupBoxNewCert";
            this.groupBoxNewCert.Size = new System.Drawing.Size(560, 140);
            this.groupBoxNewCert.TabIndex = 4;
            this.groupBoxNewCert.TabStop = false;
            this.groupBoxNewCert.Text = "Neues Zertifikat (Aktuell empfangen)";
            // 
            // lblNewThumbprint
            // 
            this.lblNewThumbprint.AutoSize = true;
            this.lblNewThumbprint.Location = new System.Drawing.Point(10, 25);
            this.lblNewThumbprint.Name = "lblNewThumbprint";
            this.lblNewThumbprint.Size = new System.Drawing.Size(85, 13);
            this.lblNewThumbprint.TabIndex = 0;
            this.lblNewThumbprint.Text = "Fingerabdruck: ";
            // 
            // lblNewIssuer
            // 
            this.lblNewIssuer.AutoSize = true;
            this.lblNewIssuer.Location = new System.Drawing.Point(10, 50);
            this.lblNewIssuer.Name = "lblNewIssuer";
            this.lblNewIssuer.Size = new System.Drawing.Size(59, 13);
            this.lblNewIssuer.TabIndex = 1;
            this.lblNewIssuer.Text = "Aussteller: ";
            // 
            // lblNewExpiry
            // 
            this.lblNewExpiry.AutoSize = true;
            this.lblNewExpiry.Location = new System.Drawing.Point(10, 75);
            this.lblNewExpiry.Name = "lblNewExpiry";
            this.lblNewExpiry.Size = new System.Drawing.Size(76, 13);
            this.lblNewExpiry.TabIndex = 2;
            this.lblNewExpiry.Text = "Ablaufdatum: ";
            // 
            // lblNewIP
            // 
            this.lblNewIP.AutoSize = true;
            this.lblNewIP.Location = new System.Drawing.Point(10, 100);
            this.lblNewIP.Name = "lblNewIP";
            this.lblNewIP.Size = new System.Drawing.Size(68, 13);
            this.lblNewIP.TabIndex = 3;
            this.lblNewIP.Text = "IP-Adresse: ";
            // 
            // btnBlock
            // 
            this.btnBlock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnBlock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBlock.ForeColor = System.Drawing.Color.White;
            this.btnBlock.Location = new System.Drawing.Point(12, 465);
            this.btnBlock.Name = "btnBlock";
            this.btnBlock.Size = new System.Drawing.Size(270, 40);
            this.btnBlock.TabIndex = 5;
            this.btnBlock.Text = "? BLOCKIEREN (Empfohlen)";
            this.btnBlock.UseVisualStyleBackColor = false;
            this.btnBlock.Click += new System.EventHandler(this.btnBlock_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(302, 465);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(270, 40);
            this.btnUpdate.TabIndex = 6;
            this.btnUpdate.Text = "?? Zertifikat aktualisieren (Risiko!)";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblHostname
            // 
            this.lblHostname.AutoSize = true;
            this.lblHostname.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHostname.Location = new System.Drawing.Point(12, 125);
            this.lblHostname.Name = "lblHostname";
            this.lblHostname.Size = new System.Drawing.Size(90, 15);
            this.lblHostname.TabIndex = 7;
            this.lblHostname.Text = "Verbindung: ";
            // 
            // lblProcess
            // 
            this.lblProcess.AutoSize = true;
            this.lblProcess.Location = new System.Drawing.Point(12, 145);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Size = new System.Drawing.Size(51, 13);
            this.lblProcess.TabIndex = 8;
            this.lblProcess.Text = "Prozess: ";
            // 
            // CertificateChangeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(584, 521);
            this.Controls.Add(this.lblProcess);
            this.Controls.Add(this.lblHostname);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnBlock);
            this.Controls.Add(this.groupBoxNewCert);
            this.Controls.Add(this.groupBoxOldCert);
            this.Controls.Add(this.lblWarningMessage);
            this.Controls.Add(this.pictureBoxWarning);
            this.Controls.Add(this.lblWarningTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CertificateChangeDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CertSec - Zertifikatsänderung erkannt";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWarning)).EndInit();
            this.groupBoxOldCert.ResumeLayout(false);
            this.groupBoxOldCert.PerformLayout();
            this.groupBoxNewCert.ResumeLayout(false);
            this.groupBoxNewCert.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblWarningTitle;
        private System.Windows.Forms.PictureBox pictureBoxWarning;
        private System.Windows.Forms.Label lblWarningMessage;
        private System.Windows.Forms.GroupBox groupBoxOldCert;
        private System.Windows.Forms.Label lblOldIP;
        private System.Windows.Forms.Label lblOldExpiry;
        private System.Windows.Forms.Label lblOldIssuer;
        private System.Windows.Forms.Label lblOldThumbprint;
        private System.Windows.Forms.GroupBox groupBoxNewCert;
        private System.Windows.Forms.Label lblNewIP;
        private System.Windows.Forms.Label lblNewExpiry;
        private System.Windows.Forms.Label lblNewIssuer;
        private System.Windows.Forms.Label lblNewThumbprint;
        private System.Windows.Forms.Button btnBlock;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lblHostname;
        private System.Windows.Forms.Label lblProcess;
    }
}
