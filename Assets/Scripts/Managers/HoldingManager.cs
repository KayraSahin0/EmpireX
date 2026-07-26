using System;
using System.Linq;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Economy;
using EmpireX.Company;
using EmpireX.Branch;

namespace EmpireX.Holding
{
    /// <summary>
    /// Oyuncunun ana holding binasının (dashboard), global seviyesinin ve istatistiklerinin yönetim sistemi.
    /// </summary>
    public class HoldingManager : EmpireX.Core.BaseManager
    {
        private readonly EconomyManager _economyManager;
        private readonly CompanyManager _companyManager;
        private readonly BranchManager _branchManager;
        
        private HoldingData _holdingData;

        public HoldingManager(IEventBus eventBus, EconomyManager economyManager, CompanyManager companyManager, BranchManager branchManager) : base(eventBus)
        {
            _economyManager = economyManager;
            _companyManager = companyManager;
            _branchManager = branchManager;
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Subscribe<MonthStarted>(OnMonthStarted);
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Unsubscribe<MonthStarted>(OnMonthStarted);
        }

        private void OnLoadCompleted(LoadCompleted e)
        {
            _holdingData = e.Data.HoldingData;
            
            // Eğer yeni bir oyunsa ve Holding ID atanmamışsa
            if (string.IsNullOrEmpty(_holdingData.Id))
            {
                _holdingData.Id = Guid.NewGuid().ToString();
                _holdingData.Name = "My Empire Holding";
                _holdingData.Level = 1;
                _eventBus.Publish(new HoldingCreated { HoldingId = _holdingData.Id, Name = _holdingData.Name });
            }
        }

        public void SetHoldingName(string name)
        {
            if (_holdingData != null)
                _holdingData.Name = name;
        }

        public bool UpgradeHolding()
        {
            if (_holdingData == null) return false;

            double upgradeCost = _holdingData.Level * 1000000; // Holding geliştirme çok masraflıdır
            if (!_economyManager.TrySpend(upgradeCost, "Holding Upgrade"))
            {
                _eventBus.Publish(new HoldingActionFailed { Reason = "Holding binasını geliştirmek için bütçe yetersiz (Gereken: 1 Milyon x Seviye)." });
                return false;
            }

            _holdingData.Level++;
            
            // İleride şirketlere kalıcı Holding Gücü (Bonus) verilebilir.
            
            _eventBus.Publish(new HoldingUpgraded { NewLevel = _holdingData.Level });
            return true;
        }

        private void OnMonthStarted(MonthStarted e)
        {
            UpdateStatistics();
        }

        /// <summary>
        /// Tüm şirketleri ve şubeleri tarayarak Holding Dashboard'da gösterilecek güncel verileri (İstihdam, Toplam Ciro) hesaplar.
        /// </summary>
        public void UpdateStatistics()
        {
            if (_holdingData == null) return;

            _holdingData.TotalRevenue = 0;
            _holdingData.TotalExpense = 0;
            _holdingData.TotalEmployees = 0;
            
            foreach (var compId in _holdingData.CompanyIds)
            {
                var comp = _companyManager.GetCompany(compId);
                if (comp != null)
                {
                    _holdingData.TotalRevenue += comp.Revenue;
                    _holdingData.TotalExpense += comp.Expense;
                    _holdingData.TotalEmployees += comp.EmployeeIds.Count;
                    
                    var branches = _branchManager.GetBranchesByCompany(compId);
                    // Şubelerin çalışanları
                    _holdingData.TotalEmployees += branches.Sum(b => b.Employees);
                }
            }

            _holdingData.TotalProfit = _holdingData.TotalRevenue - _holdingData.TotalExpense;
            
            // Holding Değerlemesi: Kasa + Tahmini Yıllık Ciro
            _holdingData.Value = _holdingData.Cash + (_holdingData.TotalRevenue * 12);

            _eventBus.Publish(new HoldingStatsUpdated { Data = _holdingData });
        }
    }
}
