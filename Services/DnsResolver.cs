//Copyright 2025 Dennis Michael Heine
using System;
using System.Net;
using System.Net.Sockets;

namespace CertSec.Services
{
    public class DnsResolver
    {
        public static string ResolveHostToIp(string hostname)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(hostname);
                
                if (hostEntry.AddressList.Length > 0)
                {
                    foreach (var address in hostEntry.AddressList)
                    {
                        if (address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return address.ToString();
                        }
                    }
                    
                    return hostEntry.AddressList[0].ToString();
                }
                
                return "Unknown";
            }
            catch (Exception)
            {
                return "Unknown";
            }
        }

        public static string GetMultipleIpAddresses(string hostname)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(hostname);
                
                if (hostEntry.AddressList.Length > 0)
                {
                    var ipAddresses = new System.Collections.Generic.List<string>();
                    
                    foreach (var address in hostEntry.AddressList)
                    {
                        if (address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddresses.Add(address.ToString());
                        }
                    }
                    
                    if (ipAddresses.Count > 0)
                    {
                        return string.Join(", ", ipAddresses);
                    }
                    
                    return hostEntry.AddressList[0].ToString();
                }
                
                return "Unknown";
            }
            catch (Exception)
            {
                return "Unknown";
            }
        }
    }
}
