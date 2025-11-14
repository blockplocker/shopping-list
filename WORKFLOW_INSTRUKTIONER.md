# 🚀 GitHub Actions Workflow Instruktioner

---

## 📖 Översikt
I denna laboration ska ni skapa EN GitHub Actions workflow som demonstrerar förståelse för CI/CD pipelines. Du kommer att implementera olika triggers, matrix builds och context variables.

## 🎯 Lärandemål
I denna uppgift kommer testa att ni kan:
- *sätta upp enklare pipelines för automatisering av testning, integration och produktionssättning (för ett givet projekt). hantera förgrening i versionshanteringsverktyg*

Och det gör vi genom att ni får visa att ni kan:

- ✅ Konfigurera olika workflow triggers
- ✅ Implementera matrix strategies för cross-platform builds
- ✅ Arbeta med context variables och JSON output
- ✅ Bygga och testa .NET applikationer automatiskt
- ✅ Använda workflow inputs och environments (valfritt)

---

## 📋 Uppgift: CI Matrix Workflow

**Mål:** Skapa en automatiserad CI/CD pipeline som bygger och testar er shopping list-applikation på olika plattformar och konfigurationer.

Skapa en workflow-fil `.github/workflows/ci.yml` som uppfyller följande krav:

### **1. Triggers**
Workflow ska triggas av:
- Manuell körning
- Push till `main` branch
- När pull requests öppnas mot main

### **2. Matrix Strategy**
Använd matrix för att köra jobbet på:
- Operativsystem: Linux och Windows
- .NET versioner: 8 och 9

### **3. Standard CI/CD Steg**
Implementera ett workflow som låter dig:

1. **Bygga källkoden**
2. **Köra testerna** - Exekvera enhetstester, men BARA om build steget lyckades

### **4. Context Intelligence**
Implementera ett "smart" kontext-steg som:
- Detekterar och visar endast RELEVANT information baserat på trigger
  - Vid PR: visa PR number, author, source/target branch
  - Vid push: visa commit message, author, changed files (om möjligt)
  - Vid manuell: visa vem som startade och med vilka inputs
- Använd conditional expressions (`if:`) för att anpassa output
- Formatera outputen på ett tydligt sätt (inte bara rå JSON dump)

### **5. (Bonus) - Discord Webhook Notification**
Skicka en notifikation till Discord när workflow är klar:
- Skapa ett steg som kör **alltid** (även vid failure) med `if: always()`
- Använd Discord webhook för att posta ett meddelande
- Meddelandet ska innehålla:
  - Workflow status (✅ Success / ❌ Failed)
  - Repository namn och branch
  - Commit message och author
  - Länk till workflow run
- Tips: Använd `curl` med Discord webhook URL (sparad som secret)
- Bonus: Olika färger för success/failure (Discord embed colors)

---

## ✅ Acceptanskriterier

### **Grundläggande (Godkänt):**
- [ ] Workflow-fil existerar på korrekt plats (`.github/workflows/ci.yml`)
- [ ] Alla tre triggers fungerar (manuell, push till main, pull request)
- [ ] Matrix build körs för alla 4 kombinationer (2 OS × 2 .NET versioner)
- [ ] Build-steget lyckas för alla matrix-kombinationer
- [ ] Test-steget körs endast om build lyckas
- [ ] Test-steget passerar för alla matrix-kombinationer
- [ ] Context intelligence visar rätt information baserat på trigger typ
- [ ] (Bonus) - Discord webhook skickar notifikation med korrekt status och information

---

## 💡 Implementeringstips

- Börja enkelt med bara triggers och matrix, bygg sedan ut steg för steg
- Testa workflow med manuell trigger först
- Kontrollera syntax noga - YAML är känsligt för indentation
- Använd "Actions" tab i GitHub för att se detaljerade loggar
- Matrix skapar 4 separata jobb (2 OS × 2 .NET versioner)
- Läs GitHub Actions dokumentationen för att hitta fler funktioner och möjligheter

**Lycka till! 🚀**

---

## 🔶 Valfria Utökningar som vore kul att se 😊
Här är idéer på extra funktionaliteter som ni antigen kan implementera i samma workflow eller i nya.

### **Path Filtering (Smart Trigger)**
- Konfigurera workflow att endast köras vid ändringar i kodfiler
- Använd `paths` eller `paths-ignore` för att exkludera dokumentationsfiler:
  - Kör **EJ** på ändringar av `.md` filer (README, dokumentation)
  - Kör **EJ** på ändringar av `.github/workflows/` ändringar (undvik loop)
  - Kör på ändringar av källkodsfiler (`.cs`, `.csproj`, etc.)


### **Environment Simulation**
- Konfigurera jobbet att köra i den miljö som specificeras via input
- Skriv ut vilket environment som används: "Running in environment: [värde]"
- Hantera fallback till 'development' om ingen input ges

### **Kodformatering**
- Lägg till validering av kodformatering (`dotnet format --verify-no-changes`)
- Workflowet ska faila om koden inte är korrekt formaterad

### **Build Performance Metrics**
- Mät och jämför byggtiden mellan olika OS/konfigurationer
- Använd timestamps för att logga duration för build och test steg
- Skapa en sammanfattning som visar snabbaste/långsammaste kombinationen
- Tips: Använd `date` command eller GitHub Actions `time` för mätning

### **Build Artifacts**
- Generera och ladda upp build artifacts (använd `actions/upload-artifact`)
- Skapa OS-specifika artifact-namn (t.ex. `app-windows`, `app-linux`)
- Sätt olika retention policies (t.ex. 7 dagar för dev, 30 dagar för prod)
- Bonus: Ladda ner artifacts i ett senare job och verifiera dem

### **Failure Handling & Notifications**
- Lägg till ett steg som kan faila (för testing av error handling)
- Implementera `continue-on-error` för detta steg
- Skapa en job summary som visar vilka steg som failade
- Bonus: Använd `actions/github-script` för att skapa en snygg sammanfattning

### **Conditional Workflows (Smart Skipping)**
- Implementera logik för att skippa vissa jobs baserat på conditions:
  - Skippa Windows builds om commit message innehåller `[linux-only]`
  - Använd `paths` filter för att endast köra vid ändringar i relevanta filer
  - Implementera "skip CI" logic om commit innehåller `[skip ci]`
- Tips: Utforska `if:` conditions och `github.event.head_commit.message`

### **Job Dependencies & Pipeline Stages**
- Skapa ett "lint" job som måste lyckas innan build körs
- Lägg till ett "report" job som sammanfattar alla matrix results
- Implementera `needs:` dependencies mellan jobs
- Bonus: Skapa ett deployment-steg som endast körs om alla tester passerar

### **💡 Egna Kreativa Tillägg:**
Utöver ovanstående utökningar uppmuntrar vi er att tänka utanför boxen! Lägg gärna till egna funktioner som tillför värde till er CI/CD pipeline - t.ex. custom badges, test coverage visualisering, automatisk versioning, säkerhetsscanning, Docker images, eller något helt annat. Kvalitet över kvantitet - ett väl implementerat tillägg är bättre än flera halvfärdiga funktioner!