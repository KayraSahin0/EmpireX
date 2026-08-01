using UnityEngine;
using TMPro;
using EmpireX.Events;

namespace EmpireX.UI
{
    public class LeftHudUI : MonoBehaviour
    {
        [Header("Part 1: TopBg")]
        public TMP_Text PlayerNameText;
        public TMP_Text PlayerLevelText; // Oyuncunun seviyesini gösterecek

        [Header("Part 2: Navigation Buttons")]
        public UnityEngine.UI.Button CompanyButton;
        public UnityEngine.UI.Button HoldingButton;
        public UnityEngine.UI.Button EmployeesButton;
        public UnityEngine.UI.Button ResearchButton;
        public UnityEngine.UI.Button CityButton;
        public UnityEngine.UI.Button CountryButton;
        public UnityEngine.UI.Button ExchangeButton;

        [Header("Part 2: Target Panels")]
        public BasePanel CompanyPanel;
        public BasePanel HoldingPanel;
        public BasePanel EmployeesPanel;
        public BasePanel ResearchPanel;
        public BasePanel CityPanel;
        public BasePanel CountryPanel;
        public BasePanel ExchangePanel;

        private void Start()
        {
            if (EmpireX.Core.GameManager.Instance != null && EmpireX.Core.GameManager.Instance.EventManager != null)
            {
                // Oyun yüklendiğinde yenilenmesi için abone ol
                EmpireX.Core.GameManager.Instance.EventManager.EventBus.Subscribe<LoadCompleted>(OnDataLoaded);
                
                // İlk açılışta verileri çek
                UpdateTopBg();
            }
            else
            {
                Debug.LogWarning("[LeftHudUI] GameManager veya EventManager bulunamadı!");
            }
            // Referans kontrolleri
            if (PlayerNameText == null) Debug.LogWarning("[LeftHudUI] PlayerNameText referansı eksik!");
            if (PlayerLevelText == null) Debug.LogWarning("[LeftHudUI] PlayerLevelText referansı eksik!");
            
            // Part 2: Butonları Panellere Bağla
            BindButtonToPanel(CompanyButton, CompanyPanel);
            BindButtonToPanel(HoldingButton, HoldingPanel);
            BindButtonToPanel(EmployeesButton, EmployeesPanel);
            BindButtonToPanel(ResearchButton, ResearchPanel);
            BindButtonToPanel(CityButton, CityPanel);
            BindButtonToPanel(CountryButton, CountryPanel);
            BindButtonToPanel(ExchangeButton, ExchangePanel);
        }

        private void BindButtonToPanel(UnityEngine.UI.Button btn, BasePanel panel)
        {
            if (btn != null && panel != null)
            {
                btn.onClick.AddListener(() => 
                {
                    if (UINavigation.Instance != null)
                    {
                        UINavigation.Instance.ShowPanel(panel, keepHistory: true);
                    }
                });
            }
            else
            {
                if (btn == null) Debug.LogWarning($"[LeftHudUI] Bir buton referansı eksik, panele bağlanamadı.");
                if (panel == null) Debug.LogWarning($"[LeftHudUI] Bir panel referansı eksik, butona bağlanamadı.");
            }
        }

        private void OnDestroy()
        {
            if (EmpireX.Core.GameManager.Instance != null && EmpireX.Core.GameManager.Instance.EventManager != null)
            {
                EmpireX.Core.GameManager.Instance.EventManager.EventBus.Unsubscribe<LoadCompleted>(OnDataLoaded);
            }
        }

        private void OnDataLoaded(LoadCompleted e)
        {
            UpdateTopBg();
        }

        public void UpdateTopBg()
        {
            if (EmpireX.Core.GameManager.Instance == null || EmpireX.Core.GameManager.Instance.SaveManager == null) return;
            
            var runtimeData = EmpireX.Core.GameManager.Instance.SaveManager.CurrentData;
            if (runtimeData == null) return;

            // CEO İsmi (PlayerName)
            if (PlayerNameText != null && runtimeData.PlayerData != null)
            {
                PlayerNameText.text = runtimeData.PlayerData.PlayerName;
            }

            // Oyuncu Seviyesi (PlayerLevel)
            if (PlayerLevelText != null && runtimeData.PlayerData != null)
            {
                PlayerLevelText.text = $"Level {runtimeData.PlayerData.Level}";
            }
        }
    }
}
