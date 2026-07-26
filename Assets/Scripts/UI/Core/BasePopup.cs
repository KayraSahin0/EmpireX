using System;

namespace EmpireX.UI
{
    /// <summary>
    /// Uyarı, bilgi, onay gibi popup pencerelerinin temel sınıfı.
    /// </summary>
    public abstract class BasePopup : BaseView
    {
        protected Action _onClose;

        public virtual void ShowPopup(Action onClose = null)
        {
            _onClose = onClose;
            Show();
        }

        protected virtual void ClosePopup()
        {
            Hide();
            _onClose?.Invoke();
            _onClose = null;
        }
    }
}
