using UnityEngine;

namespace EmpireX.Data
{
    [CreateAssetMenu(fileName = "EconomyConfig", menuName = "EmpireX/Config/Economy")]
    public class EconomyConfig : ScriptableObject
    {
        public double StartingCash = 100000;
        public float DefaultTax = 0.2f;
        public float DefaultInflation = 0.05f;
        public float DefaultInterest = 0.1f;
    }

    [CreateAssetMenu(fileName = "TimeConfig", menuName = "EmpireX/Config/Time")]
    public class TimeConfig : ScriptableObject
    {
        public float TickDuration = 1f;
        public int TicksPerDay = 24; // 1 günde kaç Tick (saat) var?
        public int DaysPerMonth = 30;
        public int MonthsPerYear = 12;
    }

    [CreateAssetMenu(fileName = "TimeIconConfig", menuName = "EmpireX/Config/TimeIcon")]
    public class TimeIconConfig : ScriptableObject
    {
        [Header("Zaman İkonları (0-24 saat aralığı için)")]
        [Tooltip("00:00 - 04:00 arası")] public Sprite MidnightIcon;
        [Tooltip("04:00 - 08:00 arası")] public Sprite SunriseIcon;
        [Tooltip("08:00 - 12:00 arası")] public Sprite SmallSunIcon;
        [Tooltip("12:00 - 16:00 arası")] public Sprite SunIcon;
        [Tooltip("16:00 - 20:00 arası")] public Sprite SunsetIcon;
        [Tooltip("20:00 - 24:00 arası")] public Sprite NightIcon;
    }

    [CreateAssetMenu(fileName = "GameplayConfig", menuName = "EmpireX/Config/Gameplay")]
    public class GameplayConfig : ScriptableObject
    {
        public int MaxCompanies = 100;
        public int MaxBranches = 500;
        public int MaxEmployees = 10000;
    }

    [CreateAssetMenu(fileName = "AudioConfig", menuName = "EmpireX/Config/Audio")]
    public class AudioConfig : ScriptableObject
    {
        public float MusicVolume = 1f;
        public float SfxVolume = 1f;
    }

    [CreateAssetMenu(fileName = "UIConfig", menuName = "EmpireX/Config/UI")]
    public class UIConfig : ScriptableObject
    {
        public float AnimationDuration = 0.2f;
        public float NotificationDuration = 3f;
    }
}
