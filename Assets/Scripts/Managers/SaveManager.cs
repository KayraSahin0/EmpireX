using EmpireX.Events;

namespace EmpireX.Core
{
    /// <summary>
    /// SaveManager sınıfı.
    /// </summary>
    public class SaveManager : BaseManager
    {
        public SaveManager(IEventBus eventBus) : base(eventBus) { }

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
