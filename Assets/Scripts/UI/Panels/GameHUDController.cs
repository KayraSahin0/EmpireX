using UnityEngine;
using DG.Tweening;

namespace EmpireX.UI
{
    public class GameHUDController : MonoBehaviour
    {
        [Header("Accordion Menu")]
        public RectTransform[] AccordionButtons; // PauseMenuBtn, NewsMenuBtn, SaveMenuBtn
        private bool _isAccordionOpen = false;

        [Header("Panels")]
        public BasePanel PauseMenuPanel;
        public BasePanel NewsPanel;

        private CanvasGroup _hudCanvasGroup;

        private void Start()
        {
            _hudCanvasGroup = GetComponent<CanvasGroup>();
            if (_hudCanvasGroup == null)
                _hudCanvasGroup = gameObject.AddComponent<CanvasGroup>();

            // Başlangıçta akordeon butonlarını gizle
            foreach (var btn in AccordionButtons)
            {
                btn.localScale = Vector3.zero;
                btn.gameObject.SetActive(false);
            }

            if (UINavigation.Instance != null)
            {
                UINavigation.Instance.OnPanelStateChanged += HandlePanelStateChanged;
            }
        }

        private void OnDestroy()
        {
            if (UINavigation.Instance != null)
            {
                UINavigation.Instance.OnPanelStateChanged -= HandlePanelStateChanged;
            }
        }

        private void HandlePanelStateChanged(bool hasActivePanel)
        {
            if (_hudCanvasGroup != null)
            {
                bool shouldBlock = hasActivePanel;
                
                // NewsPanel açıldığında HUD'ı kitleme, tıklanabilir kalsın
                if (hasActivePanel && UINavigation.Instance.CurrentPanel == NewsPanel)
                {
                    shouldBlock = false;
                }
                
                _hudCanvasGroup.blocksRaycasts = !shouldBlock;
            }
        }

        public void ToggleAccordionMenu()
        {
            if (_isAccordionOpen) CloseAccordionMenu();
            else OpenAccordionMenu();
        }

        public void OpenAccordionMenu()
        {
            if (_isAccordionOpen) return;
            _isAccordionOpen = true;
            
            // Eğer NewsPanel açıksa, akordeon açıldığında kapat
            if (NewsPanel != null && UINavigation.Instance.CurrentPanel == NewsPanel)
            {
                UINavigation.Instance.GoBack();
            }
            
            float duration = 0.2f;

            for (int i = 0; i < AccordionButtons.Length; i++)
            {
                var btn = AccordionButtons[i];
                DOTween.Kill(btn);
                btn.gameObject.SetActive(true);
                btn.DOScale(1f, duration).SetEase(Ease.OutBack).SetDelay(i * 0.05f).SetUpdate(true);
            }
        }

        public void CloseAccordionMenu()
        {
            if (!_isAccordionOpen) return;
            _isAccordionOpen = false;
            float duration = 0.2f;

            for (int i = AccordionButtons.Length - 1; i >= 0; i--)
            {
                var btn = AccordionButtons[i];
                DOTween.Kill(btn);
                btn.DOScale(0f, duration).SetEase(Ease.InBack).SetDelay((AccordionButtons.Length - 1 - i) * 0.05f).SetUpdate(true).OnComplete(() =>
                {
                    btn.gameObject.SetActive(false);
                });
            }
        }

        public void OnPauseMenuBtnClicked()
        {
            CloseAccordionMenu();
            if (PauseMenuPanel != null)
                UINavigation.Instance.ShowPanel(PauseMenuPanel);
        }

        public void OnNewsMenuBtnClicked()
        {
            CloseAccordionMenu();
            if (NewsPanel != null)
                UINavigation.Instance.ShowPanel(NewsPanel);
        }

        public void OnSaveMenuBtnClicked()
        {
            CloseAccordionMenu();
            
            if (EmpireX.Core.GameManager.Instance != null && EmpireX.Core.GameManager.Instance.SaveManager != null)
            {
                string currentHolding = EmpireX.Core.GameManager.Instance.SaveManager.CurrentSlotId;
                EmpireX.Core.GameManager.Instance.SaveManager.ManualSave(currentHolding);

                if (EmpireX.Core.GameManager.Instance.EventManager != null)
                {
                    EmpireX.Core.GameManager.Instance.EventManager.EventBus.Publish(new EmpireX.Events.ShowSystemPopupEvent
                    {
                        Title = "Kayıt Başarılı",
                        Message = "Oyun başarıyla kaydedildi.",
                        Severity = EmpireX.Events.ErrorSeverity.Info,
                        AutoCloseDuration = 1.8f, // 1.8 saniye sonra otomatik kapanır
                        Button1Text = "", // Butonları gizle
                        Button2Text = ""
                    });
                }
            }
        }
    }
}
