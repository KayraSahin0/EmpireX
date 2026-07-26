using System;
using System.Linq;
using System.Collections.Generic;
using EmpireX.Events;
using EmpireX.Data;

namespace EmpireX.Country
{
    public class CountryManager : EmpireX.Core.BaseManager
    {
        private List<CountryData> _countries;
        private EconomyData _economyData;

        public CountryManager(IEventBus eventBus) : base(eventBus)
        {
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
            _countries = e.Data.Countries;
            _economyData = e.Data.EconomyData;

            // Oyunda her zaman var olan küresel "Ana Ülke"yi oluştur
            GetOrCreateCountry("global_country");
        }

        public CountryData GetOrCreateCountry(string countryId)
        {
            if (_countries == null) return null;
            
            var country = _countries.FirstOrDefault(c => c.Id == countryId);
            if (country == null)
            {
                country = new CountryData
                {
                    Id = countryId,
                    Name = $"Ülke {countryId}",
                    Currency = "USD",
                    TaxRate = 0.2f, // %20 standart vergi
                    Inflation = 0.05f, // %5 standart enflasyon
                    InterestRate = 0.1f, // %10 standart faiz
                    Stability = 100f,
                    EconomyLevel = 50f
                };
                _countries.Add(country);
                _eventBus.Publish(new CountryDataCreated { CountryId = country.Id });
            }
            return country;
        }

        private void OnMonthStarted(MonthStarted e)
        {
            if (_countries == null) return;
            
            foreach (var country in _countries)
            {
                // Rastgele makroekonomik olaylar ve dalgalanmalar
                Random rnd = new Random();
                
                // İstikrar zamanla 100'e yaklaşır veya kriz durumunda düşer
                if (rnd.NextDouble() < 0.1) country.Stability -= (float)(rnd.NextDouble() * 10f);
                else if (country.Stability < 100f) country.Stability += 2f;

                country.Stability = UnityEngine.Mathf.Clamp(country.Stability, 0f, 100f);

                // İstikrar düşükse Ekonomi zayıflar, yüksekse güçlenir
                if (country.Stability < 50f) country.EconomyLevel -= 1f;
                else if (country.Stability > 80f) country.EconomyLevel += 1f;
                
                country.EconomyLevel = UnityEngine.Mathf.Clamp(country.EconomyLevel, 0f, 100f);

                // Enflasyon ve Vergi, Ekonomi ve İstikrara göre değişir
                if (country.EconomyLevel < 30f)
                {
                    country.Inflation += 0.005f; // Ekonomik kriz -> Enflasyon artar
                    country.TaxRate += 0.001f; // Devlet vergi artırır
                }
                else if (country.EconomyLevel > 70f)
                {
                    country.Inflation -= 0.002f; // Sağlam ekonomi -> Enflasyon düşer
                }

                country.Inflation = UnityEngine.Mathf.Clamp(country.Inflation, 0f, 0.5f);
                country.TaxRate = UnityEngine.Mathf.Clamp(country.TaxRate, 0.01f, 0.5f);
                country.InterestRate = country.Inflation + 0.05f; // Faiz genelde enflasyonun biraz üstündedir

                _eventBus.Publish(new CountryEconomyChanged { 
                    CountryId = country.Id, 
                    NewInflation = country.Inflation, 
                    NewTaxRate = country.TaxRate 
                });

                // Global ekonomi verisini güncelle
                if (country.Id == "global_country" && _economyData != null)
                {
                    _economyData.TaxRate = country.TaxRate;
                    _economyData.Inflation = country.Inflation;
                    _economyData.InterestRate = country.InterestRate;
                }
            }
        }
    }
}
