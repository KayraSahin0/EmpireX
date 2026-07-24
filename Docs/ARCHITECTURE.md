# ARCHITECTURE

## Layers

```text
Presentation
      │
      ▼
Application
      │
      ▼
Domain
      │
      ▼
Infrastructure
```

---

# Layer Responsibilities

## Presentation

UI

Views

ViewModels

Panels

Widgets

Popups

HUD

Input

Animation

Navigation

---

## Application

Game Flow

Use Cases

Commands

Queries

Services

Validation

Coordination

---

## Domain

Business Rules

Economy

Holding

Company

Employee

Research

Market

Statistics

Simulation

---

## Infrastructure

Save

Audio

Localization

Addressables

Cloud

Database

Platform Services

File System

Analytics

---

# Dependency Rule

```text
Presentation
      │
      ▼
Application
      │
      ▼
Domain
      │
      ▼
Infrastructure
```

Bağımlılık yönü yalnızca aşağı doğrudur.

---

# Core Flow

```text
Input

↓

View

↓

ViewModel

↓

Use Case

↓

Domain

↓

Events

↓

ViewModel

↓

View
```

---

# Simulation Flow

```text
Tick

↓

Time

↓

Economy

↓

Companies

↓

Employees

↓

Research

↓

Branches

↓

Holding

↓

Market

↓

Statistics

↓

News

↓

Notifications

↓

Autosave
```

---

# Event Flow

```text
System

↓

Event Bus

↓

Subscribers
```

---

# Save Flow

```text
Runtime Data

↓

Save Service

↓

Serializer

↓

JSON

↓

Storage
```

---

# Load Flow

```text
Storage

↓

JSON

↓

Deserializer

↓

Runtime Data

↓

Initialize

↓

UI Refresh
```

---

# UI Flow

```text
View

↓

ViewModel

↓

Use Case

↓

Domain

↓

Events

↓

ViewModel

↓

View
```

---

# Game Loop

```text
Launch

↓

Initialize

↓

Load Config

↓

Load Save

↓

Initialize Managers

↓

Initialize Systems

↓

Main Menu

↓

Gameplay

↓

Tick

↓

Autosave

↓

Exit
```

---

# Initialization Order

```text
Bootstrap

↓

Config

↓

Event Bus

↓

Managers

↓

Data

↓

Save

↓

Simulation

↓

UI

↓

Gameplay
```

---

# Shutdown Order

```text
Autosave

↓

Dispose Systems

↓

Dispose Managers

↓

Unload Assets

↓

Exit
```

---

# Runtime Data

```text
PlayerData

HoldingData

CompanyData

EmployeeData

ResearchData

StatisticsData

EconomyData

TimeData
```

---

# Static Data

```text
Configs

Cities

Countries

Company Types

Research Tree

Buildings

Achievements

Events

Audio

Themes
```

---

# Manager Hierarchy

```text
GameManager

├── TimeManager

├── SaveManager

├── EventManager

├── UIManager

├── AudioManager

├── EconomyManager

├── NotificationManager

├── LocalizationManager

└── SceneManager
```

---

# Domain Modules

```text
Company

Holding

Economy

Employees

Research

Cities

Countries

Market

Statistics

News

Achievements
```

---

# UI Modules

```text
Main Menu

HUD

Pause

Settings

Company

Holding

Employee

Research

Branch

City

Country

Statistics

News

Achievements

Popup

Notification
```

---

# Communication Rules

Presentation

↓

Application

↓

Domain

↓

Event Bus

↓

Presentation

---

Infrastructure yalnızca servis sağlar.

Domain UI bilmez.

Domain Manager bilmez.

UI Domain implementasyonu bilmez.

View yalnızca ViewModel ile iletişim kurar.

ViewModel yalnızca Use Case çağırır.

---

# Data Ownership

```text
Player

└── Holding

      ├── Companies

      │      ├── Employees

      │      ├── Branches

      │      ├── Research

      │      └── Statistics

      └── Global Statistics
```

---

# Tick Ownership

```text
Game

↓

Time

↓

Holding

↓

Company

↓

Employee

↓

Research

↓

Economy
```

---

# Event Ownership

```text
EventManager

↓

Global Events

↓

System Events

↓

UI Events
```

---

# Configuration

```text
ScriptableObjects

↓

Configs

↓

Runtime Initialization
```

---

# Asset Flow

```text
Addressables

↓

Load

↓

Cache

↓

Pool

↓

Release
```

---

# Memory Rules

Runtime Data yalnızca RAM'de tutulur.

Static Data ScriptableObject üzerinden okunur.

UI yeniden kullanılabilir.

Popup Pool kullanır.

Notification Pool kullanır.

Asset Addressables üzerinden yüklenir.

---

# Project Principles

- Single Responsibility
- Separation of Concerns
- Low Coupling
- High Cohesion
- Event Driven
- Data Driven
- Composition over Inheritance
- Dependency Injection
- Modular Design
- Scalable Architecture
- Testable Systems
- Mobile First
- Performance First
- Reusable Components
- Feature Isolation
```