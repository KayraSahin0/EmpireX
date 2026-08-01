using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmpireX.Data;

namespace EmpireX.UI
{
    public class EmployeeUIItem : MonoBehaviour
    {
        public Image EmployeesIcon;
        public TMP_Text EmployeesNameText;
        public TMP_Text EmployeesPozitionText;
        public Image FillStar; // Fill Amount ile kontrol edilecek (Maks 5.0 varsayılıyor)

        public void Setup(EmployeeData employee)
        {
            EmployeesNameText.text = employee.Name;
            
            // Skill hesabına göre FillAmount (0.0 - 1.0)
            if (FillStar != null) FillStar.fillAmount = employee.Skill / 5.0f;

            var configs = Resources.LoadAll<EmployeeTypeSO>("Configs");
            foreach (var cfg in configs)
            {
                if (cfg.Id == employee.EmployeeTypeId)
                {
                    EmployeesPozitionText.text = cfg.Name;
                    if (EmployeesIcon != null) EmployeesIcon.sprite = cfg.Icon;
                    break;
                }
            }
        }

        public void SetupExecutive(ExecutiveData exec)
        {
            var configs = Resources.LoadAll<ExecutiveTypeSO>("Configs");
            foreach (var cfg in configs)
            {
                if (cfg.Id == exec.ExecutiveTypeId)
                {
                    EmployeesNameText.text = "Yönetici"; // İsimleri config üzerinden ya da rastgele üretilebilir
                    EmployeesPozitionText.text = cfg.Name;
                    if (EmployeesIcon != null) EmployeesIcon.sprite = cfg.Icon;
                    // Yöneticilerin skilli varsayılan olarak 5 kabul edilsin veya 0 kalsın
                    if (FillStar != null) FillStar.fillAmount = 1.0f; 
                    break;
                }
            }
        }
    }
}
