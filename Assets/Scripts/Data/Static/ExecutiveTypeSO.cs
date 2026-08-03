using UnityEngine;
namespace EmpireX.Data
{
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
}
