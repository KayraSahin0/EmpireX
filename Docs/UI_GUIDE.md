# UI_GUIDE

## Design Philosophy

- Minimal
- Premium
- Professional
- Functional
- Fast
- Mobile First
- One Hand Friendly
- Dark Theme
- Consistent
- Information First

---

# Theme

Background

- #0F1115

Surface

- #181C23

Card

- #21262F

Primary

- Gold

Success

- Green

Warning

- Orange

Danger

- Red

Info

- Blue

Primary Text

- White

Secondary Text

- Gray

Disabled

- Dark Gray

---

# Typography

Primary Font

- Inter

Fallback

- Roboto

Weights

- Regular
- Medium
- SemiBold
- Bold

---

# Border Radius

Small

- 8

Medium

- 12

Large

- 16

Popup

- 20

Button

- 12

Card

- 16

---

# Spacing

Base Unit

- 8

Padding

- 8
- 12
- 16
- 24
- 32

Margin

- 8
- 16
- 24

Gap

- 8
- 12
- 16

---

# Shadows

Cards

- Soft

Popup

- Medium

Modal

- Strong

Floating

- Strong

---

# Icons

Style

- Rounded
- Outline
- Simple

Size

- 20
- 24
- 28
- 32

---

# Buttons

Primary

- Filled
- Gold Accent

Secondary

- Dark Filled

Success

- Green

Danger

- Red

Disabled

- Low Opacity

Loading

- Spinner

---

# Inputs

States

- Normal
- Focused
- Error
- Disabled

Placeholder

- Gray

Cursor

- Primary

---

# Cards

Content

- Header
- Body
- Footer

Animation

- Hover
- Press
- Fade

---

# Panels

Reusable

Scrollable

Animated

Responsive

---

# Popups

Types

- Confirmation
- Information
- Warning
- Error
- Reward
- Input

Rules

- Background Blur
- Close Button
- Escape Support
- Outside Click Optional

---

# Notifications

Position

Top Right

Types

- Success
- Error
- Warning
- Information

Animation

Slide

Fade

Auto Hide

Pool

---

# Tooltips

Delay

- Short

Animation

- Fade

Position

- Auto

---

# Navigation

Bottom Navigation

Top Navigation

Side Panel

Back Support

Breadcrumb Ready

---

# HUD

Visible

Minimal

Responsive

Realtime Update

---

# Tables

Sortable

Scrollable

Responsive

Alternating Rows

---

# Charts

Line

Bar

Pie

Area

Responsive

Animated

---

# Lists

Virtualized

Reusable

Pool Ready

---

# Windows

Independent

Reusable

Closable

Resizable Ready

---

# Scroll View

Vertical

Horizontal

Momentum

Virtualization Ready

---

# Responsive Rules

Support

- Phone
- Tablet

Portrait First

Landscape Ready

Safe Area Compatible

---

# Animation

Duration

- 0.15
- 0.2
- 0.3

Types

- Fade
- Scale
- Slide
- Pop
- Fill

Curve

- Ease Out

---

# Feedback

Button Press

Card Press

Popup Open

Popup Close

Notification

Success

Error

---

# Accessibility

Readable Contrast

Large Touch Area

Scalable Text Ready

Color Independent Status

---

# Loading

Loading Screen

Progress Bar

Skeleton Screen

Spinner

---

# Empty State

Icon

Title

Description

Action Button

---

# Error State

Icon

Title

Description

Retry Button

---

# Success State

Animation

Icon

Short Message

---

# UI Hierarchy

Overlay

↓

Popup

↓

Window

↓

Panel

↓

Card

↓

Widget

↓

Control

---

# Naming

Panels

- MainMenuPanel
- PausePanel
- CompanyPanel

Windows

- CompanyWindow
- ResearchWindow

Views

- CompanyView
- HoldingView

Widgets

- CompanyCard
- EmployeeCard

Buttons

- PrimaryButton
- SecondaryButton

---

# Canvas

- Canvas Scaler
- Scale With Screen Size
- Match Width Or Height
- Safe Area Support

---

# Performance

- Object Pooling
- Sprite Atlas
- Addressables
- TMP Only
- Minimize Layout Rebuild
- Avoid Nested Layout Groups
- Avoid Frequent SetActive
- Event Driven Refresh
- Virtualized Lists

---

# UI Rules

- Tek ekran tek sorumluluk.
- Her pencere bağımsız geliştirilecek.
- Yeniden kullanılabilir widget kullanılacak.
- Tüm metinler Localization uyumlu olacak.
- Renk anlam taşıyacak.
- Animasyonlar kısa olacak.
- Bilgi hiyerarşisi korunacak.
- Aynı bileşen aynı görünümü kullanacak.
- Scroll içerikleri sanallaştırılabilir olacak.
- UI doğrudan Domain katmanına erişmeyecek.
- UI yalnızca ViewModel veya Presenter ile iletişim kuracak.
- Tüm kullanıcı işlemleri görsel geri bildirim verecek.
- Her UI bileşeni mobil kullanım öncelikli tasarlanacak.
```