using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmpireX.Core;

namespace EmpireX.UI
{
    public class SettingsPanel : BasePanel
    {
        public Slider EffectVolumeSlider;
        public TMP_Text EffectVolumePercentText;
        
        public Slider MusicVolumeSlider;
        public TMP_Text MusicVolumePercentText;

        public TMP_Dropdown LanguageDropdown;
        public Image LanguageIcon;
        
        public TMP_Dropdown TimeSpeedDropdown;
        
        public Slider AutomaticSaveSlider;
        public Slider NotificationSlider;
        
        public BasePanel MainMenuPanel; // Geri dönmek için

        private void Start()
        {
            if (EffectVolumeSlider != null)
                EffectVolumeSlider.onValueChanged.AddListener(OnEffectVolumeChanged);
            if (MusicVolumeSlider != null)
                MusicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            if (LanguageDropdown != null)
                LanguageDropdown.onValueChanged.AddListener(OnLanguageChanged);
            if (TimeSpeedDropdown != null)
                TimeSpeedDropdown.onValueChanged.AddListener(OnTimeSpeedChanged);
            if (AutomaticSaveSlider != null)
                AutomaticSaveSlider.onValueChanged.AddListener(OnAutoSaveChanged);
            if (NotificationSlider != null)
                NotificationSlider.onValueChanged.AddListener(OnNotificationChanged);
        }

        private void OnEnable()
        {
            if (GameManager.Instance != null && GameManager.Instance.SaveManager != null)
            {
                if (AutomaticSaveSlider != null)
                {
                    // Menü açıldığında slider'ı backend verisiyle güncelle (Event tetiklemeden)
                    AutomaticSaveSlider.SetValueWithoutNotify(GameManager.Instance.SaveManager.IsAutoSaveEnabled ? 1 : 0);
                }
            }
        }

        private void OnEffectVolumeChanged(float value)
        {
            if (EffectVolumePercentText != null)
                EffectVolumePercentText.text = $"{(value * 100):0}%";
            // TODO: ConfigSystem üzerinden ayar kaydı yapılabilir
        }

        private void OnMusicVolumeChanged(float value)
        {
            if (MusicVolumePercentText != null)
                MusicVolumePercentText.text = $"{(value * 100):0}%";
        }

        private void OnLanguageChanged(int index)
        {
            if (LanguageIcon != null && LanguageDropdown.options.Count > index)
            {
                LanguageIcon.sprite = LanguageDropdown.options[index].image;
            }
        }

        private void OnTimeSpeedChanged(int index)
        {
            // 0: Yavaş, 1: Normal
        }

        private void OnAutoSaveChanged(float value)
        {
            bool isAutoSave = value > 0.5f;
            
            if (GameManager.Instance != null && GameManager.Instance.SaveManager != null)
            {
                GameManager.Instance.SaveManager.SetAutoSave(isAutoSave);
            }
            
            // 0 veya 1 değerine sabitle
            if (AutomaticSaveSlider != null) AutomaticSaveSlider.value = isAutoSave ? 1 : 0;
        }

        private void OnNotificationChanged(float value)
        {
            bool isNotifOn = value > 0.5f;
            if (NotificationSlider != null) NotificationSlider.value = isNotifOn ? 1 : 0;
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
