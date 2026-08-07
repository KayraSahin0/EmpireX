using UnityEngine;

namespace EmpireX.VFX
{
    [CreateAssetMenu(fileName = "NewVFXConfig", menuName = "EmpireX/Data/VFXConfig", order = 12)]
    public class VFXConfigSO : ScriptableObject
    {
        [Header("Ekonomi Efektleri")]
        public GameObject MoneyGainParticle;
        public GameObject MoneySpendParticle;

        [Header("Oyun Ä°Ã§i Olaylar")]
        public GameObject AchievementConfetti;
        public GameObject UpgradeSparkle;
    }
}

