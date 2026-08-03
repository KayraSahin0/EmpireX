using UnityEngine;
using System.Collections.Generic;
namespace EmpireX.Data
{
    [CreateAssetMenu(fileName = "NewResearch", menuName = "EmpireX/Data/Research")]
    public class ResearchSO : ScriptableObject
    {
        public string Id;
        public string Name;
        public string Category;
        public double Cost;
        public float Duration;
        public int MaxLevel;
        public List<string> Prerequisites = new List<string>();
        public string Bonus;
    }
}
