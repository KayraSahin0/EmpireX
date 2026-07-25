using UnityEngine;
using System.Collections.Generic;

namespace EmpireX.Data
{
    [CreateAssetMenu(fileName = "NewCompanyType", menuName = "EmpireX/Data/CompanyType")]
    public class CompanyTypeSO : ScriptableObject
    {
        public string Id;
        public string Name;
        public string Category;
        public string Description;
        public double BaseCost;
        public double BaseRevenue;
        public double BaseExpense;
        public float BaseGrowth;
        public int UnlockLevel;
        public Sprite Icon;
    }

    [CreateAssetMenu(fileName = "NewEmployeeType", menuName = "EmpireX/Data/EmployeeType")]
    public class EmployeeTypeSO : ScriptableObject
    {
        public string Id;
        public string Name;
        public double Salary;
        public float Productivity;
        public float Skill;
        public Sprite Icon;
    }

    [CreateAssetMenu(fileName = "NewExecutiveType", menuName = "EmpireX/Data/ExecutiveType")]
    public class ExecutiveTypeSO : ScriptableObject
    {
        public string Id;
        public string Name;
        public string BonusType;
        public float BonusValue;
        public double Salary;
        public Sprite Icon;
    }

    [CreateAssetMenu(fileName = "NewResearch", menuName = "EmpireX/Data/Research")]
    public class ResearchSO : ScriptableObject
    {
        public string Id;
        public string Name;
        public string Category;
        public double Cost;
        public float Duration;
        public int MaxLevel;
        public List<string> Prerequisites = new List<string>();
        public string Bonus;
    }

    [CreateAssetMenu(fileName = "NewCity", menuName = "EmpireX/Data/City")]
    public class CitySO : ScriptableObject
    {
        public string Id;
        public string Name;
        public string CountryId;
        public long Population;
        public float Tax;
        public double Rent;
        public float Workforce;
        public float Demand;
        public float Competition;
    }

    [CreateAssetMenu(fileName = "NewCountry", menuName = "EmpireX/Data/Country")]
    public class CountrySO : ScriptableObject
    {
        public string Id;
        public string Name;
        public string Currency;
        public float Tax;
        public float Inflation;
        public float InterestRate;
        public float Stability;
        public float Economy;
    }

    [CreateAssetMenu(fileName = "NewBuilding", menuName = "EmpireX/Data/Building")]
    public class BuildingSO : ScriptableObject
    {
        public string Id;
        public string Name;
        public int Level;
        public double Cost;
        public string Bonus;
    }

    [CreateAssetMenu(fileName = "NewEvent", menuName = "EmpireX/Data/Event")]
    public class EventSO : ScriptableObject
    {
        public string Id;
        public string Name;
        public string Category;
        public float Probability;
        public float Duration;
        public List<string> Effects = new List<string>();
    }

    [CreateAssetMenu(fileName = "NewAchievement", menuName = "EmpireX/Data/Achievement")]
    public class AchievementSO : ScriptableObject
    {
        public string Id;
        public string Name;
        public string Description;
        public string Reward;
        public Sprite Icon;
    }
}
