using UnityEngine;
using TMPro;
using EmpireX.Events;
using EmpireX.Data;

namespace EmpireX.UI
{
    public class TopHudUI : MonoBehaviour
    {
        [Header("UI References")]
        public TMP_Text TotalMoneyText;
        public TMP_Text DailyEarningsText;
        public TMP_Text ResValueText;
        public TMP_Text EmployeesText;
        public TMP_Text RatingText;

        private void Start()
        {
            if (EmpireX.Core.GameManager.Instance != null && EmpireX.Core.GameManager.Instance.EventManager != null)
            {
                EmpireX.Core.GameManager.Instance.EventManager.EventBus.Subscribe<TickStarted>(OnTickStarted);
                EmpireX.Core.GameManager.Instance.EventManager.EventBus.Subscribe<HoldingStatsUpdated>(OnHoldingStatsUpdated);
                
                UpdateHud();
            }
            else
            {
                Debug.LogWarning("[TopHudUI] GameManager veya EventManager bulunamadı!");
            }
        }

        private void OnDestroy()
        {
            if (EmpireX.Core.GameManager.Instance != null && EmpireX.Core.GameManager.Instance.EventManager != null)
            {
                EmpireX.Core.GameManager.Instance.EventManager.EventBus.Unsubscribe<TickStarted>(OnTickStarted);
                EmpireX.Core.GameManager.Instance.EventManager.EventBus.Unsubscribe<HoldingStatsUpdated>(OnHoldingStatsUpdated);
            }
        }

        private void OnTickStarted(TickStarted e)
        {
            // Para gibi dinamik verilerin saniyede bir güncellenmesi için
            UpdateHud();
        }

        private void OnHoldingStatsUpdated(HoldingStatsUpdated e)
        {
            UpdateHud();
        }

        private void UpdateHud()
        {
            if (EmpireX.Core.GameManager.Instance == null || EmpireX.Core.GameManager.Instance.SaveManager == null) return;
            var runtimeData = EmpireX.Core.GameManager.Instance.SaveManager.CurrentData;
            if (runtimeData == null || runtimeData.HoldingData == null) return;

            var holding = runtimeData.HoldingData;

            if (TotalMoneyText != null) 
            {
                TotalMoneyText.text = $"${holding.Cash:N0}";
            }

            if (DailyEarningsText != null)
            {
                // Aylık toplam kârı (Profit) 30'a bölerek günlük kazancı hesaplıyoruz
                double dailyEarnings = holding.TotalProfit / 30.0;
                string sign = dailyEarnings >= 0 ? "+" : "";
                DailyEarningsText.text = $"{sign}${dailyEarnings:N0} / gün";
            }

            if (ResValueText != null)
            {
                ResValueText.text = holding.ResearchPoints.ToString("N0");
            }

            if (EmployeesText != null)
            {
                EmployeesText.text = holding.TotalEmployees.ToString("N0");
            }

            if (RatingText != null)
            {
                float rating = CalculateRating(runtimeData);
                RatingText.text = $"{rating:F1}/5";
            }
        }

        /// <summary>
        /// Sahip olunan tüm şirketlerin 'Brand' (Marka Değeri) ortalamasına göre 5 üzerinden puan verir.
        /// Eğer şirket yoksa varsayılan puan verir (Örn: 1.0).
        /// </summary>
        private float CalculateRating(SaveData data)
        {
            if (data.Companies == null || data.Companies.Count == 0) return 1.0f; // Hiç şirketi yoksa başlangıç puanı
            
            double totalBrand = 0;
            foreach (var comp in data.Companies)
            {
                totalBrand += comp.Brand;
            }
            
            // Brand değerinin 0 ile 100 arasında olduğunu varsayarak 20'ye bölüyoruz (100/20 = 5.0)
            double avgBrand = totalBrand / data.Companies.Count;
            float rating = (float)(avgBrand / 20.0);
            
            // Sıfırın altına inmesin, 5'in üstüne çıkmasın
            return Mathf.Clamp(rating, 1f, 5f);
        }
    }
}
