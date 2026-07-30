using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using EmpireX.Core;

namespace EmpireX.UI
{
    public class MainMenuPanel : BasePanel
    {
        public BasePanel NewGamePanel;
        public BasePanel LoadMenuPanel;
        public BasePanel SettingsPanel;
        public BasePanel AchievementPanel;
        
        public Button ContinueBtn;
        public TMP_Text VersionText;

        private void Start()
        {
            if (NewGamePanel != null) NewGamePanel.HideImmediate();
            if (LoadMenuPanel != null) LoadMenuPanel.HideImmediate();
            if (SettingsPanel != null) SettingsPanel.HideImmediate();
            if (AchievementPanel != null) AchievementPanel.HideImmediate();

            CheckContinueButtonState();
        }

        private void OnEnable()
        {
            if (VersionText != null)
            {
                VersionText.text = "v" + Application.version;
            }

            CheckContinueButtonState();
        }

        private void CheckContinueButtonState()
        {
            if (ContinueBtn == null) return;

            if (GameManager.Instance == null || GameManager.Instance.SaveManager == null)
            {
                ContinueBtn.interactable = false;
                return;
            }

            var allSaves = GameManager.Instance.SaveManager.GetAllSaves();
            // Eğer herhangi bir save dosyası varsa butonu aktif et, yoksa pasif yap
            ContinueBtn.interactable = allSaves != null && allSaves.Count > 0;
        }

        public void OnContinueClicked()
        {
            if (GameManager.Instance != null && GameManager.Instance.SaveManager != null)
            {
                var allSaves = GameManager.Instance.SaveManager.GetAllSaves();
                if (allSaves.Count > 0)
                {
                    // En son oynanan kaydı bul
                    allSaves.Sort((a, b) => b.Value.SaveDate.CompareTo(a.Value.SaveDate));
                    string latestSaveId = allSaves[0].Key;
                    
                    GameManager.Instance.SaveManager.LoadGame(latestSaveId);
                    
                    // Oyuncu oyuna başladığı/yüklediği an AutoSave özelliğini aktifleştiriyoruz
                    GameManager.Instance.SaveManager.SetAutoSave(true);
                    
                    GameManager.Instance.StartGameSimulation();
                    UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
                }
            }
        }

        public void OnNewGameClicked()
        {
            UINavigation.Instance.ShowPanel(NewGamePanel);
        }

        public void OnLoadClicked()
        {
            UINavigation.Instance.ShowPanel(LoadMenuPanel);
        }

        public void OnSettingsClicked()
        {
            UINavigation.Instance.ShowPanel(SettingsPanel);
        }

        public void OnAchievementClicked()
        {
            UINavigation.Instance.ShowPanel(AchievementPanel);
        }

        public void OnQuitClicked()
        {
            if (GameManager.Instance != null && GameManager.Instance.EventManager != null)
            {
                GameManager.Instance.EventManager.EventBus.Publish(new EmpireX.Events.ShowSystemPopupEvent
                {
                    Title = "Çıkış Yap",
                    Message = "Çıkmak istediğinize emin misiniz? Oyununuz otomatik olarak kaydedilecektir.",
                    Severity = EmpireX.Events.ErrorSeverity.Info,
                    Button1Text = "Çık",
                    Button1Callback = () => 
                    {
                        if (GameManager.Instance.SaveManager != null)
                        {
                            GameManager.Instance.SaveManager.AutoSave();
                        }
                        
                        Application.Quit();
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#endif
                    },
                    Button2Text = "İptal",
                    Button2Callback = () => {}
                });
            }
            else
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }
    }
}
