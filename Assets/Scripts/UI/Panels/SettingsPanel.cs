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
        
        private bool _isInitializing = false;

        private void Start()
        {
            if (EffectVolumeSlider != null) EffectVolumeSlider.onValueChanged.AddListener(OnEffectVolumeChanged);
            if (MusicVolumeSlider != null) MusicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            if (LanguageDropdown != null) LanguageDropdown.onValueChanged.AddListener(OnLanguageChanged);
            if (TimeSpeedDropdown != null) TimeSpeedDropdown.onValueChanged.AddListener(OnTimeSpeedChanged);
            if (AutomaticSaveSlider != null) AutomaticSaveSlider.onValueChanged.AddListener(OnAutoSaveChanged);
            if (NotificationSlider != null) NotificationSlider.onValueChanged.AddListener(OnNotificationChanged);

            LoadSettings();
        }

        private void OnEnable()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            _isInitializing = true;

            // Effect Volume
            if (EffectVolumeSlider != null)
            {
                float ev = PlayerPrefs.GetFloat("Prefs_EffectVolume", 1f);
                EffectVolumeSlider.value = ev;
                UpdateEffectVolumeUI(ev);
            }

            // Music Volume
            if (MusicVolumeSlider != null)
            {
                float mv = PlayerPrefs.GetFloat("Prefs_MusicVolume", 1f);
                MusicVolumeSlider.value = mv;
                UpdateMusicVolumeUI(mv);
            }

            // Language
            if (LanguageDropdown != null)
            {
                int lang = PlayerPrefs.GetInt("Prefs_Language", 0);
                LanguageDropdown.value = lang;
                UpdateLanguageUI(lang);
            }

            // Time Speed
            if (TimeSpeedDropdown != null)
            {
                // Varsayılan hız 1 (Normal)
                int speed = PlayerPrefs.GetInt("Prefs_TimeSpeed", 1);
                TimeSpeedDropdown.value = speed;
                ApplyTimeSpeed(speed);
            }

            // Auto Save
            if (AutomaticSaveSlider != null)
            {
                int autoSave = PlayerPrefs.GetInt("Prefs_AutoSave", 1);
                AutomaticSaveSlider.value = autoSave;
                if (GameManager.Instance != null && GameManager.Instance.SaveManager != null)
                {
                    GameManager.Instance.SaveManager.SetAutoSave(autoSave == 1);
                }
            }

            // Notifications
            if (NotificationSlider != null)
            {
                int notif = PlayerPrefs.GetInt("Prefs_Notifications", 1);
                NotificationSlider.value = notif;
            }

            _isInitializing = false;
        }

        private void OnEffectVolumeChanged(float value)
        {
            if (_isInitializing) return;
            PlayerPrefs.SetFloat("Prefs_EffectVolume", value);
            PlayerPrefs.Save();
            UpdateEffectVolumeUI(value);
        }

        private void UpdateEffectVolumeUI(float value)
        {
            if (EffectVolumePercentText != null)
                EffectVolumePercentText.text = $"{(value * 100):0}%";
        }

        private void OnMusicVolumeChanged(float value)
        {
            if (_isInitializing) return;
            PlayerPrefs.SetFloat("Prefs_MusicVolume", value);
            PlayerPrefs.Save();
            UpdateMusicVolumeUI(value);
        }

        private void UpdateMusicVolumeUI(float value)
        {
            if (MusicVolumePercentText != null)
                MusicVolumePercentText.text = $"{(value * 100):0}%";
        }

        private void OnLanguageChanged(int index)
        {
            if (_isInitializing) return;
            PlayerPrefs.SetInt("Prefs_Language", index);
            PlayerPrefs.Save();
            UpdateLanguageUI(index);
        }

        private void UpdateLanguageUI(int index)
        {
            if (LanguageIcon != null && LanguageDropdown != null && LanguageDropdown.options.Count > index)
            {
                LanguageIcon.sprite = LanguageDropdown.options[index].image;
            }
        }

        private void OnTimeSpeedChanged(int index)
        {
            if (_isInitializing) return;
            PlayerPrefs.SetInt("Prefs_TimeSpeed", index);
            PlayerPrefs.Save();
            ApplyTimeSpeed(index);
        }

        private void ApplyTimeSpeed(int index)
        {
            if (GameManager.Instance != null && GameManager.Instance.TimeManager != null)
            {
                float multiplier = 1f;
                if (index == 0) multiplier = 2.5f;      // Hızlı
                else if (index == 1) multiplier = 1f;   // Normal
                else if (index == 2) multiplier = 0.8f; // Yavaş
                
                GameManager.Instance.TimeManager.SetSpeedMultiplier(multiplier);
            }
        }

        private void OnAutoSaveChanged(float value)
        {
            if (_isInitializing) return;
            int intVal = Mathf.RoundToInt(value);
            PlayerPrefs.SetInt("Prefs_AutoSave", intVal);
            PlayerPrefs.Save();

            if (GameManager.Instance != null && GameManager.Instance.SaveManager != null)
            {
                GameManager.Instance.SaveManager.SetAutoSave(intVal == 1);
            }
        }

        private void OnNotificationChanged(float value)
        {
            if (_isInitializing) return;
            int intVal = Mathf.RoundToInt(value);
            PlayerPrefs.SetInt("Prefs_Notifications", intVal);
            PlayerPrefs.Save();
        }

        public void OnBackClicked()
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
