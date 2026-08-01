using UnityEngine;
using EmpireX.Core;
using System.Collections.Generic;

namespace EmpireX.UI
{
    public class LoadMenuPanel : BasePanel
    {
        public Transform SaveContainer; // ButonsContainer yerine SaveContainer
        public GameObject SaveSlotPrefab;

        public override void Show()
        {
            base.Show();
            PopulateSaveFiles();
        }

        private void PopulateSaveFiles()
        {
            // Önce mevcut listeyi temizle
            foreach (Transform child in SaveContainer)
            {
                Destroy(child.gameObject);
            }

            if (GameManager.Instance == null || GameManager.Instance.SaveManager == null) return;

            var allSaves = GameManager.Instance.SaveManager.GetAllSaves();
            int index = 1;

            foreach (var kvp in allSaves)
            {
                var go = Instantiate(SaveSlotPrefab, SaveContainer);
                var itemUI = go.GetComponent<SaveSlotUIItem>();
                
                if (itemUI != null)
                {
                    itemUI.Setup(kvp.Key, kvp.Value, index);
                }
                index++;
            }
        }

        public void OnBackClicked()
        {
            UINavigation.Instance.GoBack();
        }
    }
}
