# NAMING

## Purpose

Bu doküman projedeki tüm isimlendirme standartlarını tanımlar.

Yeni eklenen tüm dosyalar bu kurallara uymalıdır.

---

# General

- PascalCase
- camelCase
- Anlamlı isimler
- Kısa isimlerden kaçınılır
- Kısaltma kullanılmaz
- Tekil isim tercih edilir
- Boolean isimleri fiil ile başlar

---

# Namespace

EmpireX.Core

EmpireX.Application

EmpireX.Domain

EmpireX.Infrastructure

EmpireX.Company

EmpireX.Holding

EmpireX.Economy

EmpireX.Employees

EmpireX.Research

EmpireX.UI

EmpireX.Events

EmpireX.Save

EmpireX.Audio

EmpireX.Utilities

---

# Class

Pattern

<Entity>

Examples

Company

Holding

Employee

Research

Economy

Market

Statistics

---

# Interface

Pattern

I<Entity>

Examples

IRepository

ISaveService

ITimeProvider

IEventBus

IAudioService

ICompanyFactory

---

# Abstract Class

Pattern

Base<Entity>

Examples

BasePanel

BaseWindow

BasePopup

BaseManager

BaseView

BaseViewModel

---

# Manager

Pattern

<Entity>Manager

Examples

GameManager

UIManager

TimeManager

SaveManager

AudioManager

EventManager

EconomyManager

SceneManager

LocalizationManager

NotificationManager

---

# Service

Pattern

<Entity>Service

Examples

SaveService

AudioService

LocalizationService

EconomyService

CompanyService

ResearchService

---

# Repository

Pattern

<Entity>Repository

Examples

CompanyRepository

SaveRepository

ResearchRepository

---

# Factory

Pattern

<Entity>Factory

Examples

CompanyFactory

PopupFactory

EmployeeFactory

NotificationFactory

---

# Builder

Pattern

<Entity>Builder

Examples

CompanyBuilder

HoldingBuilder

ResearchBuilder

---

# Strategy

Pattern

<Entity>Strategy

Examples

TaxStrategy

InterestStrategy

SalaryStrategy

GrowthStrategy

---

# Command

Pattern

<Action>Command

Examples

CreateCompanyCommand

HireEmployeeCommand

UpgradeCompanyCommand

SaveGameCommand

---

# Event

Pattern

<Entity><Action>

Examples

CompanyCreated

MoneyChanged

ResearchCompleted

EmployeeHired

HoldingCreated

---

# Event Handler

Pattern

<Entity>EventHandler

Examples

CompanyEventHandler

ResearchEventHandler

---

# View

Pattern

<Entity>View

Examples

CompanyView

HoldingView

ResearchView

EmployeeView

---

# ViewModel

Pattern

<Entity>ViewModel

Examples

CompanyViewModel

HoldingViewModel

MainMenuViewModel

---

# Presenter

Pattern

<Entity>Presenter

Examples

CompanyPresenter

HoldingPresenter

---

# Panel

Pattern

<Entity>Panel

Examples

MainMenuPanel

PausePanel

SettingsPanel

CompanyPanel

---

# Window

Pattern

<Entity>Window

Examples

CompanyWindow

ResearchWindow

EmployeeWindow

StatisticsWindow

---

# Popup

Pattern

<Entity>Popup

Examples

RewardPopup

ErrorPopup

ConfirmPopup

InputPopup

---

# Widget

Pattern

<Entity>Widget

Examples

CompanyCard

EmployeeCard

StatisticCard

NewsItem

NotificationItem

---

# ScriptableObject

Pattern

<Entity>SO

Examples

CompanyTypeSO

ResearchSO

CitySO

CountrySO

ConfigSO

AchievementSO

---

# Data

Pattern

<Entity>Data

Examples

PlayerData

HoldingData

CompanyData

EmployeeData

ResearchData

EconomyData

TimeData

---

# Config

Pattern

<Entity>Config

Examples

GameplayConfig

EconomyConfig

TimeConfig

AudioConfig

UIConfig

---

# Enum

Pattern

<Entity>Type

Examples

CompanyType

ResearchType

EmployeeType

PopupType

NotificationType

SceneType

---

# Attribute

Pattern

<Entity>Attribute

Examples

CompanyAttribute

EmployeeAttribute

---

# Collection

Pattern

Plural

Examples

companies

employees

branches

researches

notifications

---

# List

Pattern

<entity>List

Examples

companyList

employeeList

cityList

---

# Dictionary

Pattern

<key>To<value>

Examples

companyById

employeeById

cityById

researchById

---

# Queue

Pattern

<entity>Queue

Examples

researchQueue

notificationQueue

---

# Stack

Pattern

<entity>Stack

Examples

popupStack

windowStack

---

# Coroutine

Pattern

<Action>Routine

Examples

LoadRoutine

SaveRoutine

FadeRoutine

---

# Async

Pattern

<Action>Async

Examples

LoadAsync

SaveAsync

InitializeAsync

---

# Boolean

Pattern

is

has

can

should

Examples

isUnlocked

isVisible

hasSave

canUpgrade

shouldSave

---

# Events

Pattern

On<Action>

Examples

OnClick

OnOpened

OnClosed

OnLoaded

OnCompleted

---

# Constants

Pattern

UPPER_SNAKE_CASE

Examples

MAX_COMPANIES

MAX_LEVEL

SAVE_VERSION

DEFAULT_VOLUME

---

# Private Fields

Pattern

_camelCase

Examples

_companyData

_saveService

_eventBus

_currentCompany

---

# Serialized Fields

Pattern

_camelCase

Examples

_titleText

_confirmButton

_companyIcon

---

# Public Property

Pattern

PascalCase

Examples

Company

Holding

Revenue

Expense

Profit

---

# Method

Pattern

VerbNoun

Examples

Initialize

LoadGame

SaveGame

CreateCompany

HireEmployee

UpgradeCompany

CalculateRevenue

RefreshUI

OpenWindow

ClosePopup

---

# Extension

Pattern

<Entity>Extensions

Examples

StringExtensions

TransformExtensions

CompanyExtensions

---

# Utility

Pattern

<Entity>Utility

Examples

MathUtility

TimeUtility

SaveUtility

---

# File Name

Dosya adı sınıf adı ile aynı olmalıdır.

---

# Folder

PascalCase kullanılmalıdır.

Examples

Company

Holding

Research

Economy

UI

Managers

Utilities

---

# Scene

Pattern

<Entity>Scene

Examples

BootstrapScene

MainMenuScene

GameplayScene

LoadingScene

---

# Prefab

Pattern

<Entity>Prefab

Examples

CompanyCardPrefab

PopupPrefab

NotificationPrefab

---

# Sprite

Pattern

<Category>_<Name>

Examples

Icon_Company

Icon_Research

BG_MainMenu

UI_ButtonPrimary

---

# Animation

Pattern

<Action>

Examples

FadeIn

FadeOut

Open

Close

ScaleUp

ScaleDown

---

# Audio

Music

Music_<Name>

Examples

Music_Menu

Music_Gameplay

Music_Victory

SFX

SFX_<Action>

Examples

SFX_Click

SFX_Popup

SFX_Notification

SFX_Upgrade

---

# Rules

- Aynı kavram için her yerde aynı isim kullanılacak.
- Dosya adı ile sınıf adı aynı olacak.
- Bir sınıf yalnızca tek kavramı temsil edecek.
- İsimler görevini açıkça ifade edecek.
- Kısaltma kullanılmayacak.
- Sayı içeren isim kullanılmayacak.
- Geçici isimler kullanılmayacak.
- Yeni isimlendirmeler bu doküman güncellenmeden oluşturulmayacak.
```