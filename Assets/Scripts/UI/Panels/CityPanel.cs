using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using EmpireX.Core;
using EmpireX.Data;

namespace EmpireX.UI
{
    public class CityPanel : BasePanel
    {
        [Header("Common References")]
        public Button BackBtn;

        [Header("List Settings")]
        public Transform CityScrollviewContent;
        public GameObject CityPrefab;

        private List<GameObject> _spawnedItems = new List<GameObject>();

        private void Awake()
        {
            Debug.Log("[CityPanel] LoadingScreen sırasında prefablar yükleniyor ve dolduruluyor...");
            PopulateList();
            Debug.Log("[CityPanel] Prefablar başarıyla oluşturuldu.");
        }

        private void Start()
        {
            if (BackBtn != null) BackBtn.onClick.AddListener(OnBackClicked);
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
            foreach (var item in _spawnedItems) Destroy(item);
            _spawnedItems.Clear();

            if (CityScrollviewContent == null || CityPrefab == null) return;
            if (GameManager.Instance == null || GameManager.Instance.SaveManager == null) return;
            
            var runtimeData = GameManager.Instance.SaveManager.CurrentData;
            if (runtimeData == null || runtimeData.HoldingData == null) return;

            var allCities = Resources.LoadAll<CitySO>("Configs");

            foreach (var citySo in allCities)
            {
                var go = Instantiate(CityPrefab, CityScrollviewContent);
                var uiItem = go.GetComponent<CityUIItem>();
                if (uiItem != null)
                {
                    bool isUnlocked = runtimeData.HoldingData.CityIds.Contains(citySo.Id);
                    
                    // Bu şehirdeki şirket ve şube sayısını bul
                    int corpCount = 0;
                    if (runtimeData.Companies != null)
                        corpCount += runtimeData.Companies.Count(c => c.CityId == citySo.Id);
                    if (runtimeData.Branches != null)
                        corpCount += runtimeData.Branches.Count(b => b.CityId == citySo.Id);

                    uiItem.Setup(citySo, isUnlocked, corpCount);
                }
                _spawnedItems.Add(go);
            }
        }

        private void OnBackClicked()
        {
            if (UINavigation.Instance != null) UINavigation.Instance.GoBack();
            else Hide();
        }
    }
}
