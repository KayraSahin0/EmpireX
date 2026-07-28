using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmpireX.Data;

namespace EmpireX.UI
{
    public class AchievementUIItem : MonoBehaviour
    {
        public TMP_Text AchievementTitle;
        public TMP_Text AchievementDescriptionText;
        public TMP_Text AchievementEaringText;
        public TMP_Text AchievementProgressText;
        public TMP_Text AchievementCompleteText;
        public Slider AchievementProgressSlider;
        public Button AchievementBtn;

        private AchievementSO _config;

        public void Setup(AchievementSO config, bool isUnlocked, double currentProgress)
        {
            _config = config;

            if (AchievementTitle != null) AchievementTitle.text = config.Title;
            if (AchievementDescriptionText != null) AchievementDescriptionText.text = config.Description;
            if (AchievementEaringText != null) AchievementEaringText.text = $"${config.CashReward:N0}";

            if (isUnlocked)
            {
                if (AchievementCompleteText != null) AchievementCompleteText.gameObject.SetActive(true);
                if (AchievementProgressText != null) AchievementProgressText.gameObject.SetActive(false);
                if (AchievementProgressSlider != null) AchievementProgressSlider.gameObject.SetActive(false);
                if (AchievementBtn != null) AchievementBtn.interactable = false;
            }
            else
            {
                if (AchievementCompleteText != null) AchievementCompleteText.gameObject.SetActive(false);
                if (AchievementProgressText != null) AchievementProgressText.gameObject.SetActive(true);
                if (AchievementProgressSlider != null) AchievementProgressSlider.gameObject.SetActive(true);
                
                if (AchievementProgressSlider != null)
                {
                    AchievementProgressSlider.maxValue = (float)config.TargetValue;
                    AchievementProgressSlider.value = (float)currentProgress;
                }

                if (AchievementProgressText != null)
                {
                    AchievementProgressText.text = $"{currentProgress:N0} / {config.TargetValue:N0}";
                }

                if (AchievementBtn != null)
                {
                    // Eğer hedef ulaşıldıysa ödül butonunu aktif et
                    AchievementBtn.interactable = currentProgress >= config.TargetValue;
                }
            }
        }

        public void OnAchievementBtnClicked()
        {
            if (_config != null && EmpireX.Core.GameManager.Instance != null)
            {
                // Core Manager üzerinden ödülü al işlemi yapılabilir
                // Örnek: GameManager.Instance.AchievementManager.ClaimReward(_config.Id);
                
                // Ardından butonu pasif et
                if (AchievementBtn != null) AchievementBtn.interactable = false;
                if (AchievementCompleteText != null) AchievementCompleteText.gameObject.SetActive(true);
                if (AchievementProgressText != null) AchievementProgressText.gameObject.SetActive(false);
                if (AchievementProgressSlider != null) AchievementProgressSlider.gameObject.SetActive(false);
            }
        }
    }
}
