using EmpireX.Events;

namespace EmpireX.Core
{
    /// <summary>
    /// UIManager sınıfı.
    /// </summary>
    public class UIManager : BaseManager
    {
        public UIManager(IEventBus eventBus) : base(eventBus) { }

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
