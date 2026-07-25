using UnityEngine;
using EmpireX.Core; // Yöneticilere erişmek için gerekli

public class test : MonoBehaviour
{
    // Butondan tetiklenebilmesi için metodun "public" olması şarttır
    public void SaveBtn()
    {
        // GameManager'ın sahnede var olduğundan emin oluyoruz
        if (GameManager.Instance != null && GameManager.Instance.SaveManager != null)
        {
            GameManager.Instance.SaveManager.ManualSave("Slot_1");
            Debug.Log("Kayıt işlemi başarıyla tetiklendi! Slot_1.sav dosyası oluşturuldu.");
        }
        else
        {
            Debug.LogError("GameManager veya SaveManager bulunamadı. Sahnede GameManager objesi var mı?");
        }
    }
}
