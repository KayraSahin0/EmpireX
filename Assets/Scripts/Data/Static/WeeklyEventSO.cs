using UnityEngine;

namespace EmpireX.Data
{
    public enum WeeklyEventModifier
    {
        GlobalRevenueBoost,
        GlobalExpenseReduction,
        EmployeeSalaryIncrease,
        ConstructionCostReduction
    }

    [CreateAssetMenu(fileName = "NewWeeklyEvent", menuName = "EmpireX/Data/WeeklyEvent")]
    public class WeeklyEventSO : ScriptableObject
    {
        public string Id;
        public string Title;
        [TextArea] public string Description;
        public WeeklyEventModifier ModifierType;
        public float ModifierValue; // Örn: 1.2 (%20 artış) veya 0.8 (%20 düşüş)
        public int DurationDays; // Olayın süreceği gün sayısı (Gerçek hayatta)
    }
}
