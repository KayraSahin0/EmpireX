using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Economy;
using EmpireX.Company;

namespace EmpireX.Executive
{
    public class ExecutiveManager : EmpireX.Core.BaseManager
    {
        private readonly EconomyManager _economyManager;
        private readonly CompanyManager _companyManager;
        private EmpireX.Employee.HRManager _hrManager;
        
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

            if (EmpireX.Core.GameManager.Instance != null)
            {
                _hrManager = EmpireX.Core.GameManager.Instance.HRManager;
            }
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Unsubscribe<DayStarted>(OnDayStarted);
        }

        private void OnLoadCompleted(LoadCompleted e)
        {
            _executives = e.Data.Executives;
            if (EmpireX.Core.GameManager.Instance != null)
            {
                _hrManager = EmpireX.Core.GameManager.Instance.HRManager;
            }
        }

        public ExecutiveData HireExecutive(string companyId, string candidateId)
        {
            if (_hrManager == null) _hrManager = EmpireX.Core.GameManager.Instance.HRManager;
            
            var candidate = _hrManager.GetCandidate(candidateId);
            if (candidate == null || !candidate.IsExecutive) 
            {
                _eventBus.Publish(new ExecutiveActionFailed { Reason = "Yönetici adayı bulunamadı." });
                return null;
            }

            var company = _companyManager.GetCompany(companyId);
            if (company == null) return null;

            if (_executives.Any(ex => ex.CompanyId == companyId && ex.ExecutiveTypeId == candidate.TypeId))
            {
                _eventBus.Publish(new ExecutiveActionFailed { Reason = $"{candidate.TypeId} rolünde zaten bir yönetici var." });
                return null;
            }

            double hiringCost = candidate.ExpectedSalary * 2.0; // Yöneticilerde işe alım maliyeti maaşın 2 katı
            if (!_economyManager.TrySpend(hiringCost, $"Executive Hiring: {candidate.TypeId}"))
            {
                _eventBus.Publish(new ExecutiveActionFailed { Reason = "Yönetici işe alım maliyeti için bakiye yetersiz." });
                return null;
            }

            var exec = new ExecutiveData
            {
                Id = Guid.NewGuid().ToString(),
                CompanyId = companyId,
                ExecutiveTypeId = candidate.TypeId,
                Level = 1,
                Salary = candidate.ExpectedSalary,
                Bonus = 0,
                Age = candidate.Age,
                PortraitPath = candidate.PortraitPath
            };

            _executives.Add(exec);
            company.ExecutiveIds.Add(exec.Id);
            company.Expense += exec.Salary;

            // Adayı HR havuzundan çıkar
            _hrManager.RemoveCandidate(candidateId);

            _eventBus.Publish(new ExecutiveHired { ExecutiveId = exec.Id, Role = candidate.TypeId });
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
                        comp.Brand += 0.05f; 
                        break;
                    case "CFO":
                        double savings = (comp.Expense / 30.0) * 0.05 * exec.Level; 
                        comp.Cash += savings;
                        break;
                    case "CMO":
                        comp.MarketShare += 0.01f; 
                        break;
                    case "CTO":
                        comp.Automation += 0.1f; 
                        break;
                    case "COO":
                        comp.Cash += 500 * exec.Level; 
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
