using System;
using System.Linq;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Economy;
using EmpireX.City;

namespace EmpireX.Company
{
    public class CompanyManager : EmpireX.Core.BaseManager
    {
        private readonly EconomyManager _economyManager;
        private readonly CityManager _cityManager;
        private System.Collections.Generic.List<CompanyData> _companies;
        private HoldingData _holdingData;
        private Random _rnd = new Random();

        public CompanyManager(IEventBus eventBus, EconomyManager economyManager, CityManager cityManager) : base(eventBus)
        {
            _economyManager = economyManager;
            _cityManager = cityManager;
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Subscribe<DayStarted>(OnDayStarted);
            _eventBus.Subscribe<MonthStarted>(OnMonthStarted);
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Unsubscribe<DayStarted>(OnDayStarted);
            _eventBus.Unsubscribe<MonthStarted>(OnMonthStarted);
        }

        private void OnLoadCompleted(LoadCompleted e)
        {
            _companies = e.Data.Companies;
            _holdingData = e.Data.HoldingData;
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

            _cityManager.RegisterBusinessToCity(cityId); // Şehir dinamiklerini güncelle

            _eventBus.Publish(new CompanyCreated { CompanyId = company.Id });
            return company;
        }

        public CompanyData CreateCompetitorCompany(CompanyTypeSO type, string name, string cityId)
        {
            var company = new CompanyData
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                CompanyTypeId = type.Id,
                CityId = cityId,
                Level = 1,
                Value = type.BaseCost * 0.8,
                Brand = 5,
                MarketShare = 0.5,
                Automation = 0,
                Innovation = 0,
                Risk = 0.5,
                Cash = 5000,
                Revenue = type.BaseRevenue,
                Expense = type.BaseExpense
            };

            _companies.Add(company);
            // HOLDING'e EKLENMİYOR! Çünkü bu rakip şirket.
            
            _cityManager.RegisterBusinessToCity(cityId);
            return company;
        }

        public void DeleteCompany(string companyId)
        {
            var company = GetCompany(companyId);
            if (company != null)
            {
                _companies.Remove(company);
                if (_holdingData.CompanyIds.Contains(companyId))
                {
                    _holdingData.CompanyIds.Remove(companyId);
                }
            }
        }

        public bool UpgradeCompany(string companyId)
        {
            var company = GetCompany(companyId);
            if (company == null) return false;

            double upgradeCost = company.Value * 1.5; 

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
        public System.Collections.Generic.List<CompanyData> GetAllCompanies() => _companies;

        private void OnDayStarted(DayStarted e)
        {
            foreach (var company in _companies)
            {
                company.Brand += (_rnd.NextDouble() - 0.5) * 0.1; 
                
                var city = _cityManager.GetCity(company.CityId);
                double cityRent = city != null ? city.Rent : 0;
                float demand = city != null ? city.Demand : 1f;
                float comp = city != null ? city.Competition : 0f;

                // Talep yüksekse ciro artar, rekabet yüksekse ciro düşer
                double dailyRevenue = (company.Revenue / 30.0) * demand * (1f - (comp * 0.1f)) * company.MarketShare;
                
                // Şehir kirası günlük gidere eklenir
                double dailyExpense = (company.Expense / 30.0) + (cityRent / 30.0);
                
                company.Cash += (dailyRevenue - dailyExpense);
                company.Profit = dailyRevenue - dailyExpense; // Son günün net kârı
            }
        }

        private void OnMonthStarted(MonthStarted e)
        {
            foreach (var company in _companies)
            {
                company.Value *= 1.01; 

                if (company.Cash > 0)
                {
                    _economyManager.AddRevenue(company.Cash, $"Profit from {company.Name}");
                    company.Cash = 0;
                }
                else if (company.Cash < 0)
                {
                    _economyManager.AddExpense(Math.Abs(company.Cash), $"Loss coverage for {company.Name}");
                    company.Cash = 0;
                }
            }
        }
    }
}

