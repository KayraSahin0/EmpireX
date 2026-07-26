using System;
using UnityEngine;

namespace EmpireX.UI
{
    public class NotificationItem : BaseView
    {
        private Action<NotificationItem> _onComplete;
        private float _timer;
        private float _duration = 3f;

        public void Show(string message, int type, Action<NotificationItem> onComplete)
        {
            _onComplete = onComplete;
            _timer = 0;
            // TODO: UI Metin güncellemesi ve renk ataması
            Show();
        }

        private void Update()
        {
            if (gameObject.activeSelf)
            {
                _timer += Time.deltaTime;
                if (_timer >= _duration)
                {
                    _onComplete?.Invoke(this);
                }
            }
        }
    }
}
