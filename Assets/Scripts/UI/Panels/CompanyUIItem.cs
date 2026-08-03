using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmpireX.Data;

namespace EmpireX.UI
{
    public class CompanyUIItem : MonoBehaviour
    {
        public Image CompanyLogoImg;
        public TMP_Text CompanyNameText;
        public TMP_Text CompanyTypeText;
        public TMP_Text CompanyTotalRevenueText;
        public TMP_Text CompanyDailyRevenueText;

        public void Setup(CompanyData companyData)
        {
            if (companyData == null) return;
            
            if (CompanyNameText != null) CompanyNameText.text = companyData.Name;
            else Debug.LogError("[CompanyUIItem] CompanyNameText atanmamış!");

            if (CompanyTotalRevenueText != null) CompanyTotalRevenueText.text = $"$ {companyData.Revenue:N0}";
            else Debug.LogError("[CompanyUIItem] CompanyTotalRevenueText atanmamış!");

            if (CompanyDailyRevenueText != null) CompanyDailyRevenueText.text = $"$ {(companyData.Revenue / 30.0):N0} / gün";
            else Debug.LogError("[CompanyUIItem] CompanyDailyRevenueText atanmamış!");

            // Config'den Type'ı bul
            var configs = Resources.LoadAll<CompanyTypeSO>("Configs");
            foreach (var cfg in configs)
            {
                if (cfg.Id == companyData.CompanyTypeId)
                {
                    CompanyTypeText.text = cfg.Category;
                    if (CompanyLogoImg != null) CompanyLogoImg.sprite = cfg.Icon;
                    break;
                }
            }
        }
    }
}
