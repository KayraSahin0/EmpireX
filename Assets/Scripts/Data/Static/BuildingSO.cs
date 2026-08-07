using UnityEngine;
namespace EmpireX.Data
{
    [CreateAssetMenu(fileName = "NewBuilding", menuName = "EmpireX/Data/Building", order = 3)]
    public class BuildingSO : ScriptableObject
    {
        public string Id;
        public string Name;
        public int Level;
        public double Cost;
        public string Bonus;
    }
}

