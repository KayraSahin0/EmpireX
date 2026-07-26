using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Core;
using UnityEngine;

namespace EmpireX.Economy
{
    /// <summary>
    /// Ekonomi simülasyonunu yöneten ana sistem.
    /// </summary>
    public class EconomyManager : BaseManager
    {
        private readonly ConfigSystem _configSystem;
        private EconomyData _economyData;
        private EconomyConfig _economyConfig;
        private HoldingData _holdingData;

        public EconomyManager(IEventBus eventBus, ConfigSystem configSystem) : base(eventBus)
        {
            _configSystem = configSystem;
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Subscribe<GameStarted>(OnGameStarted);
            _eventBus.Subscribe<DayStarted>(OnDayStarted);
            _eventBus.Subscribe<MonthStarted>(OnMonthStarted);
            _eventBus.Subscribe<YearStarted>(OnYearStarted);

            _economyConfig = _configSystem.GetConfig<EconomyConfig>();
            if (_economyConfig == null)
            {
                _economyConfig = ScriptableObject.CreateInstance<EconomyConfig>();
                _economyConfig.StartingCash = 100000;
                _economyConfig.DefaultTax = 0.2f;
                _economyConfig.DefaultInflation = 0.05f;
                _economyConfig.DefaultInterest = 0.1f;
            }
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Unsubscribe<GameStarted>(OnGameStarted);
            _eventBus.Unsubscribe<DayStarted>(OnDayStarted);
            _eventBus.Unsubscribe<MonthStarted>(OnMonthStarted);
            _eventBus.Unsubscribe<YearStarted>(OnYearStarted);
        }

        private void OnLoadCompleted(LoadCompleted e)
        {
            _economyData = e.Data.EconomyData;
            _holdingData = e.Data.HoldingData;

            // Eğer oyun yepyeni bir Save ise (Tick 0) config'teki başlangıç parasını ver
            if (e.Data.TimeData != null && e.Data.TimeData.Tick == 0 && _holdingData.Cash == 0)
            {
                if (_economyConfig != null)
                {
                    _holdingData.Cash = _economyConfig.StartingCash;
                }
            }
        }

        private void OnGameStarted(GameStarted e)
        {
            // Veriler LoadCompleted'ta alındığı için burada sadece eksik veri kontrolü yapılabilir
            if (_economyData.TaxRate == 0) _economyData.TaxRate = _economyConfig.DefaultTax;
            if (_economyData.Inflation == 0) _economyData.Inflation = _economyConfig.DefaultInflation;
            if (_economyData.InterestRate == 0) _economyData.InterestRate = _economyConfig.DefaultInterest;
            if (_economyData.ExchangeRate == 0) _economyData.ExchangeRate = 1.0f;
        }

        private void OnDayStarted(DayStarted e)
        {
            SimulateMarketFluctuations();
        }

        private void OnMonthStarted(MonthStarted e)
        {
            ProcessLoans();
            CalculateNetWorth();
            
            _eventBus.Publish(new EconomyUpdated { EconomyData = _economyData });
        }

        private void OnYearStarted(YearStarted e)
        {
            ProcessTaxes();
        }

        public void AddRevenue(double amount, string reason = "General")
        {
            _economyData.Revenue += amount;
            _holdingData.Cash += amount;
            UpdateCashFlow();
            
            _eventBus.Publish(new TransactionOccurred { Amount = amount, Reason = reason, IsRevenue = true });
        }

        public void AddExpense(double amount, string reason = "General")
        {
            _economyData.Expense += amount;
            _holdingData.Cash -= amount;
            UpdateCashFlow();

            _eventBus.Publish(new TransactionOccurred { Amount = amount, Reason = reason, IsRevenue = false });
        }
        
        public bool TrySpend(double amount, string reason = "General")
        {
            if (_holdingData.Cash >= amount)
            {
                AddExpense(amount, reason);
                return true;
            }
            return false;
        }

        private void UpdateCashFlow()
        {
            _economyData.Profit = _economyData.Revenue - _economyData.Expense;
            _economyData.CashFlow = _economyData.Profit;
        }

        private void ProcessLoans()
        {
            if (_economyData.Loan > 0)
            {
                double interestAmount = _economyData.Loan * _economyData.InterestRate / 12f;
                AddExpense(interestAmount, "Loan Interest");
            }
        }

        private void ProcessTaxes()
        {
            if (_economyData.Profit > 0)
            {
                double taxAmount = _economyData.Profit * _economyData.TaxRate;
                AddExpense(taxAmount, "Corporate Tax");
            }
        }

        private void SimulateMarketFluctuations()
        {
            _economyData.Inflation += Random.Range(-0.001f, 0.001f);
            _economyData.Inflation = Mathf.Clamp(_economyData.Inflation, 0.01f, 0.5f);

            _economyData.ExchangeRate += Random.Range(-0.05f, 0.05f);
            _economyData.ExchangeRate = Mathf.Max(0.1f, _economyData.ExchangeRate);
        }

        private void CalculateNetWorth()
        {
            _economyData.NetWorth = _holdingData.Cash + _holdingData.Value - _economyData.Loan;
        }
    }
}
