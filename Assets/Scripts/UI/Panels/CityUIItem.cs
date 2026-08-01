using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmpireX.Data;

namespace EmpireX.UI
{
    public class CityUIItem : MonoBehaviour
    {
        public TMP_Text CityNameText;
        public TMP_Text CityCorpCount;
        public TMP_Text CityTaxText;
        public Button ClickBtn;
        public GameObject LockOverlay;

        public void Setup(CitySO citySo, bool isUnlocked, int corpCount)
        {
            CityNameText.text = citySo.Name;
            CityTaxText.text = $"%{citySo.Tax * 100}";
            CityCorpCount.text = corpCount.ToString();

            if (LockOverlay != null) LockOverlay.SetActive(!isUnlocked);
            
            if (ClickBtn != null)
            {
                ClickBtn.interactable = isUnlocked;
            }
        }
    }
}
