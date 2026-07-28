using UnityEngine;
using EmpireX.Events;
using EmpireX.Audio;

namespace EmpireX.Core
{
    public class AudioManager : BaseManager
    {
        private AudioConfigSO _config;
        private AudioSource _musicSource;
        private AudioSource _sfxSource;
        private GameObject _audioContainer;

        public AudioManager(IEventBus eventBus) : base(eventBus) 
        { 
        }

        public override void Initialize()
        {
            // AudioConfig yükleniyor (Resources üzerinden)
            _config = Resources.Load<AudioConfigSO>("AudioConfig");
            if (_config == null)
            {
                Debug.LogWarning("[AudioManager] AudioConfigSO Resources/AudioConfig yolunda bulunamadı!");
            }

            // AudioSourcelar için obje oluştur
            _audioContainer = new GameObject("AudioContainer");
            GameObject.DontDestroyOnLoad(_audioContainer);

            _musicSource = _audioContainer.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;

            _sfxSource = _audioContainer.AddComponent<AudioSource>();
            _sfxSource.playOnAwake = false;

            // Eventleri Dinle
            _eventBus.Subscribe<GameStarted>(OnGameStarted);
            _eventBus.Subscribe<TransactionOccurred>(OnTransactionOccurred);
            _eventBus.Subscribe<AchievementUnlocked>(OnAchievementUnlocked);
            _eventBus.Subscribe<RandomEventTriggered>(OnRandomEventTriggered);
        }
        
        public override void Dispose()
        {
            _eventBus.Unsubscribe<GameStarted>(OnGameStarted);
            _eventBus.Unsubscribe<TransactionOccurred>(OnTransactionOccurred);
            _eventBus.Unsubscribe<AchievementUnlocked>(OnAchievementUnlocked);
            _eventBus.Unsubscribe<RandomEventTriggered>(OnRandomEventTriggered);

            if (_audioContainer != null)
            {
                GameObject.Destroy(_audioContainer);
            }
        }

        public void PlaySFX(AudioClip clip, float volume = 1f)
        {
            if (clip == null || _sfxSource == null) return;
            _sfxSource.PlayOneShot(clip, volume);
        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip == null || _musicSource == null) return;
            if (_musicSource.clip == clip) return; // Zaten çalıyorsa bölme
            
            _musicSource.clip = clip;
            _musicSource.Play();
        }

        private void OnGameStarted(GameStarted e)
        {
            if (_config != null) PlayMusic(_config.GameMusic);
        }

        private void OnTransactionOccurred(TransactionOccurred e)
        {
            if (_config == null) return;
            if (e.IsRevenue) PlaySFX(_config.MoneyGain, 0.5f);
            else PlaySFX(_config.MoneySpend, 0.5f);
        }

        private void OnAchievementUnlocked(AchievementUnlocked e)
        {
            if (_config != null) PlaySFX(_config.AchievementUnlocked, 1f);
        }

        private void OnRandomEventTriggered(RandomEventTriggered e)
        {
            if (_config == null) return;
            // Olayın tipine göre iyi/kötü ses ayrımı yapılabilir, şimdilik rastgele atandı
            PlaySFX(_config.BadEventTriggered, 0.8f);
        }
    }
}
