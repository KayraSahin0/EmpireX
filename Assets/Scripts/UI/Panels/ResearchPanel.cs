using UnityEngine;
using UnityEngine.UI;

namespace EmpireX.UI
{
    public class ResearchPanel : BasePanel
    {
        [Header("Common References")]
        public Button BackBtn;

        private void Start()
        {
            if (BackBtn != null)
            {
                BackBtn.onClick.AddListener(OnBackClicked);
            }
        }

        private void OnBackClicked()
        {
            if (UINavigation.Instance != null)
            {
                UINavigation.Instance.GoBack();
            }
            else
            {
                Hide();
            }
        }
    }
}
