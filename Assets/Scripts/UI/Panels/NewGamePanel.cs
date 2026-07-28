using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using EmpireX.Core;

namespace EmpireX.UI
{
    public class NewGamePanel : BasePanel
    {
        public TMP_InputField CeoNameIF;
        public TMP_InputField HoldingNameIF;
        public BasePanel MainMenuPanel; // Geri dönmek için

        public void OnStartGameClicked()
        {
            string ceoName = CeoNameIF.text;
            string holdingName = HoldingNameIF.text;

            if (string.IsNullOrEmpty(ceoName) || string.IsNullOrEmpty(holdingName))
            {
                Debug.LogWarning("Lütfen tüm alanları doldurun!");
                return;
            }

            Debug.Log($"[NewGamePanel] Yeni oyun başlatılıyor... CEO: {ceoName}, Holding: {holdingName}");

            if (GameManager.Instance != null && GameManager.Instance.SaveManager != null)
            {
                Debug.Log("[NewGamePanel] GameManager bulundu, Save Data oluşturuluyor...");
                // Yeni veri oluşturup SaveManager içine inject edelim
                var newData = new EmpireX.Data.SaveData();
                newData.ValidateAndInitializeMissing();
                newData.PlayerData.PlayerName = ceoName;
                newData.HoldingData.Name = holdingName;

                GameManager.Instance.SaveManager.SetCurrentData(newData);
                GameManager.Instance.SaveManager.ManualSave(holdingName); // Holding adıyla kaydet
                
                // Oyuncu yeni bir oyuna başladığında AutoSave özelliğini varsayılan olarak açıyoruz
                GameManager.Instance.SaveManager.SetAutoSave(true);
                
                Debug.Log($"[NewGamePanel] Save dosyası başarıyla oluşturuldu. Dosya Adı: {holdingName}");
            }
            else
            {
                Debug.LogError("[NewGamePanel] HATA: GameManager veya SaveManager bulunamadı! Sahneye GameManager ekli olduğundan emin olun. Save alınamadan sahneye geçiliyor...");
            }

            Debug.Log("[NewGamePanel] Sahne geçişi (GameScene) yapılıyor...");
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }

        public void OnBackClicked()
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
