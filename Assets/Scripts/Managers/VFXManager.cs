using UnityEngine;
using EmpireX.Events;
using EmpireX.VFX;

namespace EmpireX.Core
{
    public class VFXManager : BaseManager
    {
        private VFXConfigSO _config;

        public VFXManager(IEventBus eventBus) : base(eventBus) 
        { 
        }

        public override void Initialize()
        {
            _config = Resources.Load<VFXConfigSO>("VFXConfig");
            if (_config == null)
            {
                Debug.LogWarning("[VFXManager] VFXConfigSO Resources/VFXConfig yolunda bulunamadı!");
            }

            _eventBus.Subscribe<AchievementUnlocked>(OnAchievementUnlocked);
        }
        
        public override void Dispose()
        {
            _eventBus.Unsubscribe<AchievementUnlocked>(OnAchievementUnlocked);
        }

        public void PlayVFX(GameObject prefab, Vector3 position)
        {
            if (prefab == null) return;
            var vfx = GameObject.Instantiate(prefab, position, Quaternion.identity);
            
            // Eğer ParticleSystem varsa, bittiğinde otomatik yok olmasını sağla
            var ps = vfx.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var main = ps.main;
                main.stopAction = ParticleSystemStopAction.Destroy;
            }
            else
            {
                // Değilse 3 saniye sonra zorla yok et
                GameObject.Destroy(vfx, 3f);
            }
        }

        private void OnAchievementUnlocked(AchievementUnlocked e)
        {
            if (_config == null) return;
            // Başarım açılınca ekranın ortasında konfeti patlat (Örnek)
            PlayVFX(_config.AchievementConfetti, Vector3.zero);
        }
    }
}
