using System;
using System.Linq;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Economy;
using EmpireX.Company;

namespace EmpireX.Acquisition
{
    public class AcquisitionManager : EmpireX.Core.BaseManager
    {
        private readonly EconomyManager _economyManager;
        private readonly CompanyManager _companyManager;
        private HoldingData _holdingData;

        public AcquisitionManager(IEventBus eventBus, EconomyManager economyManager, CompanyManager companyManager) : base(eventBus)
        {
            _economyManager = economyManager;
            _companyManager = companyManager;
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
            _holdingData = e.Data.HoldingData;
        }

        public bool BuyCompany(string companyId)
        {
            if (_holdingData.CompanyIds.Contains(companyId)) return false; // Zaten Holding'e ait

            var company = _companyManager.GetCompany(companyId);
            if (company == null) return false;

            double price = company.Value * 1.2; // Piyasa değerinin %20 üstüne (Premium) alınır
            if (_economyManager.TrySpend(price, $"Acquired {company.Name}"))
            {
                _holdingData.CompanyIds.Add(companyId);
                _eventBus.Publish(new CompanyBought { CompanyId = companyId, Price = price });
                return true;
            }
            return false;
        }

        public bool SellCompany(string companyId)
        {
            if (!_holdingData.CompanyIds.Contains(companyId)) return false; // Holding'e ait değil

            var company = _companyManager.GetCompany(companyId);
            if (company == null) return false;

            double price = company.Value * 0.9; // %10 değer kaybıyla (Discount) piyasaya satılır
            
            _holdingData.CompanyIds.Remove(companyId);
            _economyManager.AddRevenue(price, $"Sold {company.Name}");
            
            _eventBus.Publish(new CompanySold { CompanyId = companyId, Price = price });
            return true;
        }

        public bool MergeCompanies(string targetCompanyId, string absorbedCompanyId)
        {
            // Şirket birleşmesi için her iki şirketin de Holding'e ait olması gerekir
            if (!_holdingData.CompanyIds.Contains(targetCompanyId) || !_holdingData.CompanyIds.Contains(absorbedCompanyId))
                return false; 

            var target = _companyManager.GetCompany(targetCompanyId);
            var absorbed = _companyManager.GetCompany(absorbedCompanyId);
            if (target == null || absorbed == null) return false;

            // Satın alınan şirketin değerlerini (Maddi varlıkları ve kapasiteyi) hedefe aktar
            target.Cash += absorbed.Cash;
            target.Revenue += absorbed.Revenue * 0.8; // Birleşme esnasında %20 verimlilik kaybı (Merge penalty)
            target.Expense += absorbed.Expense * 0.9; // Masraflarda %10 tasarruf (Synergy)
            target.Value += absorbed.Value;
            target.Brand = Math.Max(target.Brand, absorbed.Brand); // Büyük olan markayı tut
            target.MarketShare += absorbed.MarketShare; // Pazar payları birleşir

            // Çalışanları ve Şubeleri devret
            if (absorbed.BranchIds != null) target.BranchIds.AddRange(absorbed.BranchIds);
            if (absorbed.EmployeeIds != null) target.EmployeeIds.AddRange(absorbed.EmployeeIds);
            if (absorbed.ExecutiveIds != null) target.ExecutiveIds.AddRange(absorbed.ExecutiveIds);

            // Absorbe edilen şirketi piyasadan tamamen sil
            _companyManager.DeleteCompany(absorbedCompanyId);

            _eventBus.Publish(new CompaniesMerged { MainCompanyId = targetCompanyId, AbsorbedCompanyId = absorbedCompanyId });
            return true;
        }
    }
}
