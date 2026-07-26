using System;
using System.Linq;
using System.Collections.Generic;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Company;

namespace EmpireX.Market
{
    public class MarketManager : EmpireX.Core.BaseManager
    {
        private readonly CompanyManager _companyManager;

        public MarketManager(IEventBus eventBus, CompanyManager companyManager) : base(eventBus)
        {
            _companyManager = companyManager;
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<DayStarted>(OnDayStarted);
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<DayStarted>(OnDayStarted);
        }

        private void OnDayStarted(DayStarted e)
        {
            var allCompanies = _companyManager.GetAllCompanies();
            if (allCompanies == null || !allCompanies.Any()) return;

            // Şirketleri sektörlerine (CompanyTypeId) göre grupla
            var sectors = allCompanies.GroupBy(c => c.CompanyTypeId);

            foreach (var sector in sectors)
            {
                // Sektördeki toplam Marka ve İnovasyon gücünü hesapla
                double totalPower = sector.Sum(c => c.Brand + c.Innovation + c.Automation);

                if (totalPower <= 0) continue;

                foreach (var comp in sector)
                {
                    // Şirketin gücü = Kendi markası + otomasyonu
                    double myPower = comp.Brand + comp.Innovation + comp.Automation;
                    
                    // Sektörel pazar payını dinamik olarak belirle (Market Share)
                    comp.MarketShare = (myPower / totalPower);
                    
                    // Marka değeri (Brand) her gün çok az erime eğilimi gösterir 
                    if (comp.Brand > 5.0)
                    {
                        comp.Brand -= 0.01;
                    }
                }
            }
        }
    }
}
