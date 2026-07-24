# EVENTS

## Purpose

Bu doküman projedeki tüm global eventleri tanımlar.

Sistemler birbirleriyle doğrudan haberleşmez.

Tüm iletişim Event Bus üzerinden yapılır.

---

# Game

- GameStarted
- GamePaused
- GameResumed
- GameRestarted
- GameExited

---

# Time

- TickStarted
- TickCompleted
- DayStarted
- DayEnded
- WeekStarted
- WeekEnded
- MonthStarted
- MonthEnded
- YearStarted
- YearEnded

---

# Save

- SaveStarted
- SaveCompleted
- SaveFailed
- LoadStarted
- LoadCompleted
- LoadFailed
- AutoSaveStarted
- AutoSaveCompleted

---

# Economy

- MoneyChanged
- RevenueChanged
- ExpenseChanged
- ProfitChanged
- CashFlowChanged
- TaxChanged
- InflationChanged
- InterestRateChanged
- ExchangeRateChanged
- LoanChanged
- NetWorthChanged

---

# Company

- CompanyCreated
- CompanyDeleted
- CompanyRenamed
- CompanyLevelChanged
- CompanyValueChanged
- CompanyRevenueChanged
- CompanyExpenseChanged
- CompanyProfitChanged
- CompanyBrandChanged
- CompanyMarketShareChanged
- CompanyAutomationChanged
- CompanyResearchUnlocked

---

# Branch

- BranchOpened
- BranchClosed
- BranchUpgraded
- BranchRevenueChanged
- BranchExpenseChanged

---

# Holding

- HoldingCreated
- HoldingRenamed
- HoldingLevelChanged
- HoldingValueChanged
- HoldingRevenueChanged
- HoldingExpenseChanged
- HoldingProfitChanged

---

# Employee

- EmployeeHired
- EmployeeFired
- EmployeePromoted
- EmployeeTransferred
- EmployeeSalaryChanged
- EmployeeSkillChanged
- EmployeeExperienceChanged
- EmployeeStressChanged
- EmployeeHappinessChanged
- EmployeeLoyaltyChanged

---

# Executive

- ExecutiveHired
- ExecutiveFired
- ExecutiveAssigned
- ExecutiveBonusChanged

---

# Research

- ResearchStarted
- ResearchCancelled
- ResearchCompleted
- ResearchUnlocked
- ResearchLevelChanged

---

# Market

- DemandChanged
- SupplyChanged
- CompetitionChanged
- MarketTrendChanged
- MarketShareChanged

---

# City

- CityUnlocked
- CityEntered
- CityLeft

---

# Country

- CountryUnlocked
- CountryEntered
- CountryLeft

---

# Stock Market

- IPOStarted
- IPOCompleted
- ShareBought
- ShareSold
- SharePriceChanged
- DividendPaid

---

# Acquisition

- AcquisitionStarted
- AcquisitionCompleted
- CompanyMerged
- CompanySold

---

# Office

- OfficeUpgraded
- OfficeBonusUnlocked

---

# Statistics

- StatisticsUpdated
- ReportGenerated

---

# Achievement

- AchievementUnlocked
- AchievementProgressChanged

---

# News

- NewsPublished

---

# Notification

- NotificationCreated
- NotificationDisplayed
- NotificationRemoved

---

# Audio

- MusicChanged
- MusicMuted
- SfxMuted
- VolumeChanged

---

# UI

- ScreenOpened
- ScreenClosed
- PopupOpened
- PopupClosed
- WindowFocused
- TabChanged
- TooltipShown
- TooltipHidden

---

# Settings

- LanguageChanged
- GraphicsChanged
- AudioSettingsChanged
- GameplaySettingsChanged

---

# Localization

- LocalizationLoaded
- LanguageLoaded

---

# Scene

- SceneLoading
- SceneLoaded
- SceneUnloading

---

# Debug

- DebugEnabled
- DebugDisabled

---

# Error

- ErrorOccurred
- CriticalErrorOccurred

---

# Event Naming

Pattern

<Entity><Action>

Examples

- CompanyCreated
- EmployeeHired
- MoneyChanged
- ResearchCompleted

---

# Event Rules

- Event isimleri geçmiş zaman (Past Tense) kullanılacak.
- Event isimleri benzersiz olacak.
- Event yalnızca gerçekleşen durumu temsil edecek.
- Event içerisinde iş mantığı bulunmayacak.
- Event sırası Tick sistemine uygun olacak.
- Event tetikleyen sistem sonucu dinlemeyecek.
- UI yalnızca Event dinleyecek.
- Domain yalnızca Event yayınlayacak.
- Event'ler idempotent olacak.
- Event payload'ı minimum veri taşıyacak.

---

# Event Flow

```text
System

↓

Publish Event

↓

Event Bus

↓

Subscribers

↓

UI / Managers / Systems
```

---

# Ownership

```text
Domain
    ↓
Publish
    ↓
Event Bus
    ↓
Subscribers
    ↓
Presentation
```