# Vaktbok Robot V2

Vaktbok Robot V2 er et verktøy laget for å overvåke vaktlister fra MinGat/Visma, med fokus på å oppdage endringer som ellers ikke er synlige for standard brukerkontoer i systemet.

Denne scraperen roboten er designet opp mot de interne nettsidene i Helse Bergen.

---

## Hovedfunksjoner

1. **Login**
    - Logger inn på MinGat og opprettholder en aktiv browser-session.
    - Gir andre bots tilgang til samme session uten å måtte logge inn flere ganger.

2. **Scraper**
    - Henter data fra vaktlistene og identifiserer endringer fra tidligere innhentet data.
    - Kan brukes til å holde oversikt over vaktplaner og dokumentere eventuelle justeringer.

---
## `appsettings.secret.json`

Vaktbok Robot V2 krever en **hemmelig settings-fil** som inneholder sensitive opplysninger som brukernavn og passord.

- `IHelseUser`, `IHelsePassword`: for innlogging til intranettet.
- `GatUser`, `GatPasword`:for innlogging til Gat
- `Urls.Entry`: Startside for robot.
- `Urls.LoginSleep`: Venteside for tofaktor autentifisering.
```json
{
  "Credentials" : {
    "IHelseUser": "", 
    "IHelsePassword": "",
    "GatUser": "",
    "GatPassword": ""
  },
  "Urls": {
    "Entry": "",
    "LoginSleep": ""
  }
}
