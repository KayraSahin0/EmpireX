using UnityEngine;
namespace EmpireX.Data
{
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
}
