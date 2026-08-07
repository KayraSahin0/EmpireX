using UnityEngine;
namespace EmpireX.Data
{
    [CreateAssetMenu(fileName = "NewCountry", menuName = "EmpireX/Data/Country", order = 6)]
    public class CountrySO : ScriptableObject
    {
        public string Id;
        public string Name;
        public Sprite CountryLogo;
        public string Currency;
        public float Tax;
        public float Inflation;
        public float InterestRate;
        public float Stability;
        public float Economy;
    }
}

