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
            
            CompanyNameText.text = companyData.Name;
            CompanyTotalRevenueText.text = $"$ {companyData.Revenue:N0}";
            CompanyDailyRevenueText.text = $"$ {(companyData.Revenue / 30.0):N0} / gün";

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
