using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmpireX.Core;

namespace EmpireX.UI
{
    public class HoldingPanel : BasePanel
    {
        [Header("Common References")]
        public Button BackBtn;

        [Header("Holding Details")]
        public TMP_Text HoldingNameText;
        public TMP_Text HoldingValueText;
        public TMP_Text TotalBranchText;
        public TMP_Text TotalEmployeesText;
        public TMP_Text TotalCompanyText;
        public TMP_Text TotalCountryText;

        private void Start()
        {
            if (BackBtn != null)
            {
                BackBtn.onClick.AddListener(OnBackClicked);
            }
        }

        public override void Show()
        {
            base.Show();
            UpdateDetails();
        }
        
        public override void ShowImmediate()
        {
            base.ShowImmediate();
            UpdateDetails();
        }

        private void UpdateDetails()
        {
            if (GameManager.Instance == null || GameManager.Instance.SaveManager == null) return;
            var runtimeData = GameManager.Instance.SaveManager.CurrentData;
            if (runtimeData == null || runtimeData.HoldingData == null) return;

            var holding = runtimeData.HoldingData;

            if (HoldingNameText != null) HoldingNameText.text = holding.Name;
            if (HoldingValueText != null) HoldingValueText.text = $"$ {holding.Value:N0}";
            
            if (TotalBranchText != null) TotalBranchText.text = runtimeData.Branches?.Count.ToString() ?? "0";
            
            if (TotalEmployeesText != null) 
            {
                int totalEmployees = (runtimeData.Employees?.Count ?? 0) + (runtimeData.Executives?.Count ?? 0);
                TotalEmployeesText.text = totalEmployees.ToString();
            }

            if (TotalCompanyText != null) TotalCompanyText.text = runtimeData.Companies?.Count.ToString() ?? "0";
            if (TotalCountryText != null) TotalCountryText.text = holding.CountryIds?.Count.ToString() ?? "0";
        }

        private void OnBackClicked()
        {
            if (UINavigation.Instance != null)
                UINavigation.Instance.GoBack();
            else
                Hide();
        }
    }
}
