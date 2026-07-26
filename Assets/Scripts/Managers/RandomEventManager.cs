using System;
using System.Linq;
using System.Collections.Generic;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Economy;
using EmpireX.Company;
using EmpireX.City;
using EmpireX.Country;

namespace EmpireX.RandomEvents
{
    public class RandomEventManager : EmpireX.Core.BaseManager
    {
        private readonly EconomyManager _economyManager;
        private readonly CompanyManager _companyManager;
        private readonly CityManager _cityManager;
        private readonly CountryManager _countryManager;
        private Random _rnd = new Random();

        public RandomEventManager(
            IEventBus eventBus, 
            EconomyManager economyManager, 
            CompanyManager companyManager, 
            CityManager cityManager,
            CountryManager countryManager) : base(eventBus)
        {
            _economyManager = economyManager;
            _companyManager = companyManager;
            _cityManager = cityManager;
            _countryManager = countryManager;
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<MonthStarted>(OnMonthStarted);
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<MonthStarted>(OnMonthStarted);
        }

        private void OnMonthStarted(MonthStarted e)
        {
            // %25 ihtimalle her ay rastgele bir olay olur (Test için biraz yüksek tutuldu)
            if (_rnd.NextDouble() > 0.25) return;

            int eventType = _rnd.Next(0, 5);
            string title = "";
            string desc = "";

            switch (eventType)
            {
                case 0: // Company Event (Skandal)
                    var companies = _companyManager.GetAllCompanies();
                    if (companies != null && companies.Count > 0)
                    {
                        var target = companies[_rnd.Next(companies.Count)];
                        target.Brand = Math.Max(1.0, target.Brand - 20.0);
                        title = "Şirket Skandalı!";
                        desc = $"{target.Name} şirketinde büyük bir skandal patlak verdi. Marka değeri ağır darbe aldı.";
                    }
                    break;
                case 1: // Economy Event (Borsa Çöküşü)
                    var country = _countryManager.GetOrCreateCountry("global_country");
                    if (country != null)
                    {
                        country.Stability = UnityEngine.Mathf.Max(0, country.Stability - 30f);
                        title = "Kara Cuma!";
                        desc = "Borsalarda yaşanan ani panik ülkenin ekonomik istikrarını alt üst etti.";
                    }
                    break;
                case 2: // Disaster Event (Doğal Afet)
                    title = "Doğal Afet";
                    desc = "Büyük bir fırtına tedarik zincirini vurdu, lojistik maliyetleri geçici olarak arttı.";
                    break;
                case 3: // Government Event (Vergi İndirimi)
                    var govCountry = _countryManager.GetOrCreateCountry("global_country");
                    if (govCountry != null)
                    {
                        govCountry.TaxRate = UnityEngine.Mathf.Max(0.01f, govCountry.TaxRate - 0.05f);
                        title = "Hükümet Teşviği";
                        desc = "Hükümet şirketleri desteklemek adına kurumlar vergisinde indirime gitti.";
                    }
                    break;
                case 4: // Marketing Event (Viral Reklam)
                    var mCompanies = _companyManager.GetAllCompanies();
                    if (mCompanies != null && mCompanies.Count > 0)
                    {
                        var target = mCompanies[_rnd.Next(mCompanies.Count)];
                        target.Brand += 30.0;
                        title = "Viral Başarı!";
                        desc = $"{target.Name} şirketinin son reklam kampanyası viral oldu! Marka değeri uçuşa geçti.";
                    }
                    break;
            }

            if (!string.IsNullOrEmpty(title))
            {
                _eventBus.Publish(new RandomEventTriggered { EventName = title, Description = desc });
            }
        }
    }
}
