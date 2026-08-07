using UnityEngine;

namespace EmpireX.Data
{
    public enum MissionType
    {
        EarnRevenue,
        HireEmployee,
        CreateCompany,
        PerformAcquisition
    }

    [CreateAssetMenu(fileName = "NewMission", menuName = "EmpireX/Data/Mission", order = 10)]
    public class MissionSO : ScriptableObject
    {
        public string Id;
        public string Title;
        [TextArea] public string Description;
        public MissionType Type;
        public double TargetValue;
        public double CashReward;
    }
}

