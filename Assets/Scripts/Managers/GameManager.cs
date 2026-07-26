using UnityEngine;
using EmpireX.Events;
using EmpireX.Economy;
using EmpireX.Company;
using EmpireX.Employee;
using EmpireX.Office;

namespace EmpireX.Core
{
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
        public EconomyManager EconomyManager { get; private set; }
        public CompanyManager CompanyManager { get; private set; }
        public OfficeManager OfficeManager { get; private set; }
        public EmployeeManager EmployeeManager { get; private set; }

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
            ConfigSystem = new ConfigSystem();
            ConfigSystem.Initialize();

            EventManager = new EventManager();
            EventManager.Initialize();
            var eventBus = EventManager.EventBus;

            TimeManager = new TimeManager(eventBus, ConfigSystem);
            SaveManager = new SaveManager(eventBus);
            SceneManager = new SceneManager(eventBus);
            AudioManager = new AudioManager(eventBus);
            UIManager = new UIManager(eventBus);
            NotificationManager = new NotificationManager(eventBus);
            LocalizationManager = new LocalizationManager(eventBus);
            
            EconomyManager = new EconomyManager(eventBus, ConfigSystem);
            CompanyManager = new CompanyManager(eventBus, EconomyManager);
            OfficeManager = new OfficeManager(eventBus, EconomyManager);
            EmployeeManager = new EmployeeManager(eventBus, EconomyManager, CompanyManager, OfficeManager);

            TimeManager.Initialize();
            SaveManager.Initialize();
            SceneManager.Initialize();
            AudioManager.Initialize();
            UIManager.Initialize();
            NotificationManager.Initialize();
            LocalizationManager.Initialize();
            EconomyManager.Initialize();
            CompanyManager.Initialize();
            OfficeManager.Initialize();
            EmployeeManager.Initialize();

            SaveManager.LoadGame("AutoSaveSlot");

            eventBus.Publish(new GameStarted());
        }

        private void Update()
        {
            TimeManager?.OnUpdate(Time.deltaTime);
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
                EmployeeManager?.Dispose();
                OfficeManager?.Dispose();
                CompanyManager?.Dispose();
                EconomyManager?.Dispose();
                EventManager?.Dispose();
                ConfigSystem?.Dispose();
            }
        }
    }
}
