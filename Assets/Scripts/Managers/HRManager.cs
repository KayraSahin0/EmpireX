using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using EmpireX.Events;
using EmpireX.Data;
using EmpireX.News;

namespace EmpireX.Employee
{
    public class HRManager : EmpireX.Core.BaseManager
    {
        private List<CandidateData> _employeeCandidates;
        private List<CandidateData> _executiveCandidates;
        private EmpireX.Core.TimeManager _timeManager;
        private NewsManager _newsManager;
        private System.Random _rnd = new System.Random();

        public HRManager(IEventBus eventBus, EmpireX.Core.TimeManager timeManager, NewsManager newsManager) : base(eventBus)
        {
            _timeManager = timeManager;
            _newsManager = newsManager;
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Subscribe<TickStarted>(OnTickStarted);
            _eventBus.Subscribe<DayStarted>(OnDayStarted);
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Unsubscribe<TickStarted>(OnTickStarted);
            _eventBus.Unsubscribe<DayStarted>(OnDayStarted);
        }

        private void OnLoadCompleted(LoadCompleted e)
        {
            if (e.Data.EmployeeCandidates == null) e.Data.EmployeeCandidates = new List<CandidateData>();
            if (e.Data.ExecutiveCandidates == null) e.Data.ExecutiveCandidates = new List<CandidateData>();
            
            _employeeCandidates = e.Data.EmployeeCandidates;
            _executiveCandidates = e.Data.ExecutiveCandidates;

            // Ä°lk gÃ¼n baÅŸlatÄ±ldÄ±ÄŸÄ±nda hemen havuzu doldur (10 aday)
            if (_timeManager.CurrentTime.Tick == 0)
            {
                FillEmployeePool();
                FillExecutivePool();
            }
        }

        private void OnDayStarted(DayStarted e)
        {
            // Her gÃ¼n sonunda bekleyen adaylarÄ±n sÃ¼resini artÄ±r
            AgeCandidates(_employeeCandidates);
            AgeCandidates(_executiveCandidates);
        }

        private void OnTickStarted(TickStarted e)
        {
            int currentHour = (int)(e.Tick % 24);
            int currentDay = _timeManager.CurrentTime.Day;

            // Her gÃ¼n 10:00'da (Tick % 24 == 10) Ã§alÄ±ÅŸan havuzunu gÃ¼ncelle
            if (currentHour == 10)
            {
                FillEmployeePool();
            }

            // Her haftanÄ±n 1. gÃ¼nÃ¼ (Pazartesi), 09:00'da yÃ¶netici havuzunu gÃ¼ncelle
            if (currentHour == 9 && (currentDay % 7 == 1 || currentDay == 1))
            {
                FillExecutivePool();
            }
        }

        private void AgeCandidates(List<CandidateData> candidates)
        {
            for (int i = candidates.Count - 1; i >= 0; i--)
            {
                var cand = candidates[i];
                cand.WaitDays++;

                if (cand.WaitDays >= cand.MaxWaitDays)
                {
                    // Aday havuzdan silinir, baÅŸka ÅŸirkete geÃ§er
                    if (_newsManager != null)
                    {
                        string jobType = cand.IsExecutive ? "Executive" : "Employee";
                        _newsManager.AddNews("Aday BaÅŸka Åirkete GeÃ§ti", $"{cand.Name} accepted an offer from another company.", 4);
                    }
                    candidates.RemoveAt(i);
                }
            }
        }

        private void FillEmployeePool()
        {
            int maxCandidates = 10;
            int needed = maxCandidates - _employeeCandidates.Count;

            if (needed <= 0) return;

            var allEmployeeTypes = Resources.LoadAll<EmployeeTypeSO>("EmployeeType");
            if (allEmployeeTypes.Length == 0) return;

            var portraits = Resources.LoadAll<Sprite>("EmployeePortraits");

            for (int i = 0; i < needed; i++)
            {
                var template = allEmployeeTypes[_rnd.Next(allEmployeeTypes.Length)];
                
                var cand = new CandidateData
                {
                    Id = Guid.NewGuid().ToString(),
                    IsExecutive = false,
                    TypeId = template.Id,
                    Name = GenerateRandomName(),
                    Age = _rnd.Next(22, 60),
                    ExpectedSalary = template.BaseSalary * (1 + (_rnd.NextDouble() * 0.2 - 0.1)), // %10 dalgalanma
                    BaseProductivity = template.BaseProductivity * (float)(1 + (_rnd.NextDouble() * 0.2 - 0.1)),
                    Experience = (float)(_rnd.NextDouble() * 100f),
                    WaitDays = 0,
                    MaxWaitDays = _rnd.Next(2, 8),
                    PreviousCompany = GenerateRandomPreviousCompany(), // 2 ile 7 gÃ¼n arasÄ± (8 exclusive)
                    Skills = new List<EmployeeSkillValue>()
                };

                // Template'in Ã¶zelliklerini kopyala
                foreach (var sk in template.Skills)
                {
                    cand.Skills.Add(new EmployeeSkillValue { SkillType = sk.SkillType, Value = sk.Value });
                }

                if (portraits.Length > 0)
                {
                    cand.PortraitPath = portraits[_rnd.Next(portraits.Length)].name;
                }

                _employeeCandidates.Add(cand);
            }
        }

        private void FillExecutivePool()
        {
            int maxCandidates = 3; // YÃ¶netici havuzu daha kÃ¼Ã§Ã¼k
            int needed = maxCandidates - _executiveCandidates.Count;

            if (needed <= 0) return;

            var allExecutiveTypes = Resources.LoadAll<ExecutiveTypeSO>("ExecutiveType");
            if (allExecutiveTypes.Length == 0) return;

            var portraits = Resources.LoadAll<Sprite>("ExecutivePortraits");

            for (int i = 0; i < needed; i++)
            {
                var template = allExecutiveTypes[_rnd.Next(allExecutiveTypes.Length)];
                
                var cand = new CandidateData
                {
                    Id = Guid.NewGuid().ToString(),
                    IsExecutive = true,
                    TypeId = template.Id,
                    Name = GenerateRandomName(),
                    Age = _rnd.Next(35, 65), // YÃ¶neticiler daha yaÅŸlÄ±
                    ExpectedSalary = template.Salary * (1 + (_rnd.NextDouble() * 0.3 - 0.1)), // Daha yÃ¼ksek maaÅŸ dalgalanmasÄ±
                    BaseProductivity = 100f, // YÃ¶neticilerin spesifik productivity'si bonuslarÄ±na baÄŸlÄ±
                    Experience = (float)(_rnd.NextDouble() * 500f + 100f), // YÃ¼ksek tecrÃ¼be
                    WaitDays = 0,
                    MaxWaitDays = _rnd.Next(2, 8),
                    PreviousCompany = GenerateRandomPreviousCompany(),
                    Bonuses = new List<string>(template.Bonuses)
                };

                if (portraits.Length > 0)
                {
                    cand.PortraitPath = portraits[_rnd.Next(portraits.Length)].name;
                }

                _executiveCandidates.Add(cand);
            }
        }

        public CandidateData GetCandidate(string id)
        {
            var cand = _employeeCandidates.FirstOrDefault(c => c.Id == id);
            if (cand == null) cand = _executiveCandidates.FirstOrDefault(c => c.Id == id);
            return cand;
        }

        public void RemoveCandidate(string id)
        {
            _employeeCandidates.RemoveAll(c => c.Id == id);
            _executiveCandidates.RemoveAll(c => c.Id == id);
        }

        private string GenerateRandomName()
        {
            string[] firstNames = { "Ahmet", "Mehmet", "AyÅŸe", "Fatma", "Ali", "Veli", "Can", "Cem", "Emily", "Michael", "Sarah", "David", "Jessica" };
            string[] lastNames = { "YÄ±lmaz", "Kaya", "Demir", "Ã‡elik", "Åahin", "YÄ±ldÄ±z", "Smith", "Johnson", "Williams", "Brown", "Jones" };
            return firstNames[_rnd.Next(firstNames.Length)] + " " + lastNames[_rnd.Next(lastNames.Length)];
        }
        private string GenerateRandomPreviousCompany()
        {
            string[] companies = { "None", "Google", "Microsoft", "Bank of America", "Tesla", "Otomotiv A.Ş.", "Tech Innovations", "Global Finance", "Local Startup", "Freelance" };
            return companies[_rnd.Next(companies.Length)];
        }
    }
}


