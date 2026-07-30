using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using EmpireX.Core;

namespace EmpireX.UI
{
    public class LoadingPanel : MonoBehaviour, ILoadingPanel
    {
        [Header("UI References")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _loadingPanelBG;
        [SerializeField] private Slider _loadingSlider;
        [SerializeField] private TMP_Text _loadingPercentText;
        [SerializeField] private TMP_Text _hintText;
        [SerializeField] private TMP_Text _loadingText;

        private void Awake()
        {
            // Başlangıç değerleri
            _canvasGroup.alpha = 0f;
            _loadingSlider.value = 0f;
            _loadingPercentText.text = "%0";
            
            // "Yükleniyor..." yazısına nabız (pulse) animasyonu
            if (_loadingText != null)
            {
                DOTween.To(() => _loadingText.color.a, x => { var c = _loadingText.color; c.a = x; _loadingText.color = c; }, 0.4f, 0.8f)
                       .SetLoops(-1, LoopType.Yoyo)
                       .SetUpdate(true);
            }
            
            SetupRandomContent();
        }

        private void SetupRandomContent()
        {
            if (GameManager.Instance == null || GameManager.Instance.ConfigSystem == null) return;
            
            var config = GameManager.Instance.ConfigSystem.GetConfig<LoadingConfig>();
            if (config != null)
            {
                // Rastgele Arkaplan
                if (config.Backgrounds != null && config.Backgrounds.Count > 0 && _loadingPanelBG != null)
                {
                    int bgIndex = Random.Range(0, config.Backgrounds.Count);
                    _loadingPanelBG.sprite = config.Backgrounds[bgIndex];
                }

                // Rastgele İpucu
                if (config.Hints != null && config.Hints.Count > 0 && _hintText != null)
                {
                    int hintIndex = Random.Range(0, config.Hints.Count);
                    _hintText.text = config.Hints[hintIndex];
                    
                    // İpucu animasyonu (önce görünmez, yavaşça gelir)
                    var c = _hintText.color;
                    c.a = 0f;
                    _hintText.color = c;
                    
                    DOTween.To(() => _hintText.color.a, x => { var hc = _hintText.color; hc.a = x; _hintText.color = hc; }, 1f, 1f)
                           .SetDelay(0.5f)
                           .SetUpdate(true);
                }
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1f, 0.5f).SetUpdate(true);
        }

        public void UpdateProgress(float progress)
        {
            // Slider'ı animasyonlu olarak doldur (smooth effect)
            DOTween.To(() => _loadingSlider.value, x => _loadingSlider.value = x, progress, 0.2f).SetUpdate(true);
            
            // Yüzde hesaplama (0-1 arasını 0-100'e çevir)
            int percent = Mathf.RoundToInt(progress * 100f);
            _loadingPercentText.text = $"%{percent}";
        }

        public void Hide(System.Action onHidden = null)
        {
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0f, 0.5f)
                   .SetUpdate(true)
                   .OnComplete(() =>
                   {
                       if (_loadingText != null)
                           DOTween.Kill(_loadingText);
                           
                       gameObject.SetActive(false);
                       onHidden?.Invoke();
                       Destroy(gameObject);
                   });
        }
    }
}
