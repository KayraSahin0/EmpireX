using System.Collections.Generic;
using UnityEngine;

namespace EmpireX.UI
{
    /// <summary>
    /// Bildirimleri (Notification) Object Pooling kullanarak yöneten sistem.
    /// </summary>
    public class NotificationSystem : MonoBehaviour
    {
        [SerializeField] private NotificationItem _notificationPrefab;
        [SerializeField] private Transform _container;

        private Queue<NotificationItem> _pool = new Queue<NotificationItem>();

        public void ShowNotification(string message, int type)
        {
            if (_notificationPrefab == null) return;

            NotificationItem item = GetFromPool();
            item.Show(message, type, ReturnToPool);
        }

        private NotificationItem GetFromPool()
        {
            if (_pool.Count > 0)
            {
                var item = _pool.Dequeue();
                item.gameObject.SetActive(true);
                return item;
            }

            var newItem = Instantiate(_notificationPrefab, _container);
            return newItem;
        }

        private void ReturnToPool(NotificationItem item)
        {
            item.Hide();
            _pool.Enqueue(item);
        }
    }
}
