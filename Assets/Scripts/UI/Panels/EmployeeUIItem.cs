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
        public Image FillStar; // Fill Amount ile kontrol edilecek (Maks 5.0 varsayýlýyor)

        public void Setup(EmployeeData employee)
        {
            EmployeesNameText.text = employee.Name;
            
            // Skill hesabýna göre FillAmount (0.0 - 1.0)
            if (FillStar != null) FillStar.fillAmount = employee.Skill / 5.0f;

            var configs = Resources.LoadAll<EmployeeTypeSO>("EmployeeType");
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
            var configs = Resources.LoadAll<ExecutiveTypeSO>("ExecutiveType");
            foreach (var cfg in configs)
            {
                if (cfg.Id == exec.ExecutiveTypeId)
                {
                    EmployeesNameText.text = "Yönetici"; // Ýsimleri config üzerinden ya da rastgele üretilebilir
                    EmployeesPozitionText.text = cfg.Name;
                    if (EmployeesIcon != null) EmployeesIcon.sprite = cfg.Icon;
                    // Yöneticilerin skilli varsayýlan olarak 5 kabul edilsin veya 0 kalsýn
                    if (FillStar != null) FillStar.fillAmount = 1.0f; 
                    break;
                }
            }
        }
    }
}
