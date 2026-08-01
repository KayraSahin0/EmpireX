using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmpireX.Events;
using EmpireX.Data;

namespace EmpireX.UI
{
    public class TimeCardUI : MonoBehaviour
    {
        [Header("UI References")]
        public Image TimeIcon;
        public TMP_Text HoursText;
        public TMP_Text DayCounterText;
        public TMP_Text MounthCountText; // Kullanıcının yazdığı isme sadık kalındı
        public TMP_Text YearCountText;
        
        [Header("Config")]
        public TimeIconConfig IconConfig; // Unity Editör'den veya ConfigSystem'den atanacak

        private void Start()
        {
            if (EmpireX.Core.GameManager.Instance != null && EmpireX.Core.GameManager.Instance.EventManager != null)
            {
                EmpireX.Core.GameManager.Instance.EventManager.EventBus.Subscribe<TickStarted>(OnTickStarted);
                
                // Başlangıç değerlerini çek
                UpdateTimeCard();
            }
            else
            {
                Debug.LogWarning("[TimeCardUI] GameManager veya EventManager bulunamadı!");
            }
            
            // Referans kontrolleri
            if (HoursText == null) Debug.LogWarning("[TimeCardUI] HoursText referansı eksik!");
            if (DayCounterText == null) Debug.LogWarning("[TimeCardUI] DayCounterText referansı eksik!");
            if (MounthCountText == null) Debug.LogWarning("[TimeCardUI] MounthCountText referansı eksik!");
            if (YearCountText == null) Debug.LogWarning("[TimeCardUI] YearCountText referansı eksik!");
            if (IconConfig == null) Debug.LogWarning("[TimeCardUI] IconConfig referansı eksik!");
        }

        private void OnDestroy()
        {
            if (EmpireX.Core.GameManager.Instance != null && EmpireX.Core.GameManager.Instance.EventManager != null)
            {
                EmpireX.Core.GameManager.Instance.EventManager.EventBus.Unsubscribe<TickStarted>(OnTickStarted);
            }
        }

        private void OnTickStarted(TickStarted e)
        {
            UpdateTimeCard();
        }

        private void UpdateTimeCard()
        {
            if (EmpireX.Core.GameManager.Instance == null || EmpireX.Core.GameManager.Instance.TimeManager == null) return;
            
            var timeData = EmpireX.Core.GameManager.Instance.TimeManager.CurrentTime;
            if (timeData == null) 
            {
                Debug.LogWarning("[TimeCardUI] TimeData şu an boş! Oyun henüz başlatılmamış (GameStarted tetiklenmemiş) olabilir.");
                return;
            }

            // Metinleri Güncelle
            if (DayCounterText != null) DayCounterText.text = timeData.Day.ToString();
            if (MounthCountText != null) MounthCountText.text = timeData.Month.ToString();
            if (YearCountText != null) YearCountText.text = timeData.Year.ToString();

            int hour = (int)(timeData.Tick % 24);
            if (HoursText != null) HoursText.text = $"{hour:D2}:00"; // "09:00", "14:00" formatında

            // İkonu Güncelle
            UpdateTimeIcon(hour);
        }

        private void UpdateTimeIcon(int hour)
        {
            if (IconConfig == null || TimeIcon == null) return;

            if (hour >= 0 && hour < 4)
                TimeIcon.sprite = IconConfig.MidnightIcon;
            else if (hour >= 4 && hour < 8)
                TimeIcon.sprite = IconConfig.SunriseIcon;
            else if (hour >= 8 && hour < 12)
                TimeIcon.sprite = IconConfig.SmallSunIcon;
            else if (hour >= 12 && hour < 16)
                TimeIcon.sprite = IconConfig.SunIcon;
            else if (hour >= 16 && hour < 20)
                TimeIcon.sprite = IconConfig.SunsetIcon;
            else // 20 - 24
                TimeIcon.sprite = IconConfig.NightIcon;
        }
    }
}
