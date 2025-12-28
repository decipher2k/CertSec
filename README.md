# CertSec - SSL Zertifikat Monitor mit automatischer Traffic-Umleitung

## Übersicht

CertSec ist ein Windows-Tool, das SSL/TLS-Zertifikate von Nicht-Browser-Anwendungen überwacht und Certificate Pinning implementiert, um vor Man-in-the-Middle-Angriffen zu schützen.

## Hauptfunktionen

### 1. Certificate Pinning (Trust on First Use - TOFU)
- Speichert Zertifikate beim ersten Kontakt mit einem Server
- Validiert bei zukünftigen Verbindungen die Zertifikatsübereinstimmung
- Blockiert automatisch Verbindungen mit geänderten Zertifikaten

### 2. Intelligente Zertifikatsänderungs-Erkennung ?? NEU
Bei erkannten Zertifikatsänderungen wird ein detaillierter Dialog angezeigt mit:
- **?? Deutliche Warnung** vor potentiellem Hackerangriff
- **Vergleich Alt/Neu**: Fingerabdruck, Aussteller, Ablaufdatum
- **IP-Adressen-Tracking**: Zeigt vorherige und aktuelle Server-IP
- **IP-Wechsel-Warnung**: Hervorhebung wenn sich die IP geändert hat
- **Zwei-Wege-Entscheidung**:
  - ? **BLOCKIEREN** (Empfohlen): Verbindung wird verweigert
  - ?? **Aktualisieren** (Risiko!): Neues Zertifikat akzeptieren mit doppelter Bestätigung

### 3. Automatische Traffic-Umleitung
- Leitet HTTPS-Traffic (Port 443) von Nicht-Browser-Apps automatisch um
- Nutzt Windows Firewall-Regeln und System-Proxy-Einstellungen
- Ähnlich wie Privoxy, aber speziell für Certificate Pinning

### 4. Browser-Ausschluss
- Filtert automatisch Web-Browser (Chrome, Firefox, Edge, Opera, etc.)
- Konzentriert sich auf andere Anwendungen (Update-Programme, Desktop-Apps, etc.)

### 5. Persistente Speicherung
- Zertifikatsdatenbank in `%APPDATA%\Roaming\CertSec\certificates.db`
- Event-Logs in `%APPDATA%\Roaming\CertSec\events.log`
- Überleben Programm- und Systemneustarts

### 6. Verbindungsüberwachung
- Echtzeit-Monitoring aller überwachten Verbindungen
- Farbcodierte Status-Anzeige
- Detaillierte Event-Logs

## Verwendung

### Installation
1. Kompilieren Sie das Projekt in Visual Studio
2. Starten Sie `CertSec.exe` **als Administrator** (für automatische Umleitung)

### Grundlegende Nutzung

#### Manueller Modus (ohne automatische Umleitung)
1. Starten Sie CertSec
2. Klicken Sie auf "Start"
3. Konfigurieren Sie Ihre Anwendungen manuell, um den Proxy `127.0.0.1:8888` zu verwenden
4. CertSec überwacht nun alle Verbindungen über diesen Proxy

#### Automatischer Modus (mit Traffic-Umleitung)
1. Starten Sie CertSec **als Administrator**
2. Geben Sie den gewünschten Proxy-Port ein (Standard: 8888)
3. Klicken Sie auf "Start"
4. Aktivieren Sie "Automatische Traffic-Umleitung"
5. Bestätigen Sie die Sicherheitsabfrage
6. CertSec leitet nun automatisch allen HTTPS-Traffic von Nicht-Browser-Apps um

### Certificate Management

#### Zertifikate anzeigen
- Wechseln Sie zum Tab "Zertifikate"
- Hier sehen Sie alle gespeicherten Zertifikate mit Details

#### Zertifikat entfernen
1. Wählen Sie ein Zertifikat aus
2. Klicken Sie auf "Entfernen"
3. Bei der nächsten Verbindung wird das Zertifikat neu gelernt

#### Vertrauensstatus ändern
1. Wählen Sie ein Zertifikat aus
2. Klicken Sie auf "Vertrauen ändern"
3. Als nicht vertrauenswürdig markierte Zertifikate blockieren alle Verbindungen

#### Alle Zertifikate löschen
- Klicken Sie auf "Alle löschen"
- Bestätigen Sie die Sicherheitsabfrage
- Alle Zertifikate werden entfernt und müssen neu gelernt werden

## Status-Codes

### Verbindungsstatus
- **Allowed** (Grün): Verbindung erlaubt, Zertifikat validiert
- **NewCertificate** (Blau): Neues Zertifikat gelernt (TOFU)
- **Blocked** (Rot): Verbindung blockiert (nicht vertrauenswürdig)
- **CertificateChanged** (Rot): Zertifikat hat sich geändert - möglicher MITM-Angriff!
- **CertificateExpired** (Orange): Zertifikat ist abgelaufen
- **ValidationFailed** (Rot): Zertifikatsvalidierung fehlgeschlagen

## Sicherheitshinweise

### Administrator-Rechte
Für die automatische Traffic-Umleitung werden Administrator-Rechte benötigt, da:
- Windows Firewall-Regeln erstellt werden müssen
- System-weite Proxy-Einstellungen geändert werden
- Netzwerk-Traffic umgeleitet wird

### Firewall-Regeln
Bei aktivierter automatischer Umleitung werden folgende Firewall-Regeln erstellt:
- **Name**: `CertSec_HTTPS_Redirect`
- **Richtung**: Ausgehend
- **Protokoll**: TCP
- **Port**: 443
- **Aktion**: Allow (mit Umleitung zum Proxy)

Diese Regeln werden beim Deaktivieren automatisch entfernt.

### System-Proxy
Bei aktivierter automatischer Umleitung werden die System-Proxy-Einstellungen temporär geändert:
- **Proxy**: `127.0.0.1:8888` (oder Ihr konfigurierter Port)
- **Bypass**: Lokale Adressen und Browser-Traffic

Die Einstellungen werden beim Deaktivieren wiederhergestellt.

## Technische Details

### Komponenten
- **ProxyService**: Lokaler HTTPS-Proxy für Verbindungsüberwachung
- **CertificateStore**: Persistente Speicherung von Zertifikatsdaten
- **CertificateValidator**: Validierung und Certificate Pinning
- **TrafficRedirector**: Automatische Umleitung via Firewall und System-Proxy
- **ProcessMonitor**: Identifikation von Browser- vs. Nicht-Browser-Prozessen

### Datenstruktur
```
%APPDATA%\Roaming\CertSec\
??? certificates.db    - Serialisierte Zertifikatsdatenbank
??? events.log         - Text-basierte Event-Logs
```

### Unterstützte Browser (werden ausgeschlossen)
- Google Chrome
- Mozilla Firefox
- Microsoft Edge
- Internet Explorer
- Opera
- Brave
- Vivaldi
- Safari

## Fehlerbehebung

### "Administrator-Rechte erforderlich"
- Starten Sie CertSec mit Rechtsklick ? "Als Administrator ausführen"

### "Port bereits in Verwendung"
- Ändern Sie den Proxy-Port auf einen freien Port (z.B. 8080, 8889)

### "Verbindung schlägt fehl"
- Überprüfen Sie, ob die Firewall CertSec blockiert
- Stellen Sie sicher, dass die Anwendung den Proxy verwendet

### "Zertifikat ändert sich ständig"
- Manche Server verwenden Load Balancer mit verschiedenen Zertifikaten
- Deaktivieren Sie das Monitoring für diese Hosts oder markieren Sie als vertrauenswürdig

## Bekannte Einschränkungen

1. **Process Detection**: Die Prozess-Identifikation basiert auf netstat und ist nicht 100% zuverlässig
2. **Certificate Rotation**: Legitime Zertifikatswechsel erfordern manuelle Bestätigung
3. **Performance**: Bei vielen gleichzeitigen Verbindungen kann es zu Verzögerungen kommen

## Lizenz

Dieses Projekt dient nur zu Demonstrations- und Sicherheitsforschungszwecken.

## Warnung

?? **Wichtig**: Dieses Tool sollte nur auf Ihren eigenen Systemen verwendet werden. Das Abfangen und Überwachen von Netzwerk-Traffic ohne Zustimmung kann illegal sein.
