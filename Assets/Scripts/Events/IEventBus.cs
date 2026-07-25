using System;

namespace EmpireX.Events
{
    /// <summary>
    /// Event Bus arayüzü. Sistemler arası iletişimi sağlar.
    /// </summary>
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> onEvent);
        void Unsubscribe<T>(Action<T> onEvent);
        void Publish<T>(T eventData);
        void Clear();
    }
}
