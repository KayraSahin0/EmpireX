using System;
using System.Linq;
using System.Collections.Generic;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Economy;
using EmpireX.Company;

namespace EmpireX.Executive
{
    /// <summary>
    /// Şirketin tepe yöneticilerini (CEO, CFO, CTO vb.) ve sağladıkları global pasif bonusları yöneten sistem.
    /// </summary>
    public class ExecutiveManager : EmpireX.Core.BaseManager
    {
        private readonly EconomyManager _economyManager;
        private readonly CompanyManager _companyManager;
        
        private List<ExecutiveData> _executives;

        public ExecutiveManager(IEventBus eventBus, EconomyManager economyManager, CompanyManager companyManager) : base(eventBus)
        {
            _economyManager = economyManager;
            _companyManager = companyManager;
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
            _executives = e.Data.Executives;
        }

        public ExecutiveData HireExecutive(string companyId, string executiveTypeId)
        {
            var company = _companyManager.GetCompany(companyId);
            if (company == null) return null;

            if (_executives.Any(ex => ex.CompanyId == companyId && ex.ExecutiveTypeId == executiveTypeId))
            {
                _eventBus.Publish(new ExecutiveActionFailed { Reason = $"{executiveTypeId} rolünde zaten bir yönetici var." });
                return null;
            }

            double hiringCost = 50000;
            if (!_economyManager.TrySpend(hiringCost, $"Executive Hiring: {executiveTypeId}"))
            {
                _eventBus.Publish(new ExecutiveActionFailed { Reason = "Yönetici işe alım maliyeti için bakiye yetersiz." });
                return null;
            }

            var exec = new ExecutiveData
            {
                Id = Guid.NewGuid().ToString(),
                CompanyId = companyId,
                ExecutiveTypeId = executiveTypeId, // "CEO", "CFO", "HR", "CMO", "CTO", "COO"
                Level = 1,
                Salary = 10000,
                Bonus = 0
            };

            _executives.Add(exec);
            company.ExecutiveIds.Add(exec.Id);
            company.Expense += exec.Salary; // Yönetici maaşı şirketin aylık giderine eklenir

            _eventBus.Publish(new ExecutiveHired { ExecutiveId = exec.Id, Role = executiveTypeId });
            return exec;
        }

        private void OnDayStarted(DayStarted e)
        {
            foreach (var exec in _executives)
            {
                var comp = _companyManager.GetCompany(exec.CompanyId);
                if (comp == null) continue;

                switch (exec.ExecutiveTypeId)
                {
                    case "CEO":
                        comp.Brand += 0.05f; // CEO prestiji artırır
                        break;
                    case "CFO":
                        double savings = (comp.Expense / 30.0) * 0.05 * exec.Level; // Masrafların %5'ini geri kazandırır
                        comp.Cash += savings;
                        break;
                    case "CMO":
                        comp.MarketShare += 0.01f; // Pazar payını artırır
                        break;
                    case "CTO":
                        comp.Automation += 0.1f; // Otomasyonu hızlandırır
                        break;
                    case "COO":
                        comp.Cash += 500 * exec.Level; // Ekstra operasyonel sabit kâr
                        break;
                }
            }
        }
        
        public bool HasExecutive(string companyId, string role)
        {
            if (_executives == null) return false;
            return _executives.Any(ex => ex.CompanyId == companyId && ex.ExecutiveTypeId == role);
        }
    }
}
