using EmpireX.Events;

namespace EmpireX.Core
{
    /// <summary>
    /// EventBus yaşam döngüsünü yönetir.
    /// </summary>
    public class EventManager : BaseManager
    {
        public IEventBus EventBus { get; private set; }

        public EventManager() : base(null) { }

        public override void Initialize()
        {
            EventBus = new EventBus();
        }

        public override void Dispose()
        {
            EventBus?.Clear();
        }
    }
}
