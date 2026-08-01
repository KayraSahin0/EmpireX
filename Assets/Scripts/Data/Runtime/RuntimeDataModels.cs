using System;
using System.Collections.Generic;

namespace EmpireX.Data
{
    [Serializable]
    public class PlayerData
    {
        public string Id;
        public string PlayerName;
        public int Level;
        public int Experience;
        public string HoldingId;
        public float PlayTime;
        public long LastLogin;
        public string Settings;
    }

    [Serializable]
    public class HoldingData
    {
        public string Id;
        public string Name;
        public int Level;
        public double Value;
        public double Cash;
        public double TotalRevenue;
        public double TotalExpense;
        public double TotalProfit;
        public int TotalEmployees;
        public double ResearchPoints;
        public List<string> CompanyIds = new List<string>();
        public List<string> CountryIds = new List<string>();
        public List<string> CityIds = new List<string>();
        public List<string> ResearchIds = new List<string>();
        public List<string> AchievementIds = new List<string>();
    }

    [Serializable]
    public class CompanyData
    {
        public string Id;
        public string Name;
        public string CompanyTypeId;
        public string CityId;
        public string CountryId;
        public int Level;
        public double Cash;
        public double Revenue;
        public double Expense;
        public double Profit;
        public double Value;
        public double Brand;
        public double MarketShare;
        public double Automation;
        public double Innovation;
        public double Risk;
        
        // Borsa (Stock Market) Verileri
        public bool IsPublic;
        public long TotalShares;
        public long PublicShares;
        public double SharePrice;
        public float DividendYield;
        
        public List<string> EmployeeIds = new List<string>();
        public List<string> BranchIds = new List<string>();
        public List<string> ExecutiveIds = new List<string>();
    }

    [Serializable]
    public class BranchData
    {
        public string Id;
        public string CompanyId;
        public string CityId;
        public int Level;
        public double Revenue;
        public double Expense;
        public int Employees;
    }

    [Serializable]
    public class EmployeeData
    {
        public string Id;
        public string CompanyId;
        public string EmployeeTypeId;
        public string Name;
        public int Level;
        public double Salary;
        public float Skill;
        public float Experience;
        public float Happiness;
        public float Stress;
        public float Loyalty;
        public float Productivity;
    }

    [Serializable]
    public class ExecutiveData
    {
        public string Id;
        public string CompanyId;
        public string ExecutiveTypeId;
        public int Level;
        public double Salary;
        public double Bonus;
    }

    [Serializable]
    public class ResearchData
    {
        public string Id;
        public float Progress;
        public int Level;
        public bool IsUnlocked;
        public float RemainingTime;
    }

    [Serializable]
    public class EconomyData
    {
        public double Revenue;
        public double Expense;
        public double Profit;
        public double CashFlow;
        public float Inflation;
        public float InterestRate;
        public float ExchangeRate;
        public float TaxRate;
        public double Loan;
        public double NetWorth;
    }

    [Serializable]
    public class MarketData
    {
        public float Demand;
        public float Supply;
        public float Competition;
        public float Trend;
    }

    [Serializable]
    public class StatisticsData
    {
        public double TotalRevenue;
        public double TotalExpense;
        public double TotalProfit;
        public int TotalCompanies;
        public int TotalBranches;
        public int TotalEmployees;
        public int TotalResearch;
        public int TotalCities;
        public int TotalCountries;
        public float TotalPlayTime;
        
        // Grafikler ve Raporlar için Tarihsel Veriler
        public List<double> NetWorthHistory = new List<double>();
        public List<double> MonthlyRevenueHistory = new List<double>();
        public List<double> MonthlyProfitHistory = new List<double>();
        public List<double> CompanyCountHistory = new List<double>();
    }

    [Serializable]
    public class TimeData
    {
        public long Tick;
        public int Day;
        public int Week;
        public int Month;
        public int Year;
    }

    [Serializable]
    public class NewsData
    {
        public string Id;
        public int Type;
        public string Title;
        public string Description;
        public long Date;
    }

    [Serializable]
    public class NotificationData
    {
        public string Id;
        public int Type;
        public string Message;
        public long Time;
    }

    [Serializable]
    public class OfficeData
    {
        public string Id;
        public string OwnerId;
        public string Name;
        public int Level;
        public double CustomizationValue;
        public float ProductivityBonus;
        public float HappinessBonus;
        public int MaxEmployees;
    }

    [Serializable]
    public class CityData
    {
        public string Id;
        public string Name;
        public double Rent;
        public float Workforce;
        public float Demand;
        public float Competition;
        public float CityBonus;
    }

    [Serializable]
    public class CountryData
    {
        public string Id;
        public string Name;
        public string Currency;
        public float TaxRate;
        public float Inflation;
        public float InterestRate;
        public float Stability; // 0-100 arası (100 en iyi)
        public float EconomyLevel; // 0-100 arası
    }

    [Serializable]
    public class MissionData
    {
        public string MissionId;
        public double CurrentProgress;
        public bool IsCompleted;
        public bool IsRewardClaimed;
    }

    [Serializable]
    public class SaveData
    {
        public string SaveVersion;
        public long SaveDate;
        public PlayerData PlayerData = new PlayerData();
        public HoldingData HoldingData = new HoldingData();
        public List<CompanyData> Companies = new List<CompanyData>();
        public List<BranchData> Branches = new List<BranchData>();
        public List<EmployeeData> Employees = new List<EmployeeData>();
        public List<ExecutiveData> Executives = new List<ExecutiveData>();
        public List<ResearchData> Researches = new List<ResearchData>();
        public List<OfficeData> Offices = new List<OfficeData>();
        public List<CityData> Cities = new List<CityData>();
        public List<CountryData> Countries = new List<CountryData>();
        public List<NewsData> News = new List<NewsData>();
        public EconomyData EconomyData = new EconomyData();
        public StatisticsData StatisticsData = new StatisticsData();
        public TimeData TimeData = new TimeData();
        
        public List<MissionData> ActiveMissions = new List<MissionData>();
        public long LastMissionRefreshTime; // Gerçek dünya zamanı (Real-time)
        
        public string ActiveWeeklyEventId;
        public long WeeklyEventEndTime; // Gerçek dünya zamanı (Real-time)

        /// <summary>
        /// Eksik listeleri tamamlar ve veriyi doğrular.
        /// </summary>
        public void ValidateAndInitializeMissing()
        {
            if (Companies == null) Companies = new List<CompanyData>();
            if (Branches == null) Branches = new List<BranchData>();
            if (Employees == null) Employees = new List<EmployeeData>();
            if (Executives == null) Executives = new List<ExecutiveData>();
            if (Researches == null) Researches = new List<ResearchData>();
            if (Offices == null) Offices = new List<OfficeData>();
            if (Cities == null) Cities = new List<CityData>();
            if (Countries == null) Countries = new List<CountryData>();
            if (News == null) News = new List<NewsData>();
            if (PlayerData == null) PlayerData = new PlayerData();
            if (HoldingData == null) HoldingData = new HoldingData();
            if (EconomyData == null) EconomyData = new EconomyData();
            if (StatisticsData == null) StatisticsData = new StatisticsData();
            
            // Tarihsel listeleri doğrula
            if (StatisticsData.NetWorthHistory == null) StatisticsData.NetWorthHistory = new List<double>();
            if (StatisticsData.MonthlyRevenueHistory == null) StatisticsData.MonthlyRevenueHistory = new List<double>();
            if (StatisticsData.MonthlyProfitHistory == null) StatisticsData.MonthlyProfitHistory = new List<double>();
            if (StatisticsData.CompanyCountHistory == null) StatisticsData.CompanyCountHistory = new List<double>();
            if (TimeData == null) TimeData = new TimeData();
            if (ActiveMissions == null) ActiveMissions = new List<MissionData>();
        }
    }
}
