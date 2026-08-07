using UnityEngine;
using System.Collections.Generic;
namespace EmpireX.Data
{
    [CreateAssetMenu(fileName = "NewExecutiveType", menuName = "EmpireX/Data/ExecutiveType", order = 9)]
    public class ExecutiveTypeSO : ScriptableObject
    {
        public string Id;
        public string Name;
        public double Salary;
        public List<string> Bonuses = new List<string>();
        public Sprite Icon; // Kaldırılacak
    }
}

