using UnityEngine;
using EmpireX.Core;

namespace EmpireX.UI
{
    public class AchievementPanel : BasePanel
    {
        public Transform AchievementContainer;
        public GameObject AchievementItemPrefab;
        public BasePanel MainMenuPanel; // Geri dönmek için

        public override void Show()
        {
            base.Show();
            PopulateAchievements();
        }

        private void PopulateAchievements()
        {
            // Eski listeyi temizle
            foreach (Transform child in AchievementContainer)
            {
                Destroy(child.gameObject);
            }

            var allAchievements = Resources.LoadAll<EmpireX.Data.AchievementSO>("Achievements");
            var saveData = GameManager.Instance?.SaveManager?.CurrentData;

            if (allAchievements == null) return;

            foreach (var config in allAchievements)
            {
                var go = Instantiate(AchievementItemPrefab, AchievementContainer);
                var itemUI = go.GetComponent<AchievementUIItem>();
                if (itemUI == null)
                {
                    Debug.LogError("[AchievementPanel] AchievementItemPrefab üzerinde 'AchievementUIItem' scripti bulunamadı! Lütfen Inspector'dan prefab'a bu scripti ekleyin.");
                    continue;
                }
                
                bool isUnlocked = false;
                double currentProgress = 0;

                if (saveData != null)
                {
                    isUnlocked = saveData.HoldingData.AchievementIds.Contains(config.Id);
                    
                    if (!isUnlocked && config.Type == EmpireX.Data.AchievementType.TotalRevenue)
                    {
                        currentProgress = saveData.HoldingData.TotalRevenue;
                    }
                }

                itemUI.Setup(config, isUnlocked, currentProgress);
            }
        }

        public void OnBackClicked()
        {
            if (MainMenuPanel != null)
            {
                UINavigation.Instance.ShowPanel(MainMenuPanel);
            }
            else
            {
                Hide();
            }
        }
    }
}
