# TaskFlow

TaskFlow är en fullstack-applikation byggd för att strukturera och hantera kategorier och tillhörande uppgifter. Projektet är separerat i en clean architecture och driftsatt live i molnet.

* **Frontend Live-länk:** https://taskflow-i03y.onrender.com

---

## Arkitektur

Projektet är strikt strukturerat enligt principerna för **Clean Architecture** för att säkerställa hög testbarhet, separation av ansvarsområden samt skalbarhet.

- **Domain:** Innehåller entiteter (`Category`, `TaskItem`) samt affärsobjekten.
- **Application:** Innehåller DTOs, interfaces för tjänster samt affärslogik.
- **Infrastructure:** Hanterar databaskontext (`DbContext`), EF Core-migrationer samt implementationer av Repository-mönstret (inklusive Generic Repository)
- **Web API (Backend):** Projektets backend-del som tar emot alla anrop. För att koden ska vara lätt att ändra och testa använder alla controllers interfaces istället för fasta klasser. All kommunikation sker via async/await för att appen ska vara snabb och effektiv.
- **Blazor Server (Frontend):** Appens ansikte utåt. Här kan användaren genom CRUD-funktioner se, skapa, uppdatera och ta bort data. Allt uppdateras direkt på skärmen och skickas vidare till API:et i bakgrunden.
- **TestProject:** Ett separat testprojekt uppbyggt med **xUnit** och **NSubstitute**. Med hjälp av NSubstitute skapar vi smarta "ersättare" för databasen. På så sätt kan vi testa alla tjänster i en säker, isolerad miljö utan att behöva röra den riktiga databasen.

---

##  Molndriftsättning & Databas
- **Frontend/Hosting:** Deployad live på **Render**. Blazor Server-miljön körs stabilt mot Renders Linux-containrar via WebSockets och kommunicerar internt via miljövariablerna (`PORT` / `ASPNETCORE_URLS`).
- **Databas:** Driftsatt via **Azure SQL Database**. Entity Framework Core används för att köra Code First-migrations mot molnet. Anslutningen har säkrats upp på Renders servermiljö med explicit kryptering och certifikatvalidering (`Encrypt=True;TrustServerCertificate=True`).

---

## Instruktioner för lokal körning
Eftersom molndatabasen är skyddad av brandväggar i produktion, körs projektet bäst mot en lokal SQL Server när det ska testas.

### 1. Klona arkivet
git clone <https://github.com/tingru00/TaskFlow.git>

### 2. Ställ in lokal databas (Connection String)
Ex. {
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TaskFlowDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
### 3. Skapa databasen (Migrations)
I Package Manager Console: Update-Database

## Lärdomar från projektet
Under arbetet med denna inlämningsuppgift har jag fördjupat mina kunskaper inom flera områden:

- Clean Architecture: Även om jag har använt arkitekturen tidigare fick jag verkligen brottas med strukturen den här gången. Eftersom jag råkade starta utan en Blank Solution fick jag göra om projektet och dela upp det i rätt lager i efterhand. Processen gav mig inte bara en bättre förståelse för hur lagren hänger ihop, utan också en helt annan trygghet i strukturen och i hur jag tar mig an och arbetar med den framöver.


- Isolerad enhetstestning: Genom xUnit och NSubstitute har jag lärt mig att skriva enhetstester i en helt isolerad miljö. Det blev tydligt hur mycket lättare det blir att testa affärslogik när koden följer Clean Architecture.  


- Cloudmiljöns utmaningar: Driftsättningen på Render och anslutningen till Azure SQL var det mest utmanande men också det mest lärorika.
