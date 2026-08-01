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

        public event System.Action<bool> OnPanelStateChanged;

        private Stack<BasePopup> _popupStack = new Stack<BasePopup>();
        private Stack<BasePanel> _panelHistory = new Stack<BasePanel>();
        private BasePanel _currentPanel;

        public BasePanel CurrentPanel => _currentPanel;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ShowPanel(BasePanel panel, bool keepHistory = true)
        {
            if (_currentPanel != null)
            {
                if (keepHistory) 
                    _panelHistory.Push(_currentPanel);
                    
                _currentPanel.Hide();
            }

            _currentPanel = panel;
            _currentPanel.Show();
            
            OnPanelStateChanged?.Invoke(true);
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

        public void GoBack()
        {
            if (_panelHistory.Count > 0)
            {
                if (_currentPanel != null)
                    _currentPanel.Hide();

                _currentPanel = _panelHistory.Pop();
                _currentPanel.Show();
            }
            else
            {
                // Geri dönülecek panel yoksa, bu kök (root) paneldir. Sadece kapat.
                if (_currentPanel != null)
                {
                    _currentPanel.Hide();
                    _currentPanel = null;
                    
                    OnPanelStateChanged?.Invoke(false);
                }
            }
        }
    }
}
