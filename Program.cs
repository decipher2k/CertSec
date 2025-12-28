//Copyright 2025 Dennis Michael Heine
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CertSec
{
    internal static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Check if running as administrator
            if (!IsRunningAsAdministrator())
            {
                // Restart the application with administrator rights
                RestartAsAdministrator();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static bool IsRunningAsAdministrator()
        {
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }

        private static void RestartAsAdministrator()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    WorkingDirectory = Environment.CurrentDirectory,
                    FileName = Application.ExecutablePath,
                    Verb = "runas" // Request administrator elevation
                };

                Process.Start(startInfo);
            }
            catch (Exception)
            {
                // User cancelled the UAC prompt or another error occurred
                MessageBox.Show(
                    "Diese Anwendung benötigt Administrator-Rechte, um vollständig zu funktionieren.\n\n" +
                    "Bitte starten Sie die Anwendung mit Rechtsklick → 'Als Administrator ausführen'.",
                    "Administrator-Rechte erforderlich",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
    }
}
