using UnityEngine;
using EmpireX.Events;

namespace EmpireX.Core
{
    /// <summary>
    /// Oyunun ana döngüsünü ve sistemleri başlatan yönetici sınıf.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public ConfigSystem ConfigSystem { get; private set; }
        public EventManager EventManager { get; private set; }
        public TimeManager TimeManager { get; private set; }
        public SaveManager SaveManager { get; private set; }
        public SceneManager SceneManager { get; private set; }
        public AudioManager AudioManager { get; private set; }
        public UIManager UIManager { get; private set; }
        public NotificationManager NotificationManager { get; private set; }
        public LocalizationManager LocalizationManager { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeSystems();
        }

        private void InitializeSystems()
        {
            // 1. Config Yükleme
            ConfigSystem = new ConfigSystem();
            ConfigSystem.Initialize();

            // 2. Event Bus
            EventManager = new EventManager();
            EventManager.Initialize();
            var eventBus = EventManager.EventBus;

            // 3. Sistemlerin Yaratılması (Dependency Injection)
            TimeManager = new TimeManager(eventBus);
            SaveManager = new SaveManager(eventBus);
            SceneManager = new SceneManager(eventBus);
            AudioManager = new AudioManager(eventBus);
            UIManager = new UIManager(eventBus);
            NotificationManager = new NotificationManager(eventBus);
            LocalizationManager = new LocalizationManager(eventBus);

            // 4. Sistemlerin Başlatılması
            TimeManager.Initialize();
            SaveManager.Initialize();
            SceneManager.Initialize();
            AudioManager.Initialize();
            UIManager.Initialize();
            NotificationManager.Initialize();
            LocalizationManager.Initialize();

            // 5. Oyun Başlangıç Olayı
            eventBus.Publish(new GameStarted());
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                TimeManager?.Dispose();
                SaveManager?.Dispose();
                SceneManager?.Dispose();
                AudioManager?.Dispose();
                UIManager?.Dispose();
                NotificationManager?.Dispose();
                LocalizationManager?.Dispose();
                EventManager?.Dispose();
                ConfigSystem?.Dispose();
            }
        }
    }
}
