using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using EmpireX.Core;
using EmpireX.Data;

namespace EmpireX.UI
{
    public class NewGamePanel : BasePanel
    {
        [Header("Screens")]
        public GameObject FirstScreen;
        public GameObject SecondScreen;

        [Header("First Screen References")]
        public TMP_InputField CeoNameIF;
        public TMP_InputField HoldingNameIF;
        public BasePanel MainMenuPanel; // Geri dönmek için

        [Header("Second Screen References")]
        public TMP_Dropdown CountryDropdown;
        public TMP_Dropdown CityDropdown;
        public UnityEngine.UI.Button PlayBtn;

        private List<CountrySO> _allCountries = new List<CountrySO>();
        private List<CitySO> _allCities = new List<CitySO>();
        private List<CitySO> _currentCountryCities = new List<CitySO>();

        public override void Show()
        {
            base.Show();
            ResetPanel();
        }

        public override void ShowImmediate()
        {
            base.ShowImmediate();
            ResetPanel();
        }

        private void ResetPanel()
        {
            if (FirstScreen != null) FirstScreen.SetActive(true);
            if (SecondScreen != null) SecondScreen.SetActive(false);

            if (CeoNameIF != null) CeoNameIF.text = "";
            if (HoldingNameIF != null) HoldingNameIF.text = "";

            LoadDropdownData();
        }

        private void LoadDropdownData()
        {
            _allCountries.Clear();
            _allCountries.AddRange(Resources.LoadAll<CountrySO>("Country"));
            
            _allCities.Clear();
            _allCities.AddRange(Resources.LoadAll<CitySO>("City"));

            if (CountryDropdown != null)
            {
                CountryDropdown.ClearOptions();
                var options = new List<string>();
                options.Add("Ülke Seçiniz..."); // Index 0 (Placeholder)
                
                foreach (var country in _allCountries)
                {
                    options.Add(country.Name);
                }
                CountryDropdown.AddOptions(options);
                CountryDropdown.value = 0; // Placeholder seçili

                CountryDropdown.onValueChanged.RemoveAllListeners();
                CountryDropdown.onValueChanged.AddListener(OnCountrySelected);
            }

            if (CityDropdown != null)
            {
                CityDropdown.ClearOptions();
                CityDropdown.AddOptions(new List<string> { "Önce Ülke Seçiniz..." });
                CityDropdown.value = 0;
                CityDropdown.interactable = false;
            }
            
            if (PlayBtn != null)
            {
                PlayBtn.interactable = false; // Şehir seçilene kadar kapalı tutalım
            }
        }

        private void OnCountrySelected(int index)
        {
            // index 0 "Ülke Seçiniz..." olduğu için gerçek ülke indexi index - 1 olur
            if (index == 0) 
            {
                if (CityDropdown != null)
                {
                    CityDropdown.ClearOptions();
                    CityDropdown.AddOptions(new List<string> { "Önce Ülke Seçiniz..." });
                    CityDropdown.value = 0;
                    CityDropdown.interactable = false;
                }
                if (PlayBtn != null) PlayBtn.interactable = false;
                return;
            }

            int countryIndex = index - 1;
            if (countryIndex < 0 || countryIndex >= _allCountries.Count) return;

            var selectedCountry = _allCountries[countryIndex];
            
            if (CityDropdown != null)
            {
                CityDropdown.ClearOptions();
                _currentCountryCities.Clear();
                var options = new List<string>();
                options.Add("Şehir Seçiniz..."); // Placeholder

                foreach (var city in _allCities)
                {
                    if (city.CountryId == selectedCountry.Id)
                    {
                        _currentCountryCities.Add(city);
                        options.Add(city.Name);
                    }
                }

                if (_currentCountryCities.Count == 0)
                {
                    options.Add("Şehir Bulunamadı");
                    CityDropdown.interactable = false;
                }
                else
                {
                    CityDropdown.interactable = true;
                }

                CityDropdown.AddOptions(options);
                CityDropdown.value = 0;

                CityDropdown.onValueChanged.RemoveAllListeners();
                CityDropdown.onValueChanged.AddListener(OnCitySelected);
            }

            if (PlayBtn != null) PlayBtn.interactable = false;
        }

        private void OnCitySelected(int index)
        {
            if (PlayBtn != null)
            {
                // index 0 "Şehir Seçiniz..." placeholder'ı, 0'dan büyükse geçerli bir şehir seçilmiş demektir.
                PlayBtn.interactable = index > 0;
            }
        }

        // Önceden StartGameBtn ile bağlıydı, şimdi 1. Ekrandaki Next/İleri butonuyla bağlanmalı
        public void OnNextClicked() 
        {
            string ceoName = CeoNameIF.text;
            string holdingName = HoldingNameIF.text;

            if (string.IsNullOrEmpty(ceoName) || string.IsNullOrEmpty(holdingName))
            {
                Debug.LogWarning("Lütfen tüm alanları doldurun!");
                return;
            }

            if (FirstScreen != null) FirstScreen.SetActive(false);
            if (SecondScreen != null) SecondScreen.SetActive(true);
        }

        // İkinci ekrandaki PlayBtn (Oyna) butonuna bağlanmalı
        public void OnStartGameClicked() 
        {
            string ceoName = CeoNameIF.text;
            string holdingName = HoldingNameIF.text;

            Debug.Log($"[NewGamePanel] Yeni oyun başlatılıyor... CEO: {ceoName}, Holding: {holdingName}");

            if (GameManager.Instance != null && GameManager.Instance.SaveManager != null)
            {
                Debug.Log("[NewGamePanel] GameManager bulundu, Save Data oluşturuluyor...");
                var newData = new EmpireX.Data.SaveData();
                newData.ValidateAndInitializeMissing();
                newData.PlayerData.PlayerName = ceoName;
                newData.HoldingData.Name = holdingName;

                // Seçilen Ülke ve Şehri Holding verisine ekle
                CountrySO selectedCountry = null;
                if (CountryDropdown != null && CountryDropdown.value > 0)
                {
                    selectedCountry = _allCountries[CountryDropdown.value - 1];
                    newData.HoldingData.CountryIds.Add(selectedCountry.Id);
                }
                
                if (CityDropdown != null && CityDropdown.value > 0)
                {
                    newData.HoldingData.CityIds.Add(_currentCountryCities[CityDropdown.value - 1].Id);
                }

                // 1. Config'deki tüm ülkeleri SaveData'ya kopyala
                foreach (var c in _allCountries)
                {
                    var cData = new CountryData
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Currency = c.Currency,
                        TaxRate = c.Tax,
                        Inflation = c.Inflation,
                        InterestRate = c.InterestRate,
                        Stability = c.Stability,
                        EconomyLevel = c.Economy
                    };
                    newData.Countries.Add(cData);
                }

                // 2. Config'deki tüm şehirleri SaveData'ya kopyala
                foreach (var ct in _allCities)
                {
                    var ctData = new CityData
                    {
                        Id = ct.Id,
                        Name = ct.Name,
                        Rent = ct.Rent,
                        Workforce = ct.Workforce,
                        Demand = ct.Demand,
                        Competition = ct.Competition,
                        CityBonus = 0f
                    };
                    newData.Cities.Add(ctData);
                }

                // 3. Ekonomi başlangıç değerlerini seçili ülkeye göre ayarla
                if (selectedCountry != null)
                {
                    newData.EconomyData.Inflation = selectedCountry.Inflation;
                    newData.EconomyData.TaxRate = selectedCountry.Tax;
                    newData.EconomyData.InterestRate = selectedCountry.InterestRate;
                    newData.EconomyData.ExchangeRate = 1.0f; // Default
                }

                GameManager.Instance.SaveManager.SetCurrentData(newData, holdingName);
                GameManager.Instance.SaveManager.ManualSave(holdingName); 
                GameManager.Instance.SaveManager.SetAutoSave(true);
                
                Debug.Log($"[NewGamePanel] Save dosyası başarıyla oluşturuldu. Dosya Adı: {holdingName}");
            }
            else
            {
                Debug.LogError("[NewGamePanel] HATA: GameManager veya SaveManager bulunamadı! Sahneye GameManager ekli olduğundan emin olun. Save alınamadan sahneye geçiliyor...");
            }

            Debug.Log("[NewGamePanel] Sahne geçişi (GameScene) yapılıyor ve Simülasyon başlatılıyor...");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGameSimulation();
                GameManager.Instance.SceneManager.LoadSceneAsync("GameScene");
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
            }
        }

        public void OnBackClicked()
        {
            if (SecondScreen != null && SecondScreen.activeSelf)
            {
                // SecondScreen'deysek FirstScreen'e geri dön
                SecondScreen.SetActive(false);
                FirstScreen.SetActive(true);
            }
            else
            {
                if (MainMenuPanel != null)
                {
                    UINavigation.Instance.ShowPanel(MainMenuPanel);
                }
                else
                {
                    Hide();
                }
            }
        }
    }
}
