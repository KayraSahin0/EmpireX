using System;
using System.Linq;
using System.Collections.Generic;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Economy;
using EmpireX.Company;
using EmpireX.Office;

namespace EmpireX.Employee
{
    public class EmployeeManager : EmpireX.Core.BaseManager
    {
        private readonly EconomyManager _economyManager;
        private readonly CompanyManager _companyManager;
        private readonly OfficeManager _officeManager;
        private List<EmployeeData> _employees;
        private Random _rnd = new Random();

        public EmployeeManager(IEventBus eventBus, EconomyManager economyManager, CompanyManager companyManager, OfficeManager officeManager) : base(eventBus)
        {
            _economyManager = economyManager;
            _companyManager = companyManager;
            _officeManager = officeManager;
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
            _employees = e.Data.Employees;
        }

        public EmployeeData HireEmployee(string companyId, string employeeTypeId, string name = "")
        {
            var company = _companyManager.GetCompany(companyId);
            if (company == null) return null;

            var office = _officeManager.GetOfficeByOwner(companyId);
            int currentEmployees = _employees.Count(e => e.CompanyId == companyId);
            int maxEmployees = office != null ? office.MaxEmployees : 5; // Ofis yoksa max 5 çalışan kapasitesi

            if (currentEmployees >= maxEmployees)
            {
                _eventBus.Publish(new EmployeeActionFailed { Reason = "Ofis kapasitesi dolu. Yeni ofis alın veya mevcut ofisi geliştirin." });
                return null;
            }

            double hiringCost = 1000;
            if (!_economyManager.TrySpend(hiringCost, "Hiring Fee"))
            {
                _eventBus.Publish(new EmployeeActionFailed { Reason = "İşe alım komisyonu için bakiye yetersiz." });
                return null;
            }

            if (string.IsNullOrEmpty(name)) name = $"Çalışan {_rnd.Next(1000, 9999)}";

            var emp = new EmployeeData
            {
                Id = Guid.NewGuid().ToString(),
                CompanyId = companyId,
                EmployeeTypeId = employeeTypeId,
                Name = name,
                Level = 1,
                Salary = 2000,
                Skill = (float)(_rnd.NextDouble() * 10 + 5),
                Experience = 0,
                Happiness = 80,
                Stress = 20,
                Loyalty = 50,
                Productivity = 100
            };

            _employees.Add(emp);
            company.EmployeeIds.Add(emp.Id);
            company.Expense += emp.Salary;

            _eventBus.Publish(new EmployeeHired { EmployeeId = emp.Id, CompanyId = companyId });
            return emp;
        }

        public bool PromoteEmployee(string employeeId)
        {
            var emp = GetEmployee(employeeId);
            if (emp == null) return false;

            if (emp.Experience < emp.Level * 100)
            {
                _eventBus.Publish(new EmployeeActionFailed { Reason = "Çalışanın tecrübesi terfi için yetersiz." });
                return false;
            }

            var company = _companyManager.GetCompany(emp.CompanyId);
            if (company != null)
            {
                double salaryIncrease = emp.Salary * 0.2;
                company.Expense += salaryIncrease;
                emp.Salary += salaryIncrease;
            }

            emp.Level++;
            emp.Happiness = Math.Min(100, emp.Happiness + 20);
            emp.Loyalty = Math.Min(100, emp.Loyalty + 15);
            
            _eventBus.Publish(new EmployeePromoted { EmployeeId = employeeId, NewLevel = emp.Level });
            return true;
        }

        public bool TrainEmployee(string employeeId)
        {
            var emp = GetEmployee(employeeId);
            if (emp == null) return false;

            double trainingCost = emp.Level * 500;
            if (!_economyManager.TrySpend(trainingCost, $"Training {emp.Name}"))
            {
                _eventBus.Publish(new EmployeeActionFailed { Reason = "Eğitim bütçesi yetersiz." });
                return false;
            }

            emp.Skill += 5f;
            emp.Happiness += 5f;
            
            _eventBus.Publish(new EmployeeTrained { EmployeeId = employeeId });
            return true;
        }

        public EmployeeData GetEmployee(string id) => _employees.FirstOrDefault(e => e.Id == id);

        private void OnDayStarted(DayStarted e)
        {
            foreach (var emp in _employees)
            {
                var office = _officeManager.GetOfficeByOwner(emp.CompanyId);
                float happinessBonus = office != null ? office.HappinessBonus : 0f;
                float prodBonus = office != null ? office.ProductivityBonus : 0f;

                emp.Stress += (float)(_rnd.NextDouble() * 2 - 0.5);
                emp.Stress = Math.Clamp(emp.Stress, 0, 100);

                if (emp.Stress > 80)
                {
                    emp.Happiness -= 1f;
                }
                
                // Ofis mutluluk bonusu günlük pasif bir destek sağlar
                emp.Happiness = Math.Clamp(emp.Happiness + (happinessBonus * 0.01f), 0, 100);
                
                emp.Experience += 0.5f; 
                
                emp.Productivity = (emp.Skill + emp.Experience/10f) * (emp.Happiness / 100f) * (1 - (emp.Stress/200f));
                // Ofis üretkenlik bonusu doğrudan eklenir
                emp.Productivity += prodBonus; 
            }
        }

        private void OnMonthStarted(MonthStarted e)
        {
            foreach (var emp in _employees)
            {
                if (emp.Happiness > 70) emp.Loyalty += 1f;
                else if (emp.Happiness < 30) emp.Loyalty -= 2f;
                
                emp.Loyalty = Math.Clamp(emp.Loyalty, 0, 100);
            }
        }
    }
}
