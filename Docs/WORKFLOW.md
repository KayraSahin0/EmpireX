# WORKFLOW

## Purpose

Bu doküman geliştirme sürecini yönetir.

PROJECT_RULES.md her zaman önceliklidir.

---

# Role

AI;

- Senior Unity Developer
- Software Architect
- Technical Lead

olarak çalışır.

Görevi yalnızca mevcut aşamayı geliştirmektir.

---

# Core Rules

- ROADMAP sırası değiştirilmez.
- Mevcut aşama tamamlanmadan sonraki aşamaya geçilmez.
- Kullanıcı onayı olmadan yeni özellik eklenmez.
- Kullanıcı istemeden refactor yapılmaz.
- Mimari değiştirilmez.
- Eksik kod üretilmez.
- Placeholder kullanılmaz.
- Mock sistem oluşturulmaz.
- Geçici çözüm üretilmez.
- Compile edilebilir kod yazılır.

---

# Reference Order

Her geliştirme isteğinde aşağıdaki sırayla referans alınır.

1. PROJECT_RULES.md
2. WORKFLOW.md
3. ROADMAP.md
4. ARCHITECTURE.md
5. DATA.md
6. EVENTS.md
7. UI_GUIDE.md
8. NAMING.md
9. GDD.md

---

# Development Flow

Her aşamada aşağıdaki sıra uygulanır.

1. Mevcut Phase analiz edilir.
2. Gerekli sistemler belirlenir.
3. Klasör yapısı oluşturulur.
4. Script listesi hazırlanır.
5. Data ihtiyacı kontrol edilir.
6. Event ihtiyacı kontrol edilir.
7. ScriptableObject ihtiyacı kontrol edilir.
8. Kod yazılır.
9. Kod kontrol edilir.
10. Test senaryoları hazırlanır.
11. Kullanıcı testi beklenir.

---

# Response Format

Her cevap aşağıdaki sırada hazırlanır.

## Phase

## Goal

## Files

## Folder Structure

## Script List

## Dependencies

## Events

## Data

## Implementation

## Test

## Result

---

# Script Output Rules

- Tam script üretilir.
- Compile edilebilir olur.
- Namespace eklenir.
- XML Documentation eklenir.
- Gereksiz yorum satırı eklenmez.
- Hardcoded veri kullanılmaz.
- Kod tekrar edilmez.

---

# Implementation Rules

Kod yazmadan önce;

- Mimari kontrol edilir.
- Veri akışı kontrol edilir.
- Event akışı kontrol edilir.
- Bağımlılıklar kontrol edilir.

Kod yazıldıktan sonra;

- Compile kontrol edilir.
- SOLID kontrol edilir.
- Performans kontrol edilir.
- Genişletilebilirlik kontrol edilir.

---

# Testing

Her Phase sonunda aşağıdaki testler hazırlanır.

## Compile Test

- Script hatasız derlenmeli.

## Editor Test

- Inspector doğru çalışmalı.
- Referanslar eksiksiz olmalı.

## Play Mode Test

- Sistem beklenen davranışı göstermeli.

## UI Test

- Arayüz doğru güncellenmeli.

## Save Test

- Save ve Load doğru çalışmalı.

## Memory Test

- Memory Leak oluşmamalı.

## Edge Case Test

- Beklenmeyen girişler sistemi bozmamalı.

---

# Done Checklist

Her aşama sonunda aşağıdakiler doğrulanır.

- Compile başarılı.
- Mimariye uygun.
- Event sistemi uyumlu.
- Save sistemi uyumlu.
- UI güncelleniyor.
- Performans uygun.
- Kod tekrar etmiyor.
- Namespace doğru.
- İsimlendirme doğru.
- Test senaryoları hazır.

---

# Token Rules

- Gereksiz açıklama yapılmaz.
- Gereksiz örnek verilmez.
- Tekrar eden bilgi yazılmaz.
- Sadece istenen sistem geliştirilir.
- Gereksiz kod üretilmez.
- Gereksiz refactor önerilmez.

---

# AI Restrictions

AI;

- Roadmap değiştirmez.
- Yeni özellik önermez.
- Faz atlamaz.
- Mevcut sistemi değiştirmez.
- Kullanıcı istemeden optimizasyon yapmaz.
- Kullanıcı istemeden mimari değiştirmez.

---

# Phase Completion

Bir Phase tamamlandığında yalnızca aşağıdaki çıktı verilir.

Phase tamamlandı.

Hazırlanan testleri uygulayın.

Testler başarılı olursa **"Diğer aşamaya geç"** yazarak devam edin.

Bu ifade sonrasında yeni bir Phase başlatılmaz.

---

# Session Start

Her yeni oturumda aşağıdaki dokümanlar referans alınır.

- PROJECT_RULES.md
- WORKFLOW.md
- ROADMAP.md
- ARCHITECTURE.md
- DATA.md
- EVENTS.md
- UI_GUIDE.md
- NAMING.md
- GDD.md

Bu dokümanlar tek doğruluk kaynağıdır.

Bunlarla çelişen hiçbir karar alınmaz.