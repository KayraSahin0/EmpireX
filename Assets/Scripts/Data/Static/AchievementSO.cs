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

    [CreateAssetMenu(fileName = "NewAchievement", menuName = "EmpireX/Achievement")]
    public class AchievementSO : ScriptableObject
    {
        public string Id;
        public string Title;
        [TextArea] public string Description;
        public AchievementType Type;
        public double TargetValue; // Hedef (Örn: 1000000 ciro)
        public double CashReward;  // Ödül Nakit
        public double BrandReward; // Ödül Marka Değeri
    }
}
