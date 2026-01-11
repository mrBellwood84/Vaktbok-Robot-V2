# Vaktbok Robot V2

Vaktbok Robot V2 er en konsollbasert .NET-applikasjon for automatisert innhenting, sammenligning og dokumentasjon av vaktplaner fra webbaserte systemer.

Applikasjonen er utviklet for praktisk bruk og kombinerer:
- UI-automatisering
- Databaselagring av endringer
- Manuell dokumentasjon via PDF-utskrift

---

## Funksjonalitet

Applikasjonen tilbyr tre hovedfunksjoner:

### 1. Login session
Starter en innlogget sesjon mot relevante systemer og klargjør applikasjonen for videre bruk.  
Denne brukes både for manuell kjøring og som forberedelse til automatiserte operasjoner.

### 2. Automatisert kontroll av vaktplaner
Applikasjonen kan kjøre automatisk gjennom alle uker i en angitt tidsperiode, lese vaktplaner og sammenligne disse med tidligere registrerte data.

Eventuelle endringer lagres i database.  
Tidsperioden for denne kjøringen konfigureres i `appsettings.json`.

### 3. Lagring av vaktplaner som PDF
Vaktplaner kan lagres som PDF ved hjelp av nettleserens innebygde print-funksjon.

Denne funksjonen er **halvautomatisk**:
- Applikasjonen blar automatisk mellom uker
- Brukeren lagrer PDF manuelt via print-dialogen

Dette er en bevisst designbeslutning, da nettleserens print-dialog ikke kan styres stabilt fra Playwright på tvers av operativsystemer.

---

## Installasjon

### Forutsetninger

- .NET SDK
- MySQL-server
- Tilgang til relevante interne systemer
- Nettleser støttet av Playwright

---

### 1. Klon repoet

```bash
git clone <repo-url>
```

---

### 2. Installer Playwright

Playwright **må installeres manuelt** før applikasjonen kan kjøres.

#### Windows
```powershell
./install-playwright.ps1
```

#### Linux
```bash
./install-playwright.sh
```

---

### 3. Databaseoppsett

Databasen settes opp ved hjelp av SQL-skript og DbUp.

#### Manuell initiering

Kjør følgende skript manuelt:

- `0001_CreateDatabase.sql`

Dette skriptet:
- Oppretter databasen
- Oppretter nødvendige brukere
- Setter riktige rettigheter

Det forutsettes at en eksisterende **root-bruker allerede finnes** i MySQL.

---

### 4. Konfigurasjon

#### `appsettings.secret.json`

```json
{
  "Credentials": {
    "IHelseUser": "",
    "IHelsePassword": "",
    "GatUser": "",
    "GatPassword": ""
  },
  "Urls": {
    "Entry": "",
    "LoginSleep": ""
  },
  "ConnectionStrings": {
    "Root": "Server=localhost;Database=Vaktbok_2;Uid=root;Pwd=root;",
    "Robot": "Server=localhost;Database=Vaktbok_2;Uid=robot_user;Pwd=robot_user_password;"
  },
  "FileSettings": {
    "DocumentDirectory": "path/to/documents"
  }
}
```

---

## Dokumentstruktur

### faksimile
- Lagres per ukenummer

### pdfprint
- Lagres per utskriftsdato

---