using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmpireX.Core;
using EmpireX.Data;

namespace EmpireX.UI
{
    public class EmployeesDetailPanel : BasePanel
    {
        [Header("Profile Info")]
        public Image EmployeePictureImg;
        public TMP_Text EmployeeNameText;
        public TMP_Text EmployeeAgeText;
        public TMP_Text EmployeePositionText;
        public TMP_Text EmployeeSalaryText;
        public TMP_Text EmployeeProductivityText;
        public TMP_Text EmployeeExperienceText;
        public TMP_Text EmployeePreviouseCompanyText;
        public TMP_Text EmployeeWaitingDaysCountText;

        [Header("Skills")]
        public TMP_Text ProgrammingValueText;
        public TMP_Text ResearchValueText;
        public TMP_Text DesignValueText;
        public TMP_Text ManagementValueText;
        public TMP_Text SalesValueText;
        public TMP_Text FinanceValueText;
        public TMP_Text ManufacturingValueText;
        public TMP_Text LogisticsValueText;
        public TMP_Text HealthcareValueText;

        [Header("Buttons")]
        public Button HireButon;
        public Button RejectButon;

        private CandidateData _currentCandidate;
        private EmployeesPanel _parentPanel;

        private void Start()
        {
            if (HireButon != null) HireButon.onClick.AddListener(OnHireClicked);
            if (RejectButon != null) RejectButon.onClick.AddListener(OnRejectClicked);
        }

        public void ShowCandidate(CandidateData candidate, EmployeesPanel parentPanel)
        {
            _currentCandidate = candidate;
            _parentPanel = parentPanel;

            // Doldur
            if (EmployeeNameText != null) EmployeeNameText.text = candidate.Name;
            if (EmployeeAgeText != null) EmployeeAgeText.text = candidate.Age.ToString();
            if (EmployeeSalaryText != null) EmployeeSalaryText.text = candidate.ExpectedSalary.ToString("C0");
            if (EmployeeProductivityText != null) EmployeeProductivityText.text = candidate.BaseProductivity.ToString("F1");
            if (EmployeeExperienceText != null) EmployeeExperienceText.text = candidate.Experience.ToString("F0");
            if (EmployeePreviouseCompanyText != null) EmployeePreviouseCompanyText.text = string.IsNullOrEmpty(candidate.PreviousCompany) ? "None" : candidate.PreviousCompany;
            if (EmployeeWaitingDaysCountText != null) EmployeeWaitingDaysCountText.text = $"{candidate.WaitDays} / {candidate.MaxWaitDays} Days";

            // Pozisyon ve Portre
            if (candidate.IsExecutive)
            {
                var configs = Resources.LoadAll<ExecutiveTypeSO>("ExecutiveType");
                foreach (var cfg in configs)
                {
                    if (cfg.Id == candidate.TypeId)
                    {
                        if (EmployeePositionText != null) EmployeePositionText.text = cfg.Name;
                        break;
                    }
                }
                
                if (EmployeePictureImg != null && !string.IsNullOrEmpty(candidate.PortraitPath))
                {
                    EmployeePictureImg.sprite = Resources.Load<Sprite>("ExecutivePortraits/" + candidate.PortraitPath);
                }
                
                // Yöneticilerin spesifik yetenek puanlar yerine bonuslar olabilir, o yüzden yetenekler sfırlanabilir
                ClearSkills();
            }
            else
            {
                var configs = Resources.LoadAll<EmployeeTypeSO>("EmployeeType");
                foreach (var cfg in configs)
                {
                    if (cfg.Id == candidate.TypeId)
                    {
                        if (EmployeePositionText != null) EmployeePositionText.text = cfg.Name;
                        break;
                    }
                }
                
                if (EmployeePictureImg != null && !string.IsNullOrEmpty(candidate.PortraitPath))
                {
                    EmployeePictureImg.sprite = Resources.Load<Sprite>("EmployeePortraits/" + candidate.PortraitPath);
                }

                UpdateSkills(candidate);
            }

            base.ShowImmediate();
        }

        private void UpdateSkills(CandidateData candidate)
        {
            ClearSkills(); // nce hepsini 0/bo yap

            foreach (var sk in candidate.Skills)
            {
                string val = sk.Value.ToString();
                switch (sk.SkillType)
                {
                    case EmployeeSkill.Programming: if (ProgrammingValueText != null) ProgrammingValueText.text = val; break;
                    case EmployeeSkill.Research: if (ResearchValueText != null) ResearchValueText.text = val; break;
                    case EmployeeSkill.Design: if (DesignValueText != null) DesignValueText.text = val; break;
                    case EmployeeSkill.Management: if (ManagementValueText != null) ManagementValueText.text = val; break;
                    case EmployeeSkill.Sales: if (SalesValueText != null) SalesValueText.text = val; break;
                    case EmployeeSkill.Finance: if (FinanceValueText != null) FinanceValueText.text = val; break;
                    case EmployeeSkill.Manufacturing: if (ManufacturingValueText != null) ManufacturingValueText.text = val; break;
                    case EmployeeSkill.Logistics: if (LogisticsValueText != null) LogisticsValueText.text = val; break;
                    case EmployeeSkill.Healthcare: if (HealthcareValueText != null) HealthcareValueText.text = val; break;
                }
            }
        }

        private void ClearSkills()
        {
            if (ProgrammingValueText != null) ProgrammingValueText.text = "0";
            if (ResearchValueText != null) ResearchValueText.text = "0";
            if (DesignValueText != null) DesignValueText.text = "0";
            if (ManagementValueText != null) ManagementValueText.text = "0";
            if (SalesValueText != null) SalesValueText.text = "0";
            if (FinanceValueText != null) FinanceValueText.text = "0";
            if (ManufacturingValueText != null) ManufacturingValueText.text = "0";
            if (LogisticsValueText != null) LogisticsValueText.text = "0";
            if (HealthcareValueText != null) HealthcareValueText.text = "0";
        }

        private void OnHireClicked()
        {
            if (_currentCandidate == null) return;
            
            // Oyuncunun mevcut ektii irketi bulmamz lzm. ui state iinden alnabilir.
            // imdilik test amal Player'n ilk irketi alnr veya GameManager'da seili irket kullanlabilir.
            string companyId = GameManager.Instance.SaveManager.CurrentData.HoldingData.CompanyIds.Count > 0 
                ? GameManager.Instance.SaveManager.CurrentData.HoldingData.CompanyIds[0] 
                : "";

            if (string.IsNullOrEmpty(companyId))
            {
                Debug.LogWarning("Oyuncunun irketi yok!");
                return;
            }

            if (_currentCandidate.IsExecutive)
            {
                GameManager.Instance.ExecutiveManager.HireExecutive(companyId, _currentCandidate.Id);
            }
            else
            {
                GameManager.Instance.EmployeeManager.HireEmployee(companyId, _currentCandidate.Id);
            }

            _parentPanel.RefreshCurrentList();
            Hide();
        }

        private void OnRejectClicked()
        {
            if (_currentCandidate == null) return;
            
            GameManager.Instance.HRManager.RemoveCandidate(_currentCandidate.Id);
            _parentPanel.RefreshCurrentList();
            Hide();
        }
    }
}
