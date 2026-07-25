using EmpireX.Events;

namespace EmpireX.Core
{
    /// <summary>
    /// SceneManager sınıfı.
    /// </summary>
    public class SceneManager : BaseManager
    {
        public SceneManager(IEventBus eventBus) : base(eventBus) { }

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
