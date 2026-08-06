using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using EmpireX.Core;
using EmpireX.Data;

namespace EmpireX.UI
{
    public class CountryPanel : BasePanel
    {
        [Header("Common References")]
        public Button BackBtn;

        [Header("List Settings")]
        public Transform CountryScrollviewContent;
        public GameObject CountryPrefab;

        private List<GameObject> _spawnedItems = new List<GameObject>();

        private void Awake()
        {
            Debug.Log("[CountryPanel] LoadingScreen sırasında prefablar yükleniyor ve dolduruluyor...");
            PopulateList();
            Debug.Log("[CountryPanel] Prefablar başarıyla oluşturuldu.");
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

            if (CountryScrollviewContent == null || CountryPrefab == null) return;
            if (GameManager.Instance == null || GameManager.Instance.SaveManager == null) return;
            
            var runtimeData = GameManager.Instance.SaveManager.CurrentData;
            if (runtimeData == null || runtimeData.HoldingData == null) return;

            var allCountries = Resources.LoadAll<CountrySO>("Country");
            var allCities = Resources.LoadAll<CitySO>("City");

            foreach (var countrySo in allCountries)
            {
                var go = Instantiate(CountryPrefab, CountryScrollviewContent);
                var uiItem = go.GetComponent<CountryUIItem>();
                if (uiItem != null)
                {
                    bool isUnlocked = runtimeData.HoldingData.CountryIds.Contains(countrySo.Id);
                    
                    // Bu ülkedeki AÇILMIŞ şehir sayısını bul (HoldingData.CityIds içinde olan ve CountryId eşleşen)
                    int openedCitiesCount = allCities.Count(c => c.CountryId == countrySo.Id && runtimeData.HoldingData.CityIds.Contains(c.Id));

                    uiItem.Setup(countrySo, isUnlocked, openedCitiesCount);
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
