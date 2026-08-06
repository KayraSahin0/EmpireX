using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
        public Button NewCompanyBtn;

        [Header("New Company Container")]
        public GameObject NewCompanyContainer;
        public TMP_InputField NewCompanyIF;
        public TMP_Dropdown NewCompanyCategoryDropdown;

        private List<GameObject> _spawnedItems = new List<GameObject>();
        private List<CompanyTypeSO> _availableTypes = new List<CompanyTypeSO>();

        private void Start()
        {
            if (BackBtn != null)
            {
                BackBtn.onClick.AddListener(OnBackClicked);
            }
            
            if (NewCompanyBtn != null)
            {
                NewCompanyBtn.onClick.AddListener(OnNewCompanyBtnClicked);
            }

            if (NewCompanyContainer != null)
            {
                NewCompanyContainer.SetActive(false);
            }
        }

        public override void Show()
        {
            base.Show();
            
            if (NewCompanyContainer != null)
            {
                NewCompanyContainer.SetActive(false);
            }
            PopulateList();
        }

        public override void ShowImmediate()
        {
            base.ShowImmediate();
            
            if (NewCompanyContainer != null)
            {
                NewCompanyContainer.SetActive(false);
            }
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

        private void OnNewCompanyBtnClicked()
        {
            if (NewCompanyContainer == null) return;

            if (!NewCompanyContainer.activeSelf)
            {
                // Paneli Aç ve Dropdown'ý Doldur
                OpenNewCompanyContainer();
            }
            else
            {
                // Panel açýksa Onayla / Submit
                SubmitNewCompany();
            }
        }

        private void OpenNewCompanyContainer()
        {
            NewCompanyContainer.SetActive(true);
            
            if (NewCompanyIF != null)
            {
                NewCompanyIF.text = "";
            }

            if (NewCompanyCategoryDropdown != null)
            {
                NewCompanyCategoryDropdown.ClearOptions();
                _availableTypes.Clear();
                
                var allTypes = Resources.LoadAll<CompanyTypeSO>("CompanyType");
                var options = new List<string>();

                // TODO: Ýleride araþtýrmalarla açýlanlar filtrelenecek.
                // Þimdilik hepsi ekleniyor.
                foreach (var t in allTypes)
                {
                    _availableTypes.Add(t);
                    options.Add(t.Category + $" ()");
                }

                if (options.Count == 0)
                {
                    options.Add("Kategori Bulunamadý");
                    NewCompanyCategoryDropdown.interactable = false;
                }
                else
                {
                    NewCompanyCategoryDropdown.interactable = true;
                }

                NewCompanyCategoryDropdown.AddOptions(options);
                NewCompanyCategoryDropdown.value = 0;
            }
        }

        private void SubmitNewCompany()
        {
            if (GameManager.Instance == null || GameManager.Instance.SaveManager == null) return;
            
            var runtimeData = GameManager.Instance.SaveManager.CurrentData;
            if (runtimeData == null || runtimeData.HoldingData == null) return;

            if (NewCompanyIF == null || string.IsNullOrWhiteSpace(NewCompanyIF.text))
            {
                Debug.LogWarning("[CompanyPanel] Lütfen þirket adý giriniz.");
                return;
            }

            if (_availableTypes.Count == 0 || NewCompanyCategoryDropdown == null)
            {
                return;
            }

            string companyName = NewCompanyIF.text;
            var companyType = _availableTypes[NewCompanyCategoryDropdown.value];
            double cost = companyType.BaseCost;
            
            if (runtimeData.HoldingData.Cash < cost)
            {
                // Yetersiz Bakiye
                GameManager.Instance.EventManager.EventBus.Publish(new EmpireX.Events.ShowSystemPopupEvent
                {
                    Title = "Yetersiz Bakiye",
                    Message = $"Bu þirketi kurmak için yeterli paranýz yok!\nGereken: ${cost:N0}\nMevcut: ${runtimeData.HoldingData.Cash:N0}",
                    Button1Text = "Tamam",
                    Button1Callback = null
                });
            }
            else
            {
                // Emin misiniz?
                GameManager.Instance.EventManager.EventBus.Publish(new EmpireX.Events.ShowSystemPopupEvent
                {
                    Title = "Þirket Kurulumu",
                    Message = $"{companyName} adýnda bir {companyType.Category} þirketini ${cost:N0} karþýlýðýnda kurmak istediðinize emin misiniz?",
                    Button1Text = "Evet",
                    Button1Callback = () => CreateCompany(companyType, companyName),
                    Button2Text = "Hayýr",
                    Button2Callback = null
                });
            }
        }

        private void CreateCompany(CompanyTypeSO companyType, string companyName)
        {
            var runtimeData = GameManager.Instance.SaveManager.CurrentData;
            
            // Ýlk açýk þehri bul (veya Config'den ilk þehri al)
            string targetCityId = "";
            if (runtimeData.HoldingData.CityIds != null && runtimeData.HoldingData.CityIds.Count > 0)
            {
                targetCityId = runtimeData.HoldingData.CityIds[0];
            }
            else
            {
                var allCities = Resources.LoadAll<CitySO>("City");
                if (allCities.Length > 0) targetCityId = allCities[0].Id;
            }
            
            // CompanyManager üzerinden oluþtur (Bakiye düþme iþlemini manager yapar)
            var newCompany = GameManager.Instance.CompanyManager.CreateCompany(companyType, companyName, targetCityId);
            
            if (newCompany != null)
            {
                // Container'ý kapat
                if (NewCompanyContainer != null) NewCompanyContainer.SetActive(false);

                // Listeyi yenile
                PopulateList();
                
                // Baþarýlý Pop-up
                GameManager.Instance.EventManager.EventBus.Publish(new EmpireX.Events.ShowSystemPopupEvent
                {
                    Title = "Tebrikler",
                    Message = $"{companyName} baþarýyla kuruldu!",
                    Button1Text = "Tamam",
                    Button1Callback = null
                });
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
