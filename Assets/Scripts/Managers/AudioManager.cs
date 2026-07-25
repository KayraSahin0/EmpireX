using EmpireX.Events;

namespace EmpireX.Core
{
    /// <summary>
    /// AudioManager sınıfı.
    /// </summary>
    public class AudioManager : BaseManager
    {
        public AudioManager(IEventBus eventBus) : base(eventBus) { }

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
