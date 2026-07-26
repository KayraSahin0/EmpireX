using System;
using System.Linq;
using System.Collections.Generic;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Economy;
using EmpireX.Company;

namespace EmpireX.Statistics
{
    public class StatisticsManager : EmpireX.Core.BaseManager
    {
        private StatisticsData _statsData;
        private EconomyData _economyData;
        private readonly CompanyManager _companyManager;

        public StatisticsManager(IEventBus eventBus, CompanyManager companyManager) : base(eventBus)
        {
            _companyManager = companyManager;
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Subscribe<MonthStarted>(OnMonthStarted);
            _eventBus.Subscribe<TransactionOccurred>(OnTransactionOccurred);
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Unsubscribe<MonthStarted>(OnMonthStarted);
            _eventBus.Unsubscribe<TransactionOccurred>(OnTransactionOccurred);
        }

        private void OnLoadCompleted(LoadCompleted e)
        {
            _statsData = e.Data.StatisticsData;
            _economyData = e.Data.EconomyData;
        }

        private void OnTransactionOccurred(TransactionOccurred e)
        {
            if (_statsData == null) return;

            if (e.IsRevenue)
            {
                _statsData.TotalRevenue += e.Amount;
            }
            else
            {
                _statsData.TotalExpense += e.Amount;
            }
            _statsData.TotalProfit = _statsData.TotalRevenue - _statsData.TotalExpense;
        }

        private void OnMonthStarted(MonthStarted e)
        {
            if (_statsData == null || _economyData == null) return;

            var companies = _companyManager.GetAllCompanies();
            _statsData.TotalCompanies = companies != null ? companies.Count : 0;
            _statsData.TotalBranches = companies != null ? companies.Sum(c => c.BranchIds != null ? c.BranchIds.Count : 0) : 0;
            _statsData.TotalEmployees = companies != null ? companies.Sum(c => c.EmployeeIds != null ? c.EmployeeIds.Count : 0) : 0;

            // Tarihsel verileri kaydet (Grafikler için) - Sadece son 60 ay (5 yıl) tutulacak
            RecordHistory(_statsData.NetWorthHistory, _economyData.NetWorth);
            RecordHistory(_statsData.MonthlyRevenueHistory, _economyData.Revenue);
            RecordHistory(_statsData.MonthlyProfitHistory, _economyData.Profit);
            RecordHistory(_statsData.CompanyCountHistory, _statsData.TotalCompanies);
        }

        private void RecordHistory(List<double> historyList, double newValue)
        {
            if (historyList == null) return;
            historyList.Add(newValue);
            if (historyList.Count > 60)
            {
                historyList.RemoveAt(0); // En eski ay verisini at
            }
        }
    }
}
