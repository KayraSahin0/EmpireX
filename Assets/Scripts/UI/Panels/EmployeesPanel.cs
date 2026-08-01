using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using EmpireX.Core;
using EmpireX.Data;

namespace EmpireX.UI
{
    public class EmployeesPanel : BasePanel
    {
        [Header("Common References")]
        public Button BackBtn;

        [Header("Tab Buttons")]
        public Button AllBtn;
        public Button AdministratorsBtn;
        public Button DepartmentBtn;

        [Header("List Settings")]
        public Transform EmployeesScrollviewContent;
        public GameObject EmployeesBtnPrefab;

        private List<GameObject> _spawnedItems = new List<GameObject>();

        private void Start()
        {
            if (BackBtn != null) BackBtn.onClick.AddListener(OnBackClicked);
            
            if (AllBtn != null) AllBtn.onClick.AddListener(() => PopulateList(0));
            if (AdministratorsBtn != null) AdministratorsBtn.onClick.AddListener(() => PopulateList(1));
            if (DepartmentBtn != null) DepartmentBtn.onClick.AddListener(() => PopulateList(2));
        }

        public override void Show()
        {
            base.Show();
            PopulateList(0); // Default: All
        }
        
        public override void ShowImmediate()
        {
            base.ShowImmediate();
            PopulateList(0); // Default: All
        }

        private void PopulateList(int filterMode)
        {
            // filterMode: 0=All, 1=Administrators, 2=Department

            foreach (var item in _spawnedItems)
            {
                Destroy(item);
            }
            _spawnedItems.Clear();

            if (EmployeesScrollviewContent == null || EmployeesBtnPrefab == null) return;
            if (GameManager.Instance == null || GameManager.Instance.SaveManager == null) return;
            
            var runtimeData = GameManager.Instance.SaveManager.CurrentData;
            if (runtimeData == null) return;

            // Administrators (Yöneticiler)
            if (filterMode == 0 || filterMode == 1)
            {
                if (runtimeData.Executives != null)
                {
                    foreach (var exec in runtimeData.Executives)
                    {
                        var go = Instantiate(EmployeesBtnPrefab, EmployeesScrollviewContent);
                        var uiItem = go.GetComponent<EmployeeUIItem>();
                        if (uiItem != null) uiItem.SetupExecutive(exec);
                        _spawnedItems.Add(go);
                    }
                }
            }

            // Normal Çalışanlar
            if (filterMode == 0 || filterMode == 2)
            {
                if (runtimeData.Employees != null)
                {
                    foreach (var emp in runtimeData.Employees)
                    {
                        var go = Instantiate(EmployeesBtnPrefab, EmployeesScrollviewContent);
                        var uiItem = go.GetComponent<EmployeeUIItem>();
                        if (uiItem != null) uiItem.Setup(emp);
                        _spawnedItems.Add(go);
                    }
                }
            }
        }

        private void OnBackClicked()
        {
            if (UINavigation.Instance != null) UINavigation.Instance.GoBack();
            else Hide();
        }
    }
}
