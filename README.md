# CertSec - SSL Certificate Monitor with Automatic Traffic Redirection

## Overview

CertSec is a Windows tool that monitors SSL/TLS certificates of non-browser applications and implements certificate pinning to protect against Man-in-the-Middle attacks.

## Main Features

### 1. Certificate Pinning (Trust on First Use - TOFU)
- Stores certificates on first contact with a server
- Validates certificate matching on future connections
- Automatically blocks connections with changed certificates

### 2. Intelligent Certificate Change Detection ?? NEW
When certificate changes are detected, a detailed dialog is displayed with:
- **?? Clear Warning** about potential hacker attack
- **Old/New Comparison**: Fingerprint, issuer, expiration date
- **IP Address Tracking**: Shows previous and current server IP
- **IP Change Warning**: Highlighting when the IP has changed
- **Two-Way Decision**:
  - ? **BLOCK** (Recommended): Connection is denied
  - ?? **Update** (Risk!): Accept new certificate with double confirmation

### 3. Automatic Traffic Redirection
- Automatically redirects HTTPS traffic (Port 443) from non-browser apps
- Uses Windows Firewall rules and system proxy settings
- Similar to Privoxy, but specifically for certificate pinning

### 4. Browser Exclusion
- Automatically filters web browsers (Chrome, Firefox, Edge, Opera, etc.)
- Focuses on other applications (update programs, desktop apps, etc.)

### 5. Persistent Storage
- Certificate database in `%APPDATA%\Roaming\CertSec\certificates.db`
- Event logs in `%APPDATA%\Roaming\CertSec\events.log`
- Survives program and system restarts

### 6. Connection Monitoring
- Real-time monitoring of all monitored connections
- Color-coded status display
- Detailed event logs

## Usage

### Installation
1. Compile the project in Visual Studio
2. Start `CertSec.exe` **as Administrator** (for automatic redirection)

### Basic Usage

#### Manual Mode (without automatic redirection)
1. Start CertSec
2. Click "Start"
3. Manually configure your applications to use proxy `127.0.0.1:8888`
4. CertSec now monitors all connections through this proxy

#### Automatic Mode (with traffic redirection)
1. Start CertSec **as Administrator**
2. Enter the desired proxy port (default: 8888)
3. Click "Start"
4. Enable "Automatic Traffic Redirection"
5. Confirm the security prompt
6. CertSec now automatically redirects all HTTPS traffic from non-browser apps

### Certificate Management

#### View Certificates
- Switch to the "Certificates" tab
- Here you can see all stored certificates with details

#### Remove Certificate
1. Select a certificate
2. Click "Remove"
3. On the next connection, the certificate will be learned anew

#### Change Trust Status
1. Select a certificate
2. Click "Change Trust"
3. Certificates marked as untrusted block all connections

#### Delete All Certificates
- Click "Delete All"
- Confirm the security prompt
- All certificates are removed and must be learned anew

## Status Codes

### Connection Status
- **Allowed** (Green): Connection allowed, certificate validated
- **NewCertificate** (Blue): New certificate learned (TOFU)
- **Blocked** (Red): Connection blocked (untrusted)
- **CertificateChanged** (Red): Certificate has changed - possible MITM attack!
- **CertificateExpired** (Orange): Certificate has expired
- **ValidationFailed** (Red): Certificate validation failed

## Security Notes

### Administrator Rights
Administrator rights are required for automatic traffic redirection because:
- Windows Firewall rules must be created
- System-wide proxy settings are changed
- Network traffic is redirected

### Firewall Rules
When automatic redirection is enabled, the following firewall rules are created:
- **Name**: `CertSec_HTTPS_Redirect`
- **Direction**: Outbound
- **Protocol**: TCP
- **Port**: 443
- **Action**: Allow (with redirection to proxy)

These rules are automatically removed when deactivated.

### System Proxy
When automatic redirection is enabled, system proxy settings are temporarily changed:
- **Proxy**: `127.0.0.1:8888` (or your configured port)
- **Bypass**: Local addresses and browser traffic

Settings are restored when deactivated.

## Technical Details

### Components
- **ProxyService**: Local HTTPS proxy for connection monitoring
- **CertificateStore**: Persistent storage of certificate data
- **CertificateValidator**: Validation and certificate pinning
- **TrafficRedirector**: Automatic redirection via firewall and system proxy
- **ProcessMonitor**: Identification of browser vs. non-browser processes

### Data Structure
```
%APPDATA%\Roaming\CertSec\
??? certificates.db    - Serialized certificate database
??? events.log         - Text-based event logs
```

### Supported Browsers (excluded)
- Google Chrome
- Mozilla Firefox
- Microsoft Edge
- Internet Explorer
- Opera
- Brave
- Vivaldi
- Safari

## Troubleshooting

### "Administrator Rights Required"
- Start CertSec with right-click ? "Run as administrator"

### "Port Already in Use"
- Change the proxy port to a free port (e.g., 8080, 8889)

### "Connection Fails"
- Check if the firewall is blocking CertSec
- Make sure the application is using the proxy

### "Certificate Constantly Changes"
- Some servers use load balancers with different certificates
- Disable monitoring for these hosts or mark as trusted

## Known Limitations

1. **Process Detection**: Process identification is based on netstat and is not 100% reliable
2. **Certificate Rotation**: Legitimate certificate changes require manual confirmation
3. **Performance**: With many simultaneous connections, delays may occur

## License

This project is for demonstration and security research purposes only.

## Warning

?? **Important**: This tool should only be used on your own systems. Intercepting and monitoring network traffic without consent may be illegal.
