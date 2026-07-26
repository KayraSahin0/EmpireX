using System;
using System.Linq;
using System.Collections.Generic;
using EmpireX.Events;
using EmpireX.Data;

namespace EmpireX.City
{
    public class CityManager : EmpireX.Core.BaseManager
    {
        private List<CityData> _cities;

        public CityManager(IEventBus eventBus) : base(eventBus)
        {
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
            _cities = e.Data.Cities;
        }

        public CityData GetCity(string cityId)
        {
            if (_cities == null) return null;
            
            var city = _cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                city = new CityData
                {
                    Id = cityId,
                    Name = $"Şehir {cityId}",
                    Rent = 5000,
                    Workforce = 100f,
                    Demand = 1.0f,
                    Competition = 0f,
                    CityBonus = 0f
                };
                _cities.Add(city);
                _eventBus.Publish(new CityDataCreated { CityId = city.Id });
            }
            return city;
        }

        public void RegisterBusinessToCity(string cityId)
        {
            var city = GetCity(cityId);
            if (city != null)
            {
                city.Competition += 0.05f; // Rekabet artar
                city.Rent *= 1.05f; // Kira %5 artar
                city.Workforce = Math.Max(0, city.Workforce - 2f); // İşgücü azalır
            }
        }

        private void OnDayStarted(DayStarted e)
        {
            if (_cities == null) return;
            
            foreach (var city in _cities)
            {
                if (city.Demand < 1.0f) city.Demand += 0.001f;
                else if (city.Demand > 1.0f) city.Demand -= 0.001f;
                
                if (city.Workforce < 100f) city.Workforce += 0.1f;
                
                if (city.Competition > 0f) city.Competition = Math.Max(0, city.Competition - 0.0005f);
            }
        }
    }
}
