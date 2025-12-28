//Copyright 2025 Dennis Michael Heine
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace CertSec.Services
{
    public class TrafficRedirector
    {
        private const string FirewallRuleName = "CertSec_HTTPS_Redirect";
        private int _proxyPort;
        private bool _isActive = false;

        private static readonly HashSet<string> BrowserProcesses = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "chrome.exe", "firefox.exe", "msedge.exe", "iexplore.exe", 
            "opera.exe", "brave.exe", "vivaldi.exe", "safari.exe"
        };

        public TrafficRedirector(int proxyPort)
        {
            _proxyPort = proxyPort;
        }

        public bool Activate()
        {
            if (_isActive)
                return true;

            try
            {
                // Set system-wide proxy settings
                SetSystemProxy($"127.0.0.1:{_proxyPort}");

                // Create Windows Firewall rules
                CreateFirewallRules();

                _isActive = true;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Aktivieren der Traffic-Umleitung: {ex.Message}", ex);
            }
        }

        public bool Deactivate()
        {
            if (!_isActive)
                return true;

            try
            {
                // Remove system-wide proxy settings
                ClearSystemProxy();

                // Remove Windows Firewall rules
                RemoveFirewallRules();

                _isActive = false;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Deaktivieren der Traffic-Umleitung: {ex.Message}", ex);
            }
        }

        private void SetSystemProxy(string proxyAddress)
        {
            try
            {
                using (RegistryKey registry = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true))
                {
                    if (registry != null)
                    {
                        // Enable proxy
                        registry.SetValue("ProxyEnable", 1, RegistryValueKind.DWord);
                        registry.SetValue("ProxyServer", proxyAddress, RegistryValueKind.String);
                        
                        // Exclude browsers and local addresses
                        string bypass = GetBypassList();
                        registry.SetValue("ProxyOverride", bypass, RegistryValueKind.String);
                    }
                }

                // Notify system of proxy changes
                NotifyProxyChange();
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Setzen der Proxy-Einstellungen: {ex.Message}", ex);
            }
        }

        private void ClearSystemProxy()
        {
            try
            {
                using (RegistryKey registry = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true))
                {
                    if (registry != null)
                    {
                        // Disable proxy
                        registry.SetValue("ProxyEnable", 0, RegistryValueKind.DWord);
                    }
                }

                // Notify system of proxy changes
                NotifyProxyChange();
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Entfernen der Proxy-Einstellungen: {ex.Message}", ex);
            }
        }

        private string GetBypassList()
        {
            // Bypass local addresses and create exclusions for browser processes
            return "<local>;*.local;localhost;127.*;192.168.*";
        }

        private void CreateFirewallRules()
        {
            try
            {
                // Remove existing rule first
                RemoveFirewallRules();

                // Create outbound rule to redirect HTTPS traffic to proxy
                string command = $"netsh advfirewall firewall add rule " +
                    $"name=\"{FirewallRuleName}\" " +
                    $"dir=out " +
                    $"action=allow " +
                    $"protocol=TCP " +
                    $"remoteport=443 " +
                    $"enable=yes";

                ExecuteCommand(command);
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Erstellen der Firewall-Regeln: {ex.Message}", ex);
            }
        }

        private void RemoveFirewallRules()
        {
            try
            {
                string command = $"netsh advfirewall firewall delete rule name=\"{FirewallRuleName}\"";
                ExecuteCommand(command, ignoreErrors: true);
            }
            catch
            {
                // Ignore errors when removing non-existent rules
            }
        }

        private void ExecuteCommand(string command, bool ignoreErrors = false)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    Verb = "runas" // Run as administrator
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                    
                    if (!ignoreErrors && process.ExitCode != 0)
                    {
                        string error = process.StandardError.ReadToEnd();
                        throw new Exception($"Befehl fehlgeschlagen: {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ignoreErrors)
                    throw;
            }
        }

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);

        private const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        private const int INTERNET_OPTION_REFRESH = 37;

        private void NotifyProxyChange()
        {
            // Notify the system that proxy settings have changed
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        public bool IsActive()
        {
            return _isActive;
        }

        public static bool RequiresAdminRights()
        {
            return true;
        }

        public static bool IsRunningAsAdmin()
        {
            try
            {
                var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                var principal = new System.Security.Principal.WindowsPrincipal(identity);
                return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }
    }
}
