using UnityEngine;

namespace EmpireX.Data
{
    public enum AchievementType
    {
        TotalRevenue,
        TotalCompanies,
        TotalEmployees,
        TotalProfit,
        HoldingLevel
    }

    [CreateAssetMenu(fileName = "NewAchievement", menuName = "EmpireX/Data/Achievement", order = 1)]
    public class AchievementSO : ScriptableObject
    {
        public string Id;
        public string Title;
        [TextArea] public string Description;
        public AchievementType Type;
        public double TargetValue; // Hedef (Ã–rn: 1000000 ciro)
        public double CashReward;  // Ã–dÃ¼l Nakit
        public double BrandReward; // Ã–dÃ¼l Marka DeÄŸeri
    }
}

