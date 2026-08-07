using UnityEngine;
using System.Collections.Generic;
namespace EmpireX.Data
{
    [System.Serializable]
    public class CompanySkillRequirement
    {
        public EmployeeSkill SkillType;
        [Range(0, 100)] public int Weight;
    }

    [CreateAssetMenu(fileName = "NewCompanyType", menuName = "EmpireX/Data/CompanyType", order = 5)]
    public class CompanyTypeSO : ScriptableObject
    {
        public string Id;
        public string Name;
        public string Category;
        public string Description;
        public List<CompanySkillRequirement> RequiredSkillses = new List<CompanySkillRequirement>();
        public double BaseCost;
        public double BaseRevenue;
        public double BaseExpense;
        public float BaseGrowth;
        public int UnlockLevel;
        public Sprite Icon;

        private void OnValidate()
        {
            if (RequiredSkillses != null && RequiredSkillses.Count > 0)
            {
                int total = 0;
                foreach (var req in RequiredSkillses)
                {
                    if (req != null) total += req.Weight;
                }

                if (total != 100 && total != 0)
                {
                    Debug.LogWarning($"[CompanyTypeSO] '{this.name}' objesindeki yetenek aÄŸÄ±rlÄ±klarÄ± (Weight) toplamÄ± 100 olmalÄ±dÄ±r! Åu anki toplam: {total}.");
                }
            }
        }
    }
}

