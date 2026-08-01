using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmpireX.Data;

namespace EmpireX.UI
{
    public class CountryUIItem : MonoBehaviour
    {
        public Image CountryLogo;
        public TMP_Text CountryNameText;
        public TMP_Text CountryCityCoundText;
        public GameObject CountryLockIcon;
        public Button ClickBtn;

        public void Setup(CountrySO countrySo, bool isUnlocked, int cityCount)
        {
            CountryNameText.text = countrySo.Name;
            CountryCityCoundText.text = cityCount.ToString();

            if (CountryLogo != null && countrySo.CountryLogo != null)
            {
                CountryLogo.sprite = countrySo.CountryLogo;
            }

            if (CountryLockIcon != null) CountryLockIcon.SetActive(!isUnlocked);
            
            if (ClickBtn != null)
            {
                ClickBtn.interactable = isUnlocked;
            }
        }
    }
}
