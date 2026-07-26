using System;
using System.Linq;
using System.Collections.Generic;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Economy;

namespace EmpireX.Research
{
    /// <summary>
    /// AR-GE ağacını (Tree), araştırma kuyruğunu (Queue), kilit açmalarını (Unlocks) ve süreç ilerlemesini (Progress) yöneten sistem.
    /// </summary>
    public class ResearchManager : EmpireX.Core.BaseManager
    {
        private readonly EconomyManager _economyManager;
        
        private List<ResearchData> _researches;

        public ResearchManager(IEventBus eventBus, EconomyManager economyManager) : base(eventBus)
        {
            _economyManager = economyManager;
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Subscribe<DayStarted>(OnDayStarted);
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Unsubscribe<DayStarted>(OnDayStarted);
        }

        private void OnLoadCompleted(LoadCompleted e)
        {
            _researches = e.Data.Researches;
        }

        public bool StartResearch(ResearchSO researchSO)
        {
            if (researchSO == null) return false;

            // Research Tree / Prerequisites (Kilit Sistemi)
            foreach (var req in researchSO.Prerequisites)
            {
                if (!IsUnlocked(req))
                {
                    _eventBus.Publish(new ResearchActionFailed { Reason = $"Ön koşul araştırılmamış: {req}" });
                    return false;
                }
            }

            var existing = _researches.FirstOrDefault(r => r.Id == researchSO.Id);
            if (existing != null)
            {
                if (existing.IsUnlocked)
                    _eventBus.Publish(new ResearchActionFailed { Reason = "Bu araştırma zaten tamamlanmış." });
                else
                    _eventBus.Publish(new ResearchActionFailed { Reason = "Bu araştırma halihazırda kuyrukta işleniyor." });
                
                return false;
            }

            if (!_economyManager.TrySpend(researchSO.Cost, $"Research: {researchSO.Name}"))
            {
                _eventBus.Publish(new ResearchActionFailed { Reason = "Araştırma bütçesi yetersiz." });
                return false;
            }

            var newResearch = new ResearchData
            {
                Id = researchSO.Id,
                Level = 1,
                IsUnlocked = false,
                RemainingTime = researchSO.Duration, // Gün (Day) bazında süre
                Progress = 0f
            };

            _researches.Add(newResearch);
            
            _eventBus.Publish(new ResearchStarted { ResearchId = newResearch.Id });
            return true;
        }

        private void OnDayStarted(DayStarted e)
        {
            // Research Queue: Sadece bitmemiş olan ILK araştırmayı alır ve sırayla (kuyruk mantığıyla) işler.
            var activeResearch = _researches.FirstOrDefault(r => !r.IsUnlocked);
            
            if (activeResearch != null)
            {
                activeResearch.RemainingTime -= 1f; // Her gün 1 birim ilerler (Gelecekte Research Point çarpanı eklenebilir)

                if (activeResearch.RemainingTime <= 0)
                {
                    activeResearch.RemainingTime = 0;
                    activeResearch.Progress = 100f;
                    activeResearch.IsUnlocked = true;

                    _eventBus.Publish(new ResearchCompleted { ResearchId = activeResearch.Id });
                }
            }
        }

        public bool IsUnlocked(string researchId)
        {
            if (_researches == null) return false;
            var res = _researches.FirstOrDefault(r => r.Id == researchId);
            return res != null && res.IsUnlocked;
        }
    }
}
