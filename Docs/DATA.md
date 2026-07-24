# DATA

## Purpose

Bu doküman oyundaki tüm veri modellerini tanımlar.

Runtime veriler Data sınıflarında tutulur.

Static veriler ScriptableObject üzerinden okunur.

---

# Runtime Data

## PlayerData

- Id
- PlayerName
- Level
- Experience
- HoldingId
- PlayTime
- LastLogin
- Settings

---

## HoldingData

- Id
- Name
- Level
- Value
- Cash
- TotalRevenue
- TotalExpense
- TotalProfit
- TotalEmployees
- CompanyIds
- CountryIds
- CityIds
- ResearchIds
- AchievementIds

---

## CompanyData

- Id
- Name
- CompanyTypeId
- CityId
- CountryId
- Level
- Cash
- Revenue
- Expense
- Profit
- Value
- Brand
- MarketShare
- Automation
- Innovation
- Risk
- EmployeeIds
- BranchIds
- ExecutiveIds

---

## BranchData

- Id
- CompanyId
- CityId
- Level
- Revenue
- Expense
- Employees

---

## EmployeeData

- Id
- CompanyId
- EmployeeTypeId
- Name
- Level
- Salary
- Skill
- Experience
- Happiness
- Stress
- Loyalty
- Productivity

---

## ExecutiveData

- Id
- CompanyId
- ExecutiveTypeId
- Level
- Salary
- Bonus

---

## ResearchData

- Id
- Progress
- Level
- IsUnlocked
- RemainingTime

---

## EconomyData

- Revenue
- Expense
- Profit
- CashFlow
- Inflation
- InterestRate
- ExchangeRate
- TaxRate
- Loan
- NetWorth

---

## MarketData

- Demand
- Supply
- Competition
- Trend

---

## StatisticsData

- TotalRevenue
- TotalExpense
- TotalProfit
- TotalCompanies
- TotalBranches
- TotalEmployees
- TotalResearch
- TotalCities
- TotalCountries
- TotalPlayTime

---

## TimeData

- Tick
- Day
- Week
- Month
- Year

---

## NewsData

- Id
- Type
- Title
- Description
- Date

---

## NotificationData

- Id
- Type
- Message
- Time

---

## SaveData

- SaveVersion
- SaveDate
- PlayerData
- HoldingData
- CompanyData[]
- BranchData[]
- EmployeeData[]
- ExecutiveData[]
- ResearchData[]
- EconomyData
- StatisticsData
- TimeData

---

# Static Data

## CompanyTypeSO

- Id
- Name
- Category
- Description
- BaseCost
- BaseRevenue
- BaseExpense
- BaseGrowth
- UnlockLevel
- Icon

---

## EmployeeTypeSO

- Id
- Name
- Salary
- Productivity
- Skill
- Icon

---

## ExecutiveTypeSO

- Id
- Name
- BonusType
- BonusValue
- Salary
- Icon

---

## ResearchSO

- Id
- Name
- Category
- Cost
- Duration
- MaxLevel
- Prerequisites
- Bonus

---

## CitySO

- Id
- Name
- CountryId
- Population
- Tax
- Rent
- Workforce
- Demand
- Competition

---

## CountrySO

- Id
- Name
- Currency
- Tax
- Inflation
- InterestRate
- Stability
- Economy

---

## BuildingSO

- Id
- Name
- Level
- Cost
- Bonus

---

## EventSO

- Id
- Name
- Category
- Probability
- Duration
- Effects

---

## AchievementSO

- Id
- Name
- Description
- Reward
- Icon

---

## ConfigSO

### EconomyConfig

- StartingCash
- DefaultTax
- DefaultInflation
- DefaultInterest

### TimeConfig

- TickDuration
- DaysPerMonth
- MonthsPerYear

### GameplayConfig

- MaxCompanies
- MaxBranches
- MaxEmployees

### AudioConfig

- MusicVolume
- SfxVolume

### UIConfig

- AnimationDuration
- NotificationDuration

---

# Relationships

```text
Player
└── Holding
    ├── Companies
    │   ├── Branches
    │   ├── Employees
    │   ├── Executives
    │   └── Research
    ├── Statistics
    ├── Economy
    └── Achievements
```

---

# Runtime Ownership

```text
PlayerData
    ↓
HoldingData
    ↓
CompanyData
    ↓
BranchData
    ↓
EmployeeData
```

---

# Serialization

- Runtime Data → JSON
- Static Data → ScriptableObject
- SaveData → Root Object

---

# ID Rules

Her Runtime nesnesi benzersiz Id taşır.

İlişkiler yalnızca Id üzerinden kurulur.

Object Reference kullanılmaz.

---

# Versioning

SaveData

↓

Version

↓

Migration

↓

Current Version

---

# Validation

Runtime verileri oluşturulurken doğrulanır.

Save yüklenirken doğrulanır.

ScriptableObject referansları doğrulanır.

Eksik veri varsayılan Config üzerinden tamamlanır.