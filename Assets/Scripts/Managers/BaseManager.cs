using EmpireX.Events;

namespace EmpireX.Core
{
    /// <summary>
    /// Tüm yöneticiler için temel sınıf.
    /// </summary>
    public abstract class BaseManager
    {
        protected readonly IEventBus _eventBus;

        protected BaseManager(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public virtual void Initialize() { }
        public virtual void Dispose() { }
    }
}
