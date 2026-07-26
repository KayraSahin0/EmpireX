using System;
using System.Linq;
using System.Collections.Generic;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Economy;
using EmpireX.Company;
using EmpireX.City;

namespace EmpireX.Branch
{
    public class BranchManager : EmpireX.Core.BaseManager
    {
        private readonly EconomyManager _economyManager;
        private readonly CompanyManager _companyManager;
        private readonly CityManager _cityManager;
        private List<BranchData> _branches;

        public BranchManager(IEventBus eventBus, EconomyManager economyManager, CompanyManager companyManager, CityManager cityManager) : base(eventBus)
        {
            _economyManager = economyManager;
            _companyManager = companyManager;
            _cityManager = cityManager;
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<LoadCompleted>(OnLoadCompleted);
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<LoadCompleted>(OnLoadCompleted);
        }

        private void OnLoadCompleted(LoadCompleted e)
        {
            _branches = e.Data.Branches;
        }

        public BranchData CreateBranch(string companyId, string cityId, string branchName = "Yeni Şube")
        {
            var company = _companyManager.GetCompany(companyId);
            if (company == null) return null;

            double branchCost = 15000;
            if (!_economyManager.TrySpend(branchCost, $"Branch Creation: {branchName}"))
            {
                _eventBus.Publish(new BranchActionFailed { Reason = "Şube kurulumu için yetersiz bakiye." });
                return null;
            }

            var branch = new BranchData
            {
                Id = Guid.NewGuid().ToString(),
                CompanyId = companyId,
                CityId = cityId,
                Level = 1,
                Revenue = 2000,
                Expense = 800,
                Employees = 5
            };

            _branches.Add(branch);
            company.BranchIds.Add(branch.Id);

            company.Brand += 1.5f;
            company.Revenue += branch.Revenue;
            company.Expense += branch.Expense;

            _cityManager.RegisterBusinessToCity(cityId);

            _eventBus.Publish(new BranchCreated { BranchId = branch.Id, CompanyId = companyId });
            return branch;
        }

        public bool UpgradeBranch(string branchId)
        {
            var branch = GetBranch(branchId);
            if (branch == null) return false;

            double upgradeCost = branch.Level * 20000;
            if (!_economyManager.TrySpend(upgradeCost, $"Branch Upgrade: {branch.Id}"))
            {
                _eventBus.Publish(new BranchActionFailed { Reason = "Şube geliştirme için bakiye yetersiz." });
                return false;
            }

            double oldRev = branch.Revenue;
            double oldExp = branch.Expense;

            branch.Level++;
            branch.Revenue *= 1.3;
            branch.Expense *= 1.15;
            branch.Employees += 2;

            var company = _companyManager.GetCompany(branch.CompanyId);
            if (company != null)
            {
                company.Revenue += (branch.Revenue - oldRev);
                company.Expense += (branch.Expense - oldExp);
            }

            _eventBus.Publish(new BranchUpgraded { BranchId = branchId, NewLevel = branch.Level });
            return true;
        }

        public BranchData GetBranch(string id) => _branches.FirstOrDefault(b => b.Id == id);
        public List<BranchData> GetBranchesByCompany(string companyId) => _branches.Where(b => b.CompanyId == companyId).ToList();
    }
}
