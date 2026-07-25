using EmpireX.Events;

namespace EmpireX.Core
{
    /// <summary>
    /// LocalizationManager sınıfı.
    /// </summary>
    public class LocalizationManager : BaseManager
    {
        public LocalizationManager(IEventBus eventBus) : base(eventBus) { }

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
