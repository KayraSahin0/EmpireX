using EmpireX.Events;

namespace EmpireX.Core
{
    /// <summary>
    /// TimeManager sınıfı.
    /// </summary>
    public class TimeManager : BaseManager
    {
        public TimeManager(IEventBus eventBus) : base(eventBus) { }

        public override void Initialize()
        {
            // Initialization logic
        }
        
        public override void Dispose()
        {
            // Cleanup logic
        }
    }
}
