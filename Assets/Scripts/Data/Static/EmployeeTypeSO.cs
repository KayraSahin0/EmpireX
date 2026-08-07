using UnityEngine;
using System.Collections.Generic;
namespace EmpireX.Data
{
    public enum EmployeeSkill
    {
        Programming,
        Design,
        Marketing,
        Sales,
        Finance,
        Management,
        Manufacturing,
        Research,
        Logistics,
        Healthcare
    }

    [System.Serializable]
    public class EmployeeSkillValue
    {
        public EmployeeSkill SkillType;
        public int Value;
    }

    [CreateAssetMenu(fileName = "NewEmployeeType", menuName = "EmpireX/Data/EmployeeType", order = 7)]
    public class EmployeeTypeSO : ScriptableObject
    {
        public string Id;
        public string Name;
        public double BaseSalary;
        public float BaseProductivity;
        public List<EmployeeSkillValue> Skills = new List<EmployeeSkillValue>();
        public Sprite Icon; // KaldÄ±rÄ±lacak
    }
}

