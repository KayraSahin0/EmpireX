using System;
using System.Linq;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Economy;
using EmpireX.Company;

namespace EmpireX.StockMarket
{
    public class StockManager : EmpireX.Core.BaseManager
    {
        private readonly EconomyManager _economyManager;
        private readonly CompanyManager _companyManager;
        private Random _rnd = new Random();

        public StockManager(IEventBus eventBus, EconomyManager economyManager, CompanyManager companyManager) : base(eventBus)
        {
            _economyManager = economyManager;
            _companyManager = companyManager;
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<DayStarted>(OnDayStarted);
            _eventBus.Subscribe<YearStarted>(OnYearStarted); // Yılda bir kez temettü dağıtımı
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<DayStarted>(OnDayStarted);
            _eventBus.Unsubscribe<YearStarted>(OnYearStarted);
        }

        /// <summary>
        /// Bir şirketi Halka Arz (IPO) eder.
        /// </summary>
        /// <param name="companyId">Şirket ID</param>
        /// <param name="publicPercentage">Halka arz oranı (Maks %49)</param>
        /// <param name="dividendYield">Yıllık verilecek kâr payı (Temettü) oranı (Örn. %5 için 0.05)</param>
        public bool GoPublic(string companyId, float publicPercentage, float dividendYield)
        {
            var comp = _companyManager.GetCompany(companyId);
            if (comp == null || comp.IsPublic || publicPercentage <= 0 || publicPercentage > 0.49f) return false;

            comp.IsPublic = true;
            comp.TotalShares = 1000000; // Standart 1 Milyon hisse (Sanal pay)
            comp.PublicShares = (long)(comp.TotalShares * publicPercentage);
            comp.SharePrice = comp.Value / comp.TotalShares;
            comp.DividendYield = dividendYield;

            // IPO'dan elde edilen gelir Holding'e sıcak para olarak girer
            double raisedCapital = comp.PublicShares * comp.SharePrice;
            _economyManager.AddRevenue(raisedCapital, $"IPO of {comp.Name}");

            _eventBus.Publish(new CompanyIPO { CompanyId = comp.Id, RaisedCapital = raisedCapital });
            return true;
        }

        private void OnDayStarted(DayStarted e)
        {
            var companies = _companyManager.GetAllCompanies();
            if (companies == null) return;

            foreach (var comp in companies)
            {
                if (!comp.IsPublic) continue;

                double oldPrice = comp.SharePrice;
                
                // Şirket değerine, riskine ve piyasa trendine göre hisse fiyatı günlük dalgalanır
                double changePercent = (comp.Profit > 0 ? 0.01 : -0.01) + (_rnd.NextDouble() - 0.5) * comp.Risk * 0.1;
                comp.SharePrice *= (1.0 + changePercent);
                comp.SharePrice = Math.Max(0.01, comp.SharePrice);

                // Piyasa değerini hisse fiyatı üzerinden güncelle
                comp.Value = comp.TotalShares * comp.SharePrice;

                _eventBus.Publish(new StockPriceChanged { CompanyId = comp.Id, OldPrice = oldPrice, NewPrice = comp.SharePrice });
            }
        }

        private void OnYearStarted(YearStarted e)
        {
            // Temettü (Dividend) Dağıtımı (Yılbaşında)
            var companies = _companyManager.GetAllCompanies();
            if (companies == null) return;

            foreach (var comp in companies)
            {
                if (comp.IsPublic && comp.DividendYield > 0 && comp.Cash > 0)
                {
                    double totalDividend = comp.Cash * comp.DividendYield;
                    
                    // Şirket kasasından temettü çıkar
                    comp.Cash -= totalDividend;

                    // Halka açık kısıma (dışarıya) giden temettü
                    double publicDividend = totalDividend * ((double)comp.PublicShares / comp.TotalShares);
                    
                    // Kalan kısım Holding'in (Yani oyuncunun/kurucunun) cebine nakit olarak girer
                    double holdingDividend = totalDividend - publicDividend;
                    _economyManager.AddRevenue(holdingDividend, $"Dividends from {comp.Name}");

                    _eventBus.Publish(new DividendsPaid { CompanyId = comp.Id, TotalAmount = totalDividend });
                }
            }
        }
    }
}
