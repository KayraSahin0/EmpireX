using UnityEngine;
namespace EmpireX.Data
{
    [CreateAssetMenu(fileName = "NewCity", menuName = "EmpireX/Data/City")]
    public class CitySO : ScriptableObject
    {
        public string Id;
        public string Name;
        public string CountryId;
        public long Population;
        public float Tax;
        public double Rent;
        public float Workforce;
        public float Demand;
        public float Competition;
    }
}
