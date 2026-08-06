using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Economy;

namespace EmpireX.Achievements
{
    public class AchievementManager : EmpireX.Core.BaseManager
    {
        private HoldingData _holdingData;
        private StatisticsData _statsData;
        private readonly EconomyManager _economyManager;
        
        // Bu normalde bir veritabanından (Resource veya ConfigSystem) çekilmeli
        private List<AchievementSO> _achievementsDatabase = new List<AchievementSO>();

        public AchievementManager(IEventBus eventBus, EconomyManager economyManager) : base(eventBus)
        {
            _economyManager = economyManager;
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Subscribe<DayStarted>(OnDayStarted); // Her gün başarım kontrolü yapılır
            
            LoadAchievementsFromResources();
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Unsubscribe<DayStarted>(OnDayStarted);
        }

        private void LoadAchievementsFromResources()
        {
            var loaded = Resources.LoadAll<AchievementSO>("Achievement");
            if (loaded != null && loaded.Length > 0)
            {
                _achievementsDatabase = loaded.ToList();
            }
            else
            {
                // Test amaçlı sahte başarımlar (Veritabanı yoksa)
                CreateDummyAchievements();
            }
        }

        private void CreateDummyAchievements()
        {
            var ach1 = ScriptableObject.CreateInstance<AchievementSO>();
            ach1.Id = "ach_first_million";
            ach1.Title = "İlk Milyon";
            ach1.Description = "Toplam 1.000.000$ gelire ulaş.";
            ach1.Type = AchievementType.TotalRevenue;
            ach1.TargetValue = 1000000;
            ach1.CashReward = 50000;
            _achievementsDatabase.Add(ach1);

            var ach2 = ScriptableObject.CreateInstance<AchievementSO>();
            ach2.Id = "ach_monopoly";
            ach2.Title = "Holding İmparatorluğu";
            ach2.Description = "Holding bünyesinde 5 şirkete ulaş.";
            ach2.Type = AchievementType.TotalCompanies;
            ach2.TargetValue = 5;
            ach2.CashReward = 200000;
            _achievementsDatabase.Add(ach2);
        }

        private void OnLoadCompleted(LoadCompleted e)
        {
            _holdingData = e.Data.HoldingData;
            _statsData = e.Data.StatisticsData;
        }

        private void OnDayStarted(DayStarted e)
        {
            if (_holdingData == null || _statsData == null) return;
            CheckAchievements();
        }

        public void CheckAchievements()
        {
            foreach (var ach in _achievementsDatabase)
            {
                if (_holdingData.AchievementIds.Contains(ach.Id)) continue; // Zaten açılmış

                if (IsRequirementMet(ach))
                {
                    UnlockAchievement(ach);
                }
            }
        }

        private bool IsRequirementMet(AchievementSO ach)
        {
            switch (ach.Type)
            {
                case AchievementType.TotalRevenue: return _statsData.TotalRevenue >= ach.TargetValue;
                case AchievementType.TotalProfit: return _statsData.TotalProfit >= ach.TargetValue;
                case AchievementType.TotalCompanies: return _statsData.TotalCompanies >= ach.TargetValue;
                case AchievementType.TotalEmployees: return _statsData.TotalEmployees >= ach.TargetValue;
                case AchievementType.HoldingLevel: return _holdingData.Level >= ach.TargetValue;
                default: return false;
            }
        }

        private void UnlockAchievement(AchievementSO ach)
        {
            _holdingData.AchievementIds.Add(ach.Id);
            
            if (ach.CashReward > 0)
            {
                _economyManager.AddRevenue(ach.CashReward, $"Achievement Reward: {ach.Title}");
            }

            _eventBus.Publish(new AchievementUnlocked 
            { 
                AchievementId = ach.Id, 
                Title = ach.Title, 
                RewardCash = ach.CashReward 
            });
            
            Debug.Log($"[BAŞARIM AÇILDI] {ach.Title}: {ach.Description} -> Ödül: ${ach.CashReward:N0}");
        }
    }
}
