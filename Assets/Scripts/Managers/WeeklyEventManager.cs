using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EmpireX.Events;
using EmpireX.Data;

namespace EmpireX.Events.Weekly
{
    public struct WeeklyEventStarted { public string EventId; public string Title; }
    public struct WeeklyEventEnded { public string EventId; }
}

namespace EmpireX.Managers
{
    using EmpireX.Events.Weekly;

    public class WeeklyEventManager : EmpireX.Core.BaseManager
    {
        private SaveData _saveData;
        private List<WeeklyEventSO> _eventDatabase = new List<WeeklyEventSO>();
        private WeeklyEventSO _currentEventSO;

        public WeeklyEventManager(IEventBus eventBus) : base(eventBus)
        {
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Subscribe<EmpireX.Events.DayStarted>(OnGameDayStarted); // Game day changed, but we check real-time
            
            LoadEventsFromResources();
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Unsubscribe<EmpireX.Events.DayStarted>(OnGameDayStarted);
        }

        private void LoadEventsFromResources()
        {
            var loaded = Resources.LoadAll<WeeklyEventSO>("WeeklyEvent");
            if (loaded != null && loaded.Length > 0)
            {
                _eventDatabase = loaded.ToList();
            }
            else
            {
                CreateDummyEvents();
            }
        }

        private void CreateDummyEvents()
        {
            var e1 = ScriptableObject.CreateInstance<WeeklyEventSO>();
            e1.Id = "event_tech_boom";
            e1.Title = "Teknoloji Patlaması";
            e1.Description = "Yazılım ve Teknoloji rüzgarı esiyor. Tüm gelirler 7 gün boyunca %20 artar!";
            e1.ModifierType = WeeklyEventModifier.GlobalRevenueBoost;
            e1.ModifierValue = 1.20f;
            e1.DurationDays = 7;
            _eventDatabase.Add(e1);

            var e2 = ScriptableObject.CreateInstance<WeeklyEventSO>();
            e2.Id = "event_economic_crisis";
            e2.Title = "Ekonomik Daralma";
            e2.Description = "Tedarik zinciri problemleri nedeniyle tüm giderler 7 gün boyunca %15 artar.";
            e2.ModifierType = WeeklyEventModifier.GlobalExpenseReduction;
            e2.ModifierValue = 1.15f; // Gider artışı (Ters mantık)
            e2.DurationDays = 7;
            _eventDatabase.Add(e2);
        }

        private void OnLoadCompleted(LoadCompleted e)
        {
            _saveData = e.Data;
            CheckEventStatus();
        }

        private void OnGameDayStarted(EmpireX.Events.DayStarted e)
        {
            CheckEventStatus();
        }

        private void CheckEventStatus()
        {
            if (_saveData == null || _eventDatabase.Count == 0) return;

            long currentTicks = DateTime.UtcNow.Ticks;

            // Etkinlik bitmişse veya hiç yoksa
            if (string.IsNullOrEmpty(_saveData.ActiveWeeklyEventId) || currentTicks >= _saveData.WeeklyEventEndTime)
            {
                if (!string.IsNullOrEmpty(_saveData.ActiveWeeklyEventId))
                {
                    // Eski etkinlik bitti
                    _eventBus.Publish(new WeeklyEventEnded { EventId = _saveData.ActiveWeeklyEventId });
                    Debug.Log($"[WeeklyEventManager] Haftalık Etkinlik Sona Erdi: {_saveData.ActiveWeeklyEventId}");
                }

                StartNewRandomEvent(currentTicks);
            }
            else
            {
                // Mevcut etkinlik devam ediyor, referansı güncelle
                _currentEventSO = _eventDatabase.FirstOrDefault(x => x.Id == _saveData.ActiveWeeklyEventId);
            }
        }

        private void StartNewRandomEvent(long currentTicks)
        {
            var rnd = new System.Random();
            _currentEventSO = _eventDatabase[rnd.Next(_eventDatabase.Count)];
            
            _saveData.ActiveWeeklyEventId = _currentEventSO.Id;
            // Gerçek hayatta N gün sonrası
            _saveData.WeeklyEventEndTime = currentTicks + TimeSpan.FromDays(_currentEventSO.DurationDays).Ticks;

            _eventBus.Publish(new WeeklyEventStarted { EventId = _currentEventSO.Id, Title = _currentEventSO.Title });
            Debug.Log($"[WeeklyEventManager] Yeni Haftalık Etkinlik Başladı: {_currentEventSO.Title} ({_currentEventSO.DurationDays} Gün Sürecek)");
        }

        /// <summary>
        /// Belirli bir tipteki haftalık modifier çarpanını getirir.
        /// Eğer etkinlik aktifse ve tip eşleşiyorsa ModifierValue döner, aksi halde 1.0f (Etkisiz) döner.
        /// </summary>
        public float GetActiveModifier(WeeklyEventModifier type)
        {
            if (_currentEventSO != null && _currentEventSO.ModifierType == type)
            {
                // Etkinlik süresi bitmemişse
                if (DateTime.UtcNow.Ticks < _saveData.WeeklyEventEndTime)
                {
                    return _currentEventSO.ModifierValue;
                }
            }
            return 1.0f; // Etki yok
        }
    }
}
