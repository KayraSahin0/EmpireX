using System;
using System.Linq;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Economy;

namespace EmpireX.Company
{
    /// <summary>
    /// Şirket simülasyonunu ve yönetimini gerçekleştiren sistem.
    /// </summary>
    public class CompanyManager : EmpireX.Core.BaseManager
    {
        private readonly EconomyManager _economyManager;
        private System.Collections.Generic.List<CompanyData> _companies;
        private HoldingData _holdingData;
        private Random _rnd = new Random();

        public CompanyManager(IEventBus eventBus, EconomyManager economyManager) : base(eventBus)
        {
            _economyManager = economyManager;
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Subscribe<GameStarted>(OnGameStarted);
            _eventBus.Subscribe<DayStarted>(OnDayStarted);
            _eventBus.Subscribe<MonthStarted>(OnMonthStarted);
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Unsubscribe<GameStarted>(OnGameStarted);
            _eventBus.Unsubscribe<DayStarted>(OnDayStarted);
            _eventBus.Unsubscribe<MonthStarted>(OnMonthStarted);
        }

        private void OnLoadCompleted(LoadCompleted e)
        {
            _companies = e.Data.Companies;
            _holdingData = e.Data.HoldingData;
        }

        private void OnGameStarted(GameStarted e)
        {
            // Initializing logic if needed when new game starts
        }

        public CompanyData CreateCompany(CompanyTypeSO type, string name, string cityId)
        {
            if (!_economyManager.TrySpend(type.BaseCost, $"Creation of {name}"))
            {
                _eventBus.Publish(new CompanyCreationFailed { Reason = "Yetersiz bakiye (Kasa)." });
                return null;
            }

            var company = new CompanyData
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                CompanyTypeId = type.Id,
                CityId = cityId,
                Level = 1,
                Value = type.BaseCost,
                Brand = 10,
                MarketShare = 1,
                Automation = 0,
                Innovation = 0,
                Risk = 0.5,
                Cash = 0,
                Revenue = type.BaseRevenue,
                Expense = type.BaseExpense
            };

            _companies.Add(company);
            _holdingData.CompanyIds.Add(company.Id);

            _eventBus.Publish(new CompanyCreated { CompanyId = company.Id });
            return company;
        }

        public bool UpgradeCompany(string companyId)
        {
            var company = GetCompany(companyId);
            if (company == null) return false;

            double upgradeCost = company.Value * 1.5; // Basit bir maliyet artış formülü

            if (_economyManager.TrySpend(upgradeCost, $"Upgrade {company.Name}"))
            {
                company.Level++;
                company.Revenue *= 1.25;
                company.Expense *= 1.15;
                company.Value += upgradeCost;
                
                _eventBus.Publish(new CompanyUpgraded { CompanyId = companyId, NewLevel = company.Level });
                return true;
            }

            return false;
        }

        public CompanyData GetCompany(string id) => _companies.FirstOrDefault(c => c.Id == id);

        private void OnDayStarted(DayStarted e)
        {
            // Günlük şirket simülasyonu
            foreach (var company in _companies)
            {
                // Marka değeri rastgele dalgalanır
                company.Brand += (_rnd.NextDouble() - 0.5) * 0.1; 
                
                // Günlük net kâr hesaplaması
                double dailyRevenue = company.Revenue / 30.0;
                double dailyExpense = company.Expense / 30.0;
                
                company.Cash += (dailyRevenue - dailyExpense);
                company.Profit = company.Revenue - company.Expense; // Aylık genel profit tahmini
            }
        }

        private void OnMonthStarted(MonthStarted e)
        {
            // Aylık bilanço işlemleri
            foreach (var company in _companies)
            {
                company.Value *= 1.01; // %1 şirket değeri büyümesi

                // Şirket kasasında biriken kâr Holding'e aktarılır
                if (company.Cash > 0)
                {
                    _economyManager.AddRevenue(company.Cash, $"Profit from {company.Name}");
                    company.Cash = 0;
                }
                else if (company.Cash < 0)
                {
                    // Zarar Holding'in ana kasasından kapatılır
                    _economyManager.AddExpense(Math.Abs(company.Cash), $"Loss coverage for {company.Name}");
                    company.Cash = 0;
                }
            }
        }
    }
}
