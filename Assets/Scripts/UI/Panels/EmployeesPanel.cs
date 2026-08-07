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

        [Header("Selection Buttons")]
        public Button ExecutiveSelectionBtn;
        public Button EmployeeSelectionBtn;

        [Header("List Settings")]
        public Transform EmployeesScrollviewContent;
        public GameObject EmployeesBtnPrefab;

        [Header("Detail Panel")]
        public EmployeesDetailPanel DetailPanel; // Inspector'dan atanacak

        private List<GameObject> _spawnedItems = new List<GameObject>();
        private int _currentFilterMode = 0; // 0: Employees, 1: Executives

        private void Start()
        {
            if (BackBtn != null) BackBtn.onClick.AddListener(OnBackClicked);
            
            if (EmployeeSelectionBtn != null) EmployeeSelectionBtn.onClick.AddListener(() => PopulateList(0));
            if (ExecutiveSelectionBtn != null) ExecutiveSelectionBtn.onClick.AddListener(() => PopulateList(1));
        }

        public override void Show()
        {
            base.Show();
            PopulateList(0); // Default: Employees
        }
        
        public override void ShowImmediate()
        {
            base.ShowImmediate();
            PopulateList(0);
        }

        public void RefreshCurrentList()
        {
            PopulateList(_currentFilterMode);
        }

        private void PopulateList(int filterMode)
        {
            _currentFilterMode = filterMode;

            foreach (var item in _spawnedItems)
            {
                Destroy(item);
            }
            _spawnedItems.Clear();

            if (EmployeesScrollviewContent == null || EmployeesBtnPrefab == null) return;
            if (GameManager.Instance == null || GameManager.Instance.HRManager == null) return;

            var hrManager = GameManager.Instance.HRManager;
            var saveManager = GameManager.Instance.SaveManager;

            if (saveManager == null || saveManager.CurrentData == null) return;

            // Employees (0)
            if (filterMode == 0)
            {
                var candidates = saveManager.CurrentData.EmployeeCandidates;
                if (candidates != null)
                {
                    foreach (var cand in candidates)
                    {
                        var go = Instantiate(EmployeesBtnPrefab, EmployeesScrollviewContent);
                        var uiItem = go.GetComponent<EmployeeUIItem>();
                        if (uiItem != null) uiItem.Setup(cand, OnCandidateClicked);
                        _spawnedItems.Add(go);
                    }
                }
            }
            // Executives (1)
            else if (filterMode == 1)
            {
                var candidates = saveManager.CurrentData.ExecutiveCandidates;
                if (candidates != null)
                {
                    foreach (var cand in candidates)
                    {
                        var go = Instantiate(EmployeesBtnPrefab, EmployeesScrollviewContent);
                        var uiItem = go.GetComponent<EmployeeUIItem>();
                        if (uiItem != null) uiItem.Setup(cand, OnCandidateClicked);
                        _spawnedItems.Add(go);
                    }
                }
            }
        }

        private void OnCandidateClicked(CandidateData candidate)
        {
            if (DetailPanel != null)
            {
                DetailPanel.ShowCandidate(candidate, this);
            }
            else
            {
                Debug.LogWarning("DetailPanel is not assigned in EmployeesPanel!");
            }
        }

        private void OnBackClicked()
        {
            if (UINavigation.Instance != null) UINavigation.Instance.GoBack();
            else Hide();
        }
    }
}
