using UnityEngine;
using EmpireX.Events;
using EmpireX.Economy;
using EmpireX.Company;
using EmpireX.Employee;
using EmpireX.Office;
using EmpireX.Branch;
using EmpireX.Holding;
using EmpireX.Executive;
using EmpireX.Research;
using EmpireX.City;
using EmpireX.Country;
using EmpireX.Market;
using EmpireX.RandomEvents;
using EmpireX.News;
using EmpireX.Statistics;
using EmpireX.Acquisition;
using EmpireX.StockMarket;
using EmpireX.Achievements;

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
        
        public MarketManager MarketManager { get; private set; }
        public CountryManager CountryManager { get; private set; }
        public CityManager CityManager { get; private set; }
        public EconomyManager EconomyManager { get; private set; }
        public CompanyManager CompanyManager { get; private set; }
        public OfficeManager OfficeManager { get; private set; }
        public ExecutiveManager ExecutiveManager { get; private set; }
        public EmployeeManager EmployeeManager { get; private set; }
        public BranchManager BranchManager { get; private set; }
        public HoldingManager HoldingManager { get; private set; }
        public ResearchManager ResearchManager { get; private set; }
        public RandomEventManager RandomEventManager { get; private set; }
        public NewsManager NewsManager { get; private set; }
        public StatisticsManager StatisticsManager { get; private set; }
        public AcquisitionManager AcquisitionManager { get; private set; }
        public StockManager StockManager { get; private set; }
        public AchievementManager AchievementManager { get; private set; }

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
            
            CountryManager = new CountryManager(eventBus);
            CityManager = new CityManager(eventBus);
            EconomyManager = new EconomyManager(eventBus, ConfigSystem);
            ResearchManager = new ResearchManager(eventBus, EconomyManager);
            CompanyManager = new CompanyManager(eventBus, EconomyManager, CityManager);
            MarketManager = new MarketManager(eventBus, CompanyManager);
            RandomEventManager = new RandomEventManager(eventBus, EconomyManager, CompanyManager, CityManager, CountryManager);
            NewsManager = new NewsManager(eventBus);
            StatisticsManager = new StatisticsManager(eventBus, CompanyManager);
            AcquisitionManager = new AcquisitionManager(eventBus, EconomyManager, CompanyManager);
            StockManager = new StockManager(eventBus, EconomyManager, CompanyManager);
            AchievementManager = new AchievementManager(eventBus, EconomyManager);
            OfficeManager = new OfficeManager(eventBus, EconomyManager);
            ExecutiveManager = new ExecutiveManager(eventBus, EconomyManager, CompanyManager);
            EmployeeManager = new EmployeeManager(eventBus, EconomyManager, CompanyManager, OfficeManager, ExecutiveManager);
            BranchManager = new BranchManager(eventBus, EconomyManager, CompanyManager, CityManager);
            HoldingManager = new HoldingManager(eventBus, EconomyManager, CompanyManager, BranchManager);

            TimeManager.Initialize();
            SaveManager.Initialize();
            SceneManager.Initialize();
            AudioManager.Initialize();
            UIManager.Initialize();
            NotificationManager.Initialize();
            LocalizationManager.Initialize();
            CountryManager.Initialize();
            CityManager.Initialize();
            EconomyManager.Initialize();
            ResearchManager.Initialize();
            CompanyManager.Initialize();
            MarketManager.Initialize();
            RandomEventManager.Initialize();
            NewsManager.Initialize();
            StatisticsManager.Initialize();
            AcquisitionManager.Initialize();
            StockManager.Initialize();
            AchievementManager.Initialize();
            OfficeManager.Initialize();
            ExecutiveManager.Initialize();
            EmployeeManager.Initialize();
            BranchManager.Initialize();
            HoldingManager.Initialize();

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
                HoldingManager?.Dispose();
                BranchManager?.Dispose();
                EmployeeManager?.Dispose();
                ExecutiveManager?.Dispose();
                OfficeManager?.Dispose();
                CompanyManager?.Dispose();
                ResearchManager?.Dispose();
                EconomyManager?.Dispose();
                CityManager?.Dispose();
                CountryManager?.Dispose();
                MarketManager?.Dispose();
                RandomEventManager?.Dispose();
                NewsManager?.Dispose();
                StatisticsManager?.Dispose();
                AcquisitionManager?.Dispose();
                StockManager?.Dispose();
                AchievementManager?.Dispose();
                EventManager?.Dispose();
                ConfigSystem?.Dispose();
            }
        }
    }
}
