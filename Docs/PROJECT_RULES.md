# PROJECT_RULES

## Purpose

Bu doküman proje boyunca değişmeyecek yazılım standartlarını tanımlar.

Tüm geliştirme süreci bu kurallara uygun ilerleyecektir.

---

# General

- Unity 6 LTS kullanılacak.
- URP kullanılacak.
- Mobil platform (Android / iOS) hedeflenecek.
- Kod tamamen modüler olacak.
- Kod yeniden kullanılabilir olacak.
- Kod test edilebilir olacak.
- Kod okunabilir olacak.
- Kod genişletilebilir olacak.
- Kod performans odaklı olacak.
- Kısa vadeli çözümler uygulanmayacak.

---

# Architecture

- Clean Architecture
- SOLID
- Event Driven Architecture
- MVVM
- Observer Pattern
- Factory Pattern
- Strategy Pattern
- State Pattern
- Command Pattern
- Repository Pattern
- Dependency Injection
- Composition over Inheritance
- Data Driven Design

---

# Folder Structure

Scripts

Core

Application

Domain

Infrastructure

Managers

Company

Holding

Economy

Employees

Research

Cities

Countries

Statistics

Events

Achievements

UI

Audio

Data

Utilities

Save

Editor

---

# Namespace

EmpireX.Core

EmpireX.Application

EmpireX.Domain

EmpireX.Infrastructure

EmpireX.Company

EmpireX.Holding

EmpireX.Economy

EmpireX.UI

EmpireX.Events

EmpireX.Audio

EmpireX.Save

EmpireX.Utilities

---

# Coding Standard

- PascalCase
- camelCase
- readonly
- const
- enum
- XML Documentation
- Anlamlı isimlendirme
- Tek sorumluluk prensibi
- Küçük sınıflar
- Kısa metodlar
- Extension Method kullanımı
- Generic yapıların tercih edilmesi
- Interface tabanlı geliştirme

---

# Data Rules

- Oyun verileri Data sınıflarında tutulacak.
- Config verileri ScriptableObject olacak.
- Runtime verileri ScriptableObject içerisinde tutulmayacak.
- Inspector verileri SerializeField ile yönetilecek.
- Hardcoded veri kullanılmayacak.

---

# ScriptableObject

ScriptableObject kullanılacak.

- Company Types
- Research
- Cities
- Countries
- Buildings
- Events
- Achievements
- Configs
- Audio
- UI Theme
- Bonuses

---

# Managers

Manager kullanılacak sistemler.

- GameManager
- TimeManager
- SaveManager
- UIManager
- AudioManager
- EventManager
- EconomyManager
- NotificationManager
- SceneManager
- LocalizationManager

---

# Event System

Sistemler yalnızca Event Bus üzerinden haberleşecek.

---

# Tick System

Simülasyon Tick sistemi ile çalışacak.

Tick Order

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

Holding

↓

Statistics

↓

Events

↓

Notifications

↓

Autosave

---

# UI

- Responsive
- Safe Area Compatible
- Dark Theme
- Glassmorphism
- Reusable Components
- Object Pooling
- UI Animation
- Micro Animation

---

# Save

- JSON
- Auto Save
- Manual Save
- Versioning
- Data Migration
- Encryption
- Cloud Ready

---

# Performance

- Addressables
- Object Pooling
- Lazy Loading
- Async Loading
- Sprite Atlas
- Event Based Updates
- Memory Cache

---

# Logging

Central Logger kullanılacak.

Categories

- Info
- Warning
- Error
- Critical

---

# Testing

Her sistem aşağıdaki testlerden geçmelidir.

- Compile Test
- Editor Test
- Play Mode Test
- Save Test
- Performance Test
- Memory Test
- Edge Case Test

---

# Expandability

Yeni içerikler mevcut sistemi değiştirmeden eklenebilmelidir.

- Company
- Sector
- City
- Country
- Research
- Employee
- Executive
- Event
- Achievement
- Economy Model

---

# AI Rules

Kod üretirken;

- PROJECT_RULES.md dışına çıkılmayacak.
- Mevcut mimari değiştirilmeyecek.
- Kod tekrar edilmeyecek.
- Placeholder kullanılmayacak.
- Mock sistem oluşturulmayacak.
- Compile edilebilir kod üretilecek.
- Mevcut sistemi bozacak değişiklik yapılmayacak.
- Gereksiz açıklama yapılmayacak.
- Sadece istenilen sistem geliştirilecek.
- Kullanıcı onayı olmadan sonraki aşamaya geçilmeyecek.

---

# Definition of Done

Bir sistem tamamlanmış sayılabilmesi için;

- Compile hatasız olmalı.
- Testleri geçmeli.
- Mevcut mimariye uygun olmalı.
- Event sistemine entegre olmalı.
- Save sistemi ile uyumlu olmalı.
- Performans açısından uygun olmalı.
- Genişletilebilir olmalı.
- Dokümantasyonu güncellenmiş olmalı.
```