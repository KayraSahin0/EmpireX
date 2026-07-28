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
            if (GameManager.Instance != null && GameManager.Instance.SaveManager != null)
            {
                GameManager.Instance.SaveManager.ManualSave("AutoSaveSlot");
            }
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
