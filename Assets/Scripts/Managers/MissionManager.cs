using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Economy;

namespace EmpireX.Missions
{
    public class MissionManager : EmpireX.Core.BaseManager
    {
        private SaveData _saveData;
        private readonly EconomyManager _economyManager;
        private List<MissionSO> _missionDatabase = new List<MissionSO>();

        // Günlük yenilenme süresi (ticks)
        private readonly long TicksPerDay = TimeSpan.TicksPerDay;

        public MissionManager(IEventBus eventBus, EconomyManager economyManager) : base(eventBus)
        {
            _economyManager = economyManager;
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Subscribe<DayStarted>(OnGameDayStarted);
            _eventBus.Subscribe<TransactionOccurred>(OnTransactionOccurred);
            _eventBus.Subscribe<CompanyCreated>(OnCompanyCreated);
            _eventBus.Subscribe<EmployeeHired>(OnEmployeeHired);
            
            LoadMissionsFromResources();
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Unsubscribe<DayStarted>(OnGameDayStarted);
            _eventBus.Unsubscribe<TransactionOccurred>(OnTransactionOccurred);
            _eventBus.Unsubscribe<CompanyCreated>(OnCompanyCreated);
            _eventBus.Unsubscribe<EmployeeHired>(OnEmployeeHired);
        }

        private void LoadMissionsFromResources()
        {
            var loaded = Resources.LoadAll<MissionSO>("Mission");
            if (loaded != null && loaded.Length > 0)
            {
                _missionDatabase = loaded.ToList();
            }
            else
            {
                CreateDummyMissions();
            }
        }

        private void CreateDummyMissions()
        {
            var m1 = ScriptableObject.CreateInstance<MissionSO>();
            m1.Id = "mission_earn_10k";
            m1.Title = "Günlük Kazanç";
            m1.Description = "Bugün 10.000$ gelir elde et.";
            m1.Type = MissionType.EarnRevenue;
            m1.TargetValue = 10000;
            m1.CashReward = 2500;
            _missionDatabase.Add(m1);

            var m2 = ScriptableObject.CreateInstance<MissionSO>();
            m2.Id = "mission_hire_1";
            m2.Title = "İstihdam Yarat";
            m2.Description = "1 yeni çalışan işe al.";
            m2.Type = MissionType.HireEmployee;
            m2.TargetValue = 1;
            m2.CashReward = 1000;
            _missionDatabase.Add(m2);

            var m3 = ScriptableObject.CreateInstance<MissionSO>();
            m3.Id = "mission_new_company";
            m3.Title = "Büyüme Adımları";
            m3.Description = "Yeni 1 şirket kur.";
            m3.Type = MissionType.CreateCompany;
            m3.TargetValue = 1;
            m3.CashReward = 50000;
            _missionDatabase.Add(m3);
        }

        private void OnLoadCompleted(LoadCompleted e)
        {
            _saveData = e.Data;
            CheckForDailyRefresh();
        }

        private void OnGameDayStarted(DayStarted e)
        {
            CheckForDailyRefresh();
        }

        private void CheckForDailyRefresh()
        {
            if (_saveData == null) return;

            long currentTicks = DateTime.UtcNow.Date.Ticks;
            if (_saveData.LastMissionRefreshTime < currentTicks || _saveData.ActiveMissions.Count == 0)
            {
                RefreshDailyMissions(currentTicks);
            }
        }

        private void RefreshDailyMissions(long currentTicks)
        {
            _saveData.ActiveMissions.Clear();
            _saveData.LastMissionRefreshTime = currentTicks;

            // Database içerisinden rastgele 3 görev seç
            var rnd = new System.Random();
            var shuffled = _missionDatabase.OrderBy(x => rnd.Next()).Take(3).ToList();

            foreach (var so in shuffled)
            {
                _saveData.ActiveMissions.Add(new MissionData
                {
                    MissionId = so.Id,
                    CurrentProgress = 0,
                    IsCompleted = false,
                    IsRewardClaimed = false
                });
            }

            _eventBus.Publish(new MissionsRefreshed());
            Debug.Log("[MissionManager] Günlük görevler yenilendi!");
        }

        public void ClaimReward(string missionId)
        {
            if (_saveData == null) return;
            var md = _saveData.ActiveMissions.FirstOrDefault(m => m.MissionId == missionId);
            if (md != null && md.IsCompleted && !md.IsRewardClaimed)
            {
                var so = _missionDatabase.FirstOrDefault(m => m.Id == missionId);
                if (so != null)
                {
                    md.IsRewardClaimed = true;
                    _economyManager.AddRevenue(so.CashReward, $"Mission Reward: {so.Title}");
                    _eventBus.Publish(new MissionRewardClaimed { MissionId = missionId, RewardAmount = so.CashReward });
                    Debug.Log($"[MissionManager] Ödül alındı: {so.Title} (+${so.CashReward:N0})");
                }
            }
        }

        private void AddProgress(MissionType type, double amount)
        {
            if (_saveData == null) return;

            bool progressed = false;
            foreach (var md in _saveData.ActiveMissions)
            {
                if (md.IsCompleted) continue;

                var so = _missionDatabase.FirstOrDefault(m => m.Id == md.MissionId);
                if (so != null && so.Type == type)
                {
                    md.CurrentProgress += amount;
                    progressed = true;

                    if (md.CurrentProgress >= so.TargetValue)
                    {
                        md.CurrentProgress = so.TargetValue;
                        md.IsCompleted = true;
                        _eventBus.Publish(new MissionCompleted { MissionId = md.MissionId });
                        Debug.Log($"[MissionManager] Görev Tamamlandı: {so.Title}");
                    }
                    else
                    {
                        _eventBus.Publish(new MissionProgressed { MissionId = md.MissionId, CurrentProgress = md.CurrentProgress, Target = so.TargetValue });
                    }
                }
            }
        }

        private void OnTransactionOccurred(TransactionOccurred e)
        {
            if (e.IsRevenue) AddProgress(MissionType.EarnRevenue, e.Amount);
        }

        private void OnCompanyCreated(CompanyCreated e)
        {
            AddProgress(MissionType.CreateCompany, 1);
        }

        private void OnEmployeeHired(EmployeeHired e)
        {
            AddProgress(MissionType.HireEmployee, 1);
        }
    }
}
