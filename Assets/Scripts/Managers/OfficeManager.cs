using System;
using System.Linq;
using System.Collections.Generic;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Economy;

namespace EmpireX.Office
{
    /// <summary>
    /// Holding veya Şirketlerin ofis yönetimini, geliştirmelerini ve bonuslarını sağlayan sistem.
    /// </summary>
    public class OfficeManager : EmpireX.Core.BaseManager
    {
        private readonly EconomyManager _economyManager;
        private List<OfficeData> _offices;

        public OfficeManager(IEventBus eventBus, EconomyManager economyManager) : base(eventBus)
        {
            _economyManager = economyManager;
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
            _offices = e.Data.Offices;
        }

        public OfficeData CreateOffice(string ownerId, string name)
        {
            if (GetOfficeByOwner(ownerId) != null)
            {
                _eventBus.Publish(new OfficeActionFailed { Reason = "Bu şirketin zaten bir ofisi var." });
                return null;
            }

            double baseCost = 25000;
            if (!_economyManager.TrySpend(baseCost, $"Office Creation: {name}"))
            {
                _eventBus.Publish(new OfficeActionFailed { Reason = "Yetersiz bakiye (Ofis Kurulumu)." });
                return null;
            }

            var office = new OfficeData
            {
                Id = Guid.NewGuid().ToString(),
                OwnerId = ownerId,
                Name = name,
                Level = 1,
                ProductivityBonus = 5f,
                HappinessBonus = 5f,
                MaxEmployees = 10,
                CustomizationValue = 0
            };

            _offices.Add(office);
            _eventBus.Publish(new OfficeCreated { OfficeId = office.Id });
            return office;
        }

        public bool UpgradeOffice(string officeId)
        {
            var office = GetOffice(officeId);
            if (office == null) return false;

            double upgradeCost = office.Level * 50000;
            if (!_economyManager.TrySpend(upgradeCost, $"Office Upgrade: {office.Name}"))
            {
                _eventBus.Publish(new OfficeActionFailed { Reason = "Yetersiz bakiye (Ofis Geliştirme)." });
                return false;
            }

            office.Level++;
            office.ProductivityBonus += 2.5f;
            office.HappinessBonus += 2.5f;
            office.MaxEmployees += 10;
            
            _eventBus.Publish(new OfficeUpgraded { OfficeId = officeId, NewLevel = office.Level });
            return true;
        }

        public bool CustomizeOffice(string officeId, double cost, float bonusIncrease)
        {
            var office = GetOffice(officeId);
            if (office == null) return false;

            if (!_economyManager.TrySpend(cost, $"Office Customization: {office.Name}"))
            {
                _eventBus.Publish(new OfficeActionFailed { Reason = "Yetersiz bakiye (Ofis Dekorasyonu)." });
                return false;
            }

            office.CustomizationValue += cost;
            office.HappinessBonus += bonusIncrease;
            
            _eventBus.Publish(new OfficeCustomized { OfficeId = officeId });
            return true;
        }

        public OfficeData GetOffice(string id) => _offices.FirstOrDefault(o => o.Id == id);
        public OfficeData GetOfficeByOwner(string ownerId) => _offices.FirstOrDefault(o => o.OwnerId == ownerId);
    }
}
