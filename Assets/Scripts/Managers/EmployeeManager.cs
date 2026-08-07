using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.Economy;
using EmpireX.Company;
using EmpireX.Office;
using EmpireX.Executive;

namespace EmpireX.Employee
{
    public class EmployeeManager : EmpireX.Core.BaseManager
    {
        private readonly EconomyManager _economyManager;
        private readonly CompanyManager _companyManager;
        private readonly OfficeManager _officeManager;
        private readonly ExecutiveManager _executiveManager;
        private HRManager _hrManager; // GameManager'dan alınacak
        private List<EmployeeData> _employees;
        private System.Random _rnd = new System.Random();

        public EmployeeManager(IEventBus eventBus, EconomyManager economyManager, CompanyManager companyManager, OfficeManager officeManager, ExecutiveManager executiveManager) : base(eventBus)
        {
            _economyManager = economyManager;
            _companyManager = companyManager;
            _officeManager = officeManager;
            _executiveManager = executiveManager;
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Subscribe<DayStarted>(OnDayStarted);
            _eventBus.Subscribe<MonthStarted>(OnMonthStarted);
            
            // Lazy load HRManager
            if (EmpireX.Core.GameManager.Instance != null)
            {
                _hrManager = EmpireX.Core.GameManager.Instance.HRManager;
            }
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
            if (EmpireX.Core.GameManager.Instance != null)
            {
                _hrManager = EmpireX.Core.GameManager.Instance.HRManager;
            }
        }

        public EmployeeData HireEmployee(string companyId, string candidateId)
        {
            if (_hrManager == null) _hrManager = EmpireX.Core.GameManager.Instance.HRManager;
            
            var candidate = _hrManager.GetCandidate(candidateId);
            if (candidate == null || candidate.IsExecutive) 
            {
                _eventBus.Publish(new EmployeeActionFailed { Reason = "Aday bulunamadı veya bu aday bir yönetici." });
                return null;
            }

            var company = _companyManager.GetCompany(companyId);
            if (company == null) return null;

            var office = _officeManager.GetOfficeByOwner(companyId);
            int currentEmployees = _employees.Count(e => e.CompanyId == companyId);
            int maxEmployees = office != null ? office.MaxEmployees : 5;

            if (currentEmployees >= maxEmployees)
            {
                _eventBus.Publish(new EmployeeActionFailed { Reason = "Ofis kapasitesi dolu. Yeni ofis alın veya mevcut ofisi geliştirin." });
                return null;
            }

            double hiringCost = candidate.ExpectedSalary * 1.5; // İşe alım komisyonu
            if (!_economyManager.TrySpend(hiringCost, "Hiring Fee"))
            {
                _eventBus.Publish(new EmployeeActionFailed { Reason = "İşe alım komisyonu için bakiye yetersiz." });
                return null;
            }

            var emp = new EmployeeData
            {
                Id = Guid.NewGuid().ToString(),
                CompanyId = companyId,
                EmployeeTypeId = candidate.TypeId,
                Name = candidate.Name,
                Age = candidate.Age,
                PortraitPath = candidate.PortraitPath,
                Level = 1,
                Salary = candidate.ExpectedSalary,
                Skill = CalculateBaseSkillAverage(candidate.Skills),
                Experience = candidate.Experience,
                Happiness = 80,
                Stress = 20,
                Loyalty = 50,
                Productivity = candidate.BaseProductivity
            };

            _employees.Add(emp);
            company.EmployeeIds.Add(emp.Id);
            company.Expense += emp.Salary;

            // Adayı havuzdan sil
            _hrManager.RemoveCandidate(candidateId);

            _eventBus.Publish(new EmployeeHired { EmployeeId = emp.Id, CompanyId = companyId });
            return emp;
        }

        private float CalculateBaseSkillAverage(List<EmployeeSkillValue> skills)
        {
            if (skills == null || skills.Count == 0) return 10f;
            return (float)skills.Average(s => s.Value);
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
            var companyTypes = Resources.LoadAll<CompanyTypeSO>("CompanyType");
            var employeeTypes = Resources.LoadAll<EmployeeTypeSO>("EmployeeType");

            foreach (var emp in _employees)
            {
                var company = _companyManager.GetCompany(emp.CompanyId);
                if (company == null) continue;

                var office = _officeManager.GetOfficeByOwner(emp.CompanyId);
                float happinessBonus = office != null ? office.HappinessBonus : 0f;
                float prodBonus = office != null ? office.ProductivityBonus : 0f;

                emp.Stress += (float)(_rnd.NextDouble() * 2 - 0.5);
                
                if (_executiveManager != null && _executiveManager.HasExecutive(emp.CompanyId, "HR"))
                {
                    emp.Stress -= 1.5f;
                }

                emp.Stress = Math.Clamp(emp.Stress, 0, 100);

                if (emp.Stress > 80)
                {
                    emp.Happiness -= 1f;
                }
                
                emp.Happiness = Math.Clamp(emp.Happiness + (happinessBonus * 0.01f), 0, 100);
                emp.Experience += 0.5f; 

                // Yeni Verimlilik (Productivity) Matematiği
                float skillScore = CalculateSkillScore(emp, company, companyTypes, employeeTypes);
                
                // Base productivity (EmployeeTypeSO üzerinden)
                var empType = employeeTypes.FirstOrDefault(t => t.Id == emp.EmployeeTypeId);
                float baseProd = empType != null ? empType.BaseProductivity : 50f;

                emp.Productivity = baseProd * (skillScore / 100f) * (emp.Happiness / 100f) * (1 - (emp.Stress/200f));
                emp.Productivity += prodBonus; 
            }
        }

        private float CalculateSkillScore(EmployeeData emp, CompanyData company, CompanyTypeSO[] companyTypes, EmployeeTypeSO[] employeeTypes)
        {
            var compType = companyTypes.FirstOrDefault(c => c.Id == company.CompanyTypeId);
            var empType = employeeTypes.FirstOrDefault(t => t.Id == emp.EmployeeTypeId);

            if (compType == null || empType == null) return emp.Skill; // Fallback

            float score = 0f;
            foreach (var req in compType.RequiredSkillses)
            {
                // Çalışanın o yetenekteki değerini bul
                var empSkill = empType.Skills.FirstOrDefault(s => s.SkillType == req.SkillType);
                float val = empSkill != null ? empSkill.Value : 0f;

                // Ağırlıkla (Weight) çarp (Weight yüzdeliktir, örneğin %70 ise 0.70 ile çarp)
                score += val * (req.Weight / 100f);
            }

            // Eğer şirket tipi bir ağırlık tanımlamadıysa fallback dön
            if (compType.RequiredSkillses == null || compType.RequiredSkillses.Count == 0)
                return emp.Skill;

            return score;
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

