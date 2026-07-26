using UnityEngine;
using EmpireX.Core;
using EmpireX.Data;
using System.Linq;

namespace EmpireX.Test
{
    public class DebugAcquisition : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                CreateRandomCompetitor();
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                BuyFirstCompetitor();
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                MergeFirstTwoCompanies();
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                IPOFirstCompany();
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                SellFirstCompany();
            }
        }

        private void IPOFirstCompany()
        {
            if (GameManager.Instance == null) return;
            var hd = GameManager.Instance.SaveManager.CurrentData.HoldingData;

            if (hd.CompanyIds.Count > 0)
            {
                string id = hd.CompanyIds[0];
                bool success = GameManager.Instance.StockManager.GoPublic(id, 0.40f, 0.05f);
                Debug.Log($"[Stock Test] Halka Arz Denemesi: " + (success ? "BAŞARILI (%40 Halka Arz, %5 Temettü)" : "BAŞARISIZ (Zaten halka açık olabilir)"));
            }
        }

        private void CreateRandomCompetitor()
        {
            if (GameManager.Instance == null) return;
            var cm = GameManager.Instance.CompanyManager;
            
            // Rastgele bir isim üret
            string[] names = { "Titan Corp", "Global Dynamics", "Apex Industries", "Nexus Tech", "Umbrella Co" };
            string name = names[Random.Range(0, names.Length)] + " " + Random.Range(10, 99);
            
            // Dummy bir tip oluştur
            var type = ScriptableObject.CreateInstance<CompanyTypeSO>();
            type.Id = "comp_tech_1";
            type.BaseCost = 50000;
            type.BaseRevenue = 15000;
            type.BaseExpense = 8000;
            
            cm.CreateCompetitorCompany(type, name, "city_istanbul");
            Debug.Log($"[Acquisition Test] Rakip şirket oluşturuldu: {name}");
        }

        private void BuyFirstCompetitor()
        {
            if (GameManager.Instance == null) return;
            var hd = GameManager.Instance.SaveManager.CurrentData.HoldingData;
            var allCompanies = GameManager.Instance.CompanyManager.GetAllCompanies();
            
            var competitor = allCompanies.FirstOrDefault(c => !hd.CompanyIds.Contains(c.Id));
            if (competitor != null)
            {
                bool success = GameManager.Instance.AcquisitionManager.BuyCompany(competitor.Id);
                Debug.Log($"[Acquisition Test] Satın alma denemesi ({competitor.Name}): " + (success ? "BAŞARILI" : "BAŞARISIZ (Bakiye yetersiz olabilir)"));
            }
            else
            {
                Debug.LogWarning("[Acquisition Test] Piyasada alınacak rakip şirket yok. Önce 'O' tuşu ile üretin.");
            }
        }

        private void MergeFirstTwoCompanies()
        {
            if (GameManager.Instance == null) return;
            var hd = GameManager.Instance.SaveManager.CurrentData.HoldingData;
            
            if (hd.CompanyIds.Count >= 2)
            {
                string target = hd.CompanyIds[0];
                string absorbed = hd.CompanyIds[1];
                
                bool success = GameManager.Instance.AcquisitionManager.MergeCompanies(target, absorbed);
                Debug.Log($"[Acquisition Test] Birleştirme denemesi: " + (success ? "BAŞARILI" : "BAŞARISIZ"));
            }
            else
            {
                Debug.LogWarning("[Acquisition Test] Holdingin en az 2 şirketi olmalı.");
            }
        }

        private void SellFirstCompany()
        {
            if (GameManager.Instance == null) return;
            var hd = GameManager.Instance.SaveManager.CurrentData.HoldingData;
            
            if (hd.CompanyIds.Count > 0)
            {
                string id = hd.CompanyIds[0];
                bool success = GameManager.Instance.AcquisitionManager.SellCompany(id);
                Debug.Log($"[Acquisition Test] Satış denemesi: " + (success ? "BAŞARILI" : "BAŞARISIZ"));
            }
        }
    }
}
