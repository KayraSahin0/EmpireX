using EmpireX.Events;

namespace EmpireX.Core
{
    /// <summary>
    /// NotificationManager sınıfı.
    /// </summary>
    public class NotificationManager : BaseManager
    {
        public NotificationManager(IEventBus eventBus) : base(eventBus) { }

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
