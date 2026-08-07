using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmpireX.Data;
using System;

namespace EmpireX.UI
{
    public class EmployeeUIItem : MonoBehaviour
    {
        public Image EmployeesIcon;
        public TMP_Text EmployeesNameTxt;
        public TMP_Text EmployeesPozitionTxt;
        public TMP_Text EmployeesExperienceText;
        public TMP_Text EmployeesSalaryText;
        public Button ItemButton;

        private CandidateData _currentCandidate;
        private Action<CandidateData> _onClickCallback;

        private void Start()
        {
            if (ItemButton != null)
            {
                ItemButton.onClick.AddListener(OnItemClicked);
            }
            else
            {
                var btn = GetComponent<Button>();
                if (btn != null) btn.onClick.AddListener(OnItemClicked);
            }
        }

        public void Setup(CandidateData candidate, Action<CandidateData> onClickCallback)
        {
            _currentCandidate = candidate;
            _onClickCallback = onClickCallback;

            if (EmployeesNameTxt != null) EmployeesNameTxt.text = candidate.Name;
            if (EmployeesExperienceText != null) EmployeesExperienceText.text = candidate.Experience.ToString("F0") + " Exp";
            if (EmployeesSalaryText != null) EmployeesSalaryText.text = candidate.ExpectedSalary.ToString("C0");

            // Pozisyon ismini bul
            if (candidate.IsExecutive)
            {
                var configs = Resources.LoadAll<ExecutiveTypeSO>("ExecutiveType");
                foreach (var cfg in configs)
                {
                    if (cfg.Id == candidate.TypeId)
                    {
                        if (EmployeesPozitionTxt != null) EmployeesPozitionTxt.text = cfg.Name;
                        break;
                    }
                }
            }
            else
            {
                var configs = Resources.LoadAll<EmployeeTypeSO>("EmployeeType");
                foreach (var cfg in configs)
                {
                    if (cfg.Id == candidate.TypeId)
                    {
                        if (EmployeesPozitionTxt != null) EmployeesPozitionTxt.text = cfg.Name;
                        break;
                    }
                }
            }

            // Portre ykle
            if (EmployeesIcon != null && !string.IsNullOrEmpty(candidate.PortraitPath))
            {
                string folder = candidate.IsExecutive ? "ExecutivePortraits" : "EmployeePortraits";
                Sprite loadedSprite = Resources.Load<Sprite>(folder + "/" + candidate.PortraitPath);
                if (loadedSprite != null)
                {
                    EmployeesIcon.sprite = loadedSprite;
                }
            }
        }

        private void OnItemClicked()
        {
            _onClickCallback?.Invoke(_currentCandidate);
        }
    }
}
