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
            if (citySo == null) return;

            if (CityNameText != null) CityNameText.text = citySo.Name;
            else Debug.LogError("[CityUIItem] CityNameText atanmamış!");

            if (CityTaxText != null) CityTaxText.text = $"%{citySo.Tax * 100}";
            else Debug.LogError("[CityUIItem] CityTaxText atanmamış!");

            if (CityCorpCount != null) CityCorpCount.text = $"Şehirdeki mevcut şirket sayısı: {corpCount}";
            else Debug.LogError("[CityUIItem] CityCorpCount atanmamış!");

            if (LockOverlay != null) LockOverlay.SetActive(!isUnlocked);
            
            if (ClickBtn != null)
            {
                ClickBtn.interactable = isUnlocked;
            }
        }
    }
}
