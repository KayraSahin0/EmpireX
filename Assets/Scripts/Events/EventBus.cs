using System;
using System.Collections.Generic;

namespace EmpireX.Events
{
    /// <summary>
    /// Event Bus implementasyonu. Observer pattern uygular.
    /// </summary>
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
            {
                _subscribers[type] = new List<Delegate>();
            }
            _subscribers[type].Add(onEvent);
        }

        public void Unsubscribe<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            if (_subscribers.ContainsKey(type))
            {
                _subscribers[type].Remove(onEvent);
            }
        }

        public void Publish<T>(T eventData)
        {
            var type = typeof(T);
            if (_subscribers.ContainsKey(type))
            {
                var handlers = _subscribers[type];
                for (int i = handlers.Count - 1; i >= 0; i--)
                {
                    (handlers[i] as Action<T>)?.Invoke(eventData);
                }
            }
        }

        public void Clear()
        {
            _subscribers.Clear();
        }
    }
}
