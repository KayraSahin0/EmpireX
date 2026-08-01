using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using EmpireX.Core;
using EmpireX.Data;

namespace EmpireX.UI
{
    public class CompanyPanel : BasePanel
    {
        [Header("Common References")]
        public Button BackBtn;

        [Header("Company List Settings")]
        public Transform CompanyScrollviewContent;
        public GameObject CompanyPrefab;

        private List<GameObject> _spawnedItems = new List<GameObject>();

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
            PopulateList();
        }

        public override void ShowImmediate()
        {
            base.ShowImmediate();
            PopulateList();
        }

        private void PopulateList()
        {
            foreach (var item in _spawnedItems)
            {
                Destroy(item);
            }
            _spawnedItems.Clear();

            if (CompanyScrollviewContent == null || CompanyPrefab == null) return;

            if (GameManager.Instance == null || GameManager.Instance.SaveManager == null) return;
            var runtimeData = GameManager.Instance.SaveManager.CurrentData;
            if (runtimeData == null || runtimeData.Companies == null) return;

            foreach (var company in runtimeData.Companies)
            {
                var go = Instantiate(CompanyPrefab, CompanyScrollviewContent);
                var uiItem = go.GetComponent<CompanyUIItem>();
                if (uiItem != null)
                {
                    uiItem.Setup(company);
                }
                _spawnedItems.Add(go);
            }
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
