using UnityEngine;
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
}
