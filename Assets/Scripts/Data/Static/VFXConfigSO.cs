using UnityEngine;

namespace EmpireX.VFX
{
    [CreateAssetMenu(fileName = "NewVFXConfig", menuName = "EmpireX/Data/VFXConfig")]
    public class VFXConfigSO : ScriptableObject
    {
        [Header("Ekonomi Efektleri")]
        public GameObject MoneyGainParticle;
        public GameObject MoneySpendParticle;

        [Header("Oyun İçi Olaylar")]
        public GameObject AchievementConfetti;
        public GameObject UpgradeSparkle;
    }
}
