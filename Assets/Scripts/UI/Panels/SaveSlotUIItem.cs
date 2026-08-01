using UnityEngine;
using TMPro;
using EmpireX.Data;

namespace EmpireX.UI
{
    public class SaveSlotUIItem : MonoBehaviour
    {
        public TMP_Text SaveCountName;
        public TMP_Text HoldingName;
        public TMP_Text SaveDateText;
        public TMP_Text SaveHourText;
        
        private string _slotId;

        public void Setup(string slotId, SaveData data, int index)
        {
            _slotId = slotId;

            if (SaveCountName != null) SaveCountName.text = $"Kayıt {index}";
            if (HoldingName != null) HoldingName.text = data.HoldingData.Name;

            // SaveDate, Ticks olarak kaydedilmişti
            var date = new System.DateTime(data.SaveDate);
            if (SaveDateText != null) SaveDateText.text = date.ToString("dd.MM.yyyy");
            if (SaveHourText != null) SaveHourText.text = date.ToString("HH:mm");
        }

        public void OnLoadClicked()
        {
            if (EmpireX.Core.GameManager.Instance != null && EmpireX.Core.GameManager.Instance.EventManager != null)
            {
                EmpireX.Core.GameManager.Instance.EventManager.EventBus.Publish(new EmpireX.Events.ShowSystemPopupEvent
                {
                    Title = "Oyunu Yükle",
                    Message = "Kaydedilmemiş veriler silinir, emin misiniz?",
                    Severity = EmpireX.Events.ErrorSeverity.Warning,
                    Button1Text = "Evet",
                    Button1Callback = () => 
                    {
                        if (EmpireX.Core.GameManager.Instance.SaveManager != null)
                        {
                            EmpireX.Core.GameManager.Instance.SaveManager.LoadGame(_slotId);
                            EmpireX.Core.GameManager.Instance.SaveManager.SetAutoSave(true);
                            EmpireX.Core.GameManager.Instance.StartGameSimulation();
                            EmpireX.Core.GameManager.Instance.SceneManager.LoadSceneAsync("GameScene");
                        }
                    },
                    Button2Text = "Hayır",
                    Button2Callback = () => {}
                });
            }
        }

        public void OnDeleteClicked()
        {
            if (EmpireX.Core.GameManager.Instance != null && EmpireX.Core.GameManager.Instance.SaveManager != null)
            {
                // Core sistemden dosyayı sil
                EmpireX.Core.GameManager.Instance.SaveManager.DeleteSave(_slotId);
                // Ekranda listeden bu objeyi kaldır
                Destroy(gameObject);
            }
        }
    }
}
