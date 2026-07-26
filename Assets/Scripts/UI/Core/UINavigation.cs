using System.Collections.Generic;
using UnityEngine;

namespace EmpireX.UI
{
    /// <summary>
    /// Panel ve Popup geçişlerini yöneten navigasyon sistemi.
    /// </summary>
    public class UINavigation : MonoBehaviour
    {
        public static UINavigation Instance { get; private set; }

        private Stack<BasePopup> _popupStack = new Stack<BasePopup>();
        private BasePanel _currentPanel;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ShowPanel(BasePanel panel)
        {
            if (_currentPanel != null)
                _currentPanel.Hide();

            _currentPanel = panel;
            _currentPanel.Show();
        }

        public void ShowPopup(BasePopup popup)
        {
            _popupStack.Push(popup);
            popup.ShowPopup(OnPopupClosed);
        }

        private void OnPopupClosed()
        {
            if (_popupStack.Count > 0)
                _popupStack.Pop();
        }
    }
}
