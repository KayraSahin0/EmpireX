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

    [CreateAssetMenu(fileName = "NewWeeklyEvent", menuName = "EmpireX/Data/WeeklyEvent", order = 13)]
    public class WeeklyEventSO : ScriptableObject
    {
        public string Id;
        public string Title;
        [TextArea] public string Description;
        public WeeklyEventModifier ModifierType;
        public float ModifierValue; // Ã–rn: 1.2 (%20 artÄ±ÅŸ) veya 0.8 (%20 dÃ¼ÅŸÃ¼ÅŸ)
        public int DurationDays; // OlayÄ±n sÃ¼receÄŸi gÃ¼n sayÄ±sÄ± (GerÃ§ek hayatta)
    }
}

