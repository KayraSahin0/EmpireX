using UnityEngine;
namespace EmpireX.Data
{
    [CreateAssetMenu(fileName = "NewEvent", menuName = "EmpireX/Data/Event", order = 8)]
    public class EventSO : ScriptableObject
    {
        public string Id;
        public string Name;
        public string Category;
        public float Probability;
        public float Duration;
        public string Effect;
    }
}

