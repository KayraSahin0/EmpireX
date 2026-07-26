using EmpireX.Events;
using EmpireX.Data;

namespace EmpireX.Core
{
    /// <summary>
    /// Simülasyon zaman sistemi. Tick tabanlı gün, hafta, ay, yıl döngüsünü yönetir.
    /// </summary>
    public class TimeManager : BaseManager
    {
        private readonly ConfigSystem _configSystem;
        private TimeData _timeData;
        private TimeConfig _timeConfig;
        
        private float _tickTimer;
        private bool _isPaused = true;

        public TimeManager(IEventBus eventBus, ConfigSystem configSystem) : base(eventBus) 
        {
            _configSystem = configSystem;
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Subscribe<GameStarted>(OnGameStarted);
            _eventBus.Subscribe<GamePaused>(OnGamePaused);
            _eventBus.Subscribe<GameResumed>(OnGameResumed);
            
            _timeConfig = _configSystem.GetConfig<TimeConfig>();
            
            if (_timeConfig == null)
            {
                _timeConfig = UnityEngine.ScriptableObject.CreateInstance<TimeConfig>();
                _timeConfig.TickDuration = 1f;
                _timeConfig.DaysPerMonth = 30;
                _timeConfig.MonthsPerYear = 12;
            }
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Unsubscribe<GameStarted>(OnGameStarted);
            _eventBus.Unsubscribe<GamePaused>(OnGamePaused);
            _eventBus.Unsubscribe<GameResumed>(OnGameResumed);
        }

        private void OnLoadCompleted(LoadCompleted e)
        {
            _timeData = e.Data.TimeData;
        }

        private void OnGameStarted(GameStarted e)
        {
            _isPaused = false;
            if (_timeData == null)
            {
                _timeData = new TimeData { Day = 1, Month = 1, Year = 1, Tick = 0, Week = 1 };
            }
        }

        private void OnGamePaused(GamePaused e) => _isPaused = true;
        
        private void OnGameResumed(GameResumed e) => _isPaused = false;

        public void OnUpdate(float deltaTime)
        {
            if (_isPaused || _timeData == null) return;

            _tickTimer += deltaTime;
            if (_tickTimer >= _timeConfig.TickDuration)
            {
                _tickTimer -= _timeConfig.TickDuration;
                ProcessTick();
            }
        }

        private void ProcessTick()
        {
            _eventBus.Publish(new TickStarted { Tick = _timeData.Tick });

            _timeData.Tick++;
            _timeData.Day++;

            bool newWeek = false;
            bool newMonth = false;
            bool newYear = false;

            if (_timeData.Day > 7 && _timeData.Day % 7 == 1)
            {
                _timeData.Week++;
                newWeek = true;
            }

            if (_timeData.Day > _timeConfig.DaysPerMonth)
            {
                _timeData.Day = 1;
                _timeData.Month++;
                newMonth = true;

                if (_timeData.Month > _timeConfig.MonthsPerYear)
                {
                    _timeData.Month = 1;
                    _timeData.Year++;
                    newYear = true;
                }
            }

            _eventBus.Publish(new DayStarted { Day = _timeData.Day });
            if (newWeek) _eventBus.Publish(new WeekStarted { Week = _timeData.Week });
            if (newMonth) _eventBus.Publish(new MonthStarted { Month = _timeData.Month });
            if (newYear) _eventBus.Publish(new YearStarted { Year = _timeData.Year });

            _eventBus.Publish(new DayEnded { Day = _timeData.Day });
            if (newWeek) _eventBus.Publish(new WeekEnded { Week = _timeData.Week });
            if (newMonth) _eventBus.Publish(new MonthEnded { Month = _timeData.Month });
            if (newYear) _eventBus.Publish(new YearEnded { Year = _timeData.Year });

            _eventBus.Publish(new TickCompleted { Tick = _timeData.Tick });
        }
    }
}
