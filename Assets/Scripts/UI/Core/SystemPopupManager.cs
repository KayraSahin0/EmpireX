using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using EmpireX.Events;

namespace EmpireX.UI
{
    public class SystemPopupManager : MonoBehaviour
    {
        public static SystemPopupManager Instance { get; private set; }

        [Header("UI References")]
        [SerializeField] private GameObject _popupRoot;
        [SerializeField] private CanvasGroup _blockerPanel;
        [SerializeField] private RectTransform _popupWindow;
        
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private TMP_Text _technicalDetailsText;
        
        [SerializeField] private GameObject _buttonContainer;
        
        [SerializeField] private Button _button1;
        [SerializeField] private TMP_Text _button1Text;
        
        [SerializeField] private Button _button2;
        [SerializeField] private TMP_Text _button2Text;

        private Action _button1Callback;
        private Action _button2Callback;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (EmpireX.Core.GameManager.Instance != null && EmpireX.Core.GameManager.Instance.EventManager != null)
            {
                SubscribeToEvents(EmpireX.Core.GameManager.Instance.EventManager.EventBus);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInitialize()
        {
            if (Instance == null)
            {
                var popupPrefab = Resources.Load<GameObject>("UI/SystemPopupManager");
                if (popupPrefab != null)
                {
                    Instantiate(popupPrefab);
                }
            }
        }

        private void Initialize()
        {
            _popupRoot.SetActive(false);
            
            _button1.onClick.AddListener(OnButton1Clicked);
            _button2.onClick.AddListener(OnButton2Clicked);
            
            // EventBus'a abone ol (GameManager'dan daha erken uyanabilme ihtimaline karşı geç abone olma vb.)
            // Ancak en güvenlisi Initialize edildiğinde dinlemeye başlamaktır.
        }

        public void SubscribeToEvents(IEventBus eventBus)
        {
            eventBus.Subscribe<SystemErrorEvent>(OnSystemError);
            eventBus.Subscribe<ShowSystemPopupEvent>(OnShowSystemPopup);
        }

        public void UnsubscribeFromEvents(IEventBus eventBus)
        {
            eventBus.Unsubscribe<SystemErrorEvent>(OnSystemError);
            eventBus.Unsubscribe<ShowSystemPopupEvent>(OnShowSystemPopup);
        }

        private void OnSystemError(SystemErrorEvent e)
        {
            // Eğer oyun durdurulduysa veya teknik bir çökme varsa UI'yi göster
            
            string title = e.Severity == ErrorSeverity.Critical ? "Kritik Hata" : "Uyarı";
            string message = "Oyun sırasında beklenmeyen bir hata oluştu.";
            
            // Sadece Editor veya Development Build'de teknik detay gösterilir
            string technicalDetails = Debug.isDebugBuild ? $"{e.ErrorMessage}\n\n{e.StackTrace}" : "";

            Action btn1Action = null;
            string btn1Text = "";
            
            Action btn2Action = null;
            string btn2Text = "";

            if (e.Severity == ErrorSeverity.Critical)
            {
                message += "\n\nOyun durumu tehlikede olabilir. Güvenliğiniz için son kayıt noktasına dönmeniz önerilir.";
                
                btn1Text = "Son Kayda Dön";
                btn1Action = () => 
                {
                    if (EmpireX.Core.GameManager.Instance != null && EmpireX.Core.GameManager.Instance.SaveManager != null)
                    {
                        string currentSlot = EmpireX.Core.GameManager.Instance.SaveManager.CurrentSlotId;
                        if (!string.IsNullOrEmpty(currentSlot))
                        {
                            EmpireX.Core.GameManager.Instance.SaveManager.LoadGame(currentSlot);
                            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
                        }
                        else
                        {
                            // Kayıt yoksa menüye dön
                            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                        }
                    }
                };

                btn2Text = "Raporla ve Dön";
                btn2Action = () => 
                {
                    // Raporlamayı TelemetryManager devralır (Zaten fırlatılan Event'i dinliyor).
                    // Biz sadece UI'dan dönüş işlemini yaparız.
                    btn1Action?.Invoke();
                };
            }
            else
            {
                btn1Text = "Devam Et";
                btn1Action = () => { /* Sadece kapatır */ };
                
                btn2Text = "Raporla";
                btn2Action = () => 
                {
                    // Raporlamayı TelemetryManager yapar
                };
            }

            ShowPopup(new ShowSystemPopupEvent
            {
                Title = title,
                Message = message,
                Severity = e.Severity,
                Button1Text = btn1Text,
                Button1Callback = btn1Action,
                Button2Text = btn2Text,
                Button2Callback = btn2Action,
                TechnicalDetails = technicalDetails
            });
            
            // TelemetryManager'ı da tetiklemek için hata eventini Telemetry'e aktarıyoruz.
            // Fakat TelemetryManager zaten SystemErrorEvent dinlediği için ekstra bir şey yapmaya gerek yok.
        }

        private void OnShowSystemPopup(ShowSystemPopupEvent e)
        {
            ShowPopup(e);
        }

        private void ShowPopup(ShowSystemPopupEvent e)
        {
            // Eğer halihazırda açıksa animasyon çakışmasını engelle
            DOTween.Kill(_popupWindow);
            DOTween.Kill(_blockerPanel);

            _titleText.text = e.Title;
            _messageText.text = e.Message;
            
            if (_technicalDetailsText != null)
            {
                _technicalDetailsText.text = e.TechnicalDetails;
                _technicalDetailsText.gameObject.SetActive(!string.IsNullOrEmpty(e.TechnicalDetails));
            }

            SetupButton(_button1, _button1Text, e.Button1Text, e.Button1Callback, ref _button1Callback);
            SetupButton(_button2, _button2Text, e.Button2Text, e.Button2Callback, ref _button2Callback);

            if (_buttonContainer != null)
            {
                bool hasAnyButton = !string.IsNullOrEmpty(e.Button1Text) || !string.IsNullOrEmpty(e.Button2Text);
                _buttonContainer.SetActive(hasAnyButton);
            }

            _popupRoot.SetActive(true);
            
            // Animasyonlar (DOTween)
            _blockerPanel.alpha = 0f;
            DOTween.To(() => _blockerPanel.alpha, x => _blockerPanel.alpha = x, 1f, 0.3f).SetUpdate(true); // SetUpdate(true) Time.timeScale = 0 olsa bile çalışmasını sağlar
            
            _popupWindow.localScale = Vector3.one * 0.8f;
            _popupWindow.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetUpdate(true);

            if (e.AutoCloseDuration > 0)
            {
                DOVirtual.DelayedCall(e.AutoCloseDuration, () => 
                {
                    if (_popupRoot.activeSelf) ClosePopup();
                }, ignoreTimeScale: true);
            }
        }

        private void SetupButton(Button btn, TMP_Text btnText, string text, Action callback, ref Action storedCallback)
        {
            if (string.IsNullOrEmpty(text))
            {
                btn.gameObject.SetActive(false);
                storedCallback = null;
            }
            else
            {
                btn.gameObject.SetActive(true);
                btnText.text = text;
                storedCallback = callback;
            }
        }

        private void OnButton1Clicked()
        {
            _button1Callback?.Invoke();
            ClosePopup();
        }

        private void OnButton2Clicked()
        {
            _button2Callback?.Invoke();
            ClosePopup();
        }

        private void ClosePopup()
        {
            DOTween.Kill(_popupWindow);
            DOTween.Kill(_blockerPanel);

            _popupWindow.DOScale(0.8f, 0.2f).SetEase(Ease.InBack).SetUpdate(true);
            DOTween.To(() => _blockerPanel.alpha, x => _blockerPanel.alpha = x, 0f, 0.2f).SetUpdate(true).OnComplete(() =>
            {
                _popupRoot.SetActive(false);
            });
        }
    }
}
