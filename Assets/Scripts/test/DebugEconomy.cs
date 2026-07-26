using UnityEngine;
using TMPro;
using EmpireX.Core;
using EmpireX.Events;
using EmpireX.Data;

public class DebugEconomy : MonoBehaviour
{
    private string testCompanyId = "";
    private string testEmployeeId = "";
    
    [Header("UI Referansları")]
    public TextMeshProUGUI moneyText;

    private void Start()
    {
        Invoke(nameof(SubscribeEvents), 0.5f);
    }

    private void SubscribeEvents()
    {
        if (GameManager.Instance != null && GameManager.Instance.EventManager != null)
        {
            var eventBus = GameManager.Instance.EventManager.EventBus;
            
            eventBus.Subscribe<DayStarted>(OnDayStarted);
            eventBus.Subscribe<MonthStarted>(OnMonthStarted);
            eventBus.Subscribe<EconomyUpdated>(OnEconomyUpdated);
            
            eventBus.Subscribe<CompanyCreated>(OnCompanyCreated);
            eventBus.Subscribe<CompanyUpgraded>(OnCompanyUpgraded);
            eventBus.Subscribe<CompanyCreationFailed>(OnCompanyFailed);
            
            eventBus.Subscribe<EmployeeHired>(OnEmployeeHired);
            eventBus.Subscribe<EmployeeTrained>(OnEmployeeTrained);
            eventBus.Subscribe<EmployeeActionFailed>(OnEmployeeFailed);

            eventBus.Subscribe<OfficeCreated>(OnOfficeCreated);
            eventBus.Subscribe<OfficeUpgraded>(OnOfficeUpgraded);
            eventBus.Subscribe<OfficeActionFailed>(OnOfficeFailed);
            
                        eventBus.Subscribe<BranchCreated>(OnBranchCreated);
            eventBus.Subscribe<HoldingUpgraded>(OnHoldingUpgraded);
            eventBus.Subscribe<HoldingStatsUpdated>(OnHoldingStatsUpdated);
            eventBus.Subscribe<ExecutiveHired>(OnExecutiveHired);
            eventBus.Subscribe<ResearchStarted>(OnResearchStarted);
            eventBus.Subscribe<ResearchCompleted>(OnResearchCompleted);
            Debug.Log("DebugEconomy: Event'lere başarıyla abone olundu.");
        }
    }

    private void Update()
    {
        if (moneyText != null && GameManager.Instance != null && GameManager.Instance.SaveManager != null)
        {
            var data = GameManager.Instance.SaveManager.CurrentData;
            if (data != null && data.HoldingData != null)
            {
                moneyText.text = $"Kasa: {data.HoldingData.Cash:C0}";
            }
        }
    }

    public void CompanyCreate()
    {
        if (GameManager.Instance != null && GameManager.Instance.CompanyManager != null)
        {
            var typeSO = ScriptableObject.CreateInstance<CompanyTypeSO>();
            typeSO.Id = "tech_1";
            typeSO.Name = "Teknoloji";
            typeSO.BaseCost = 50000;
            typeSO.BaseRevenue = 20000;
            typeSO.BaseExpense = 8000;

            GameManager.Instance.CompanyManager.CreateCompany(typeSO, "Test Bilişim A.Ş.", "city_1");
        }
    }

    public void CompanyUpgrade()
    {
        if (!string.IsNullOrEmpty(testCompanyId) && GameManager.Instance != null)
        {
            GameManager.Instance.CompanyManager.UpgradeCompany(testCompanyId);
        }
    }

    public void GiveMoneyCheat()
    {
        if (GameManager.Instance != null && GameManager.Instance.EconomyManager != null)
        {
            GameManager.Instance.EconomyManager.AddRevenue(500000, "Test Para Hilesi");
        }
    }
    
    public void EmployeeHire()
    {
        if (GameManager.Instance != null && GameManager.Instance.EmployeeManager != null)
        {
            if (!string.IsNullOrEmpty(testCompanyId))
            {
                var emp = GameManager.Instance.EmployeeManager.HireEmployee(testCompanyId, "dev_1");
                if (emp != null) testEmployeeId = emp.Id;
            }
        }
    }

    public void EmployeeTrain()
    {
        if (GameManager.Instance != null && GameManager.Instance.EmployeeManager != null)
        {
            if (!string.IsNullOrEmpty(testEmployeeId))
            {
                GameManager.Instance.EmployeeManager.TrainEmployee(testEmployeeId);
            }
        }
    }

    // YENİ: Ofis Kurulumu
    public void OfficeCreate()
    {
        if (GameManager.Instance != null && GameManager.Instance.OfficeManager != null)
        {
            if (!string.IsNullOrEmpty(testCompanyId))
            {
                GameManager.Instance.OfficeManager.CreateOffice(testCompanyId, "Ana Merkez Ofisi");
            }
            else
            {
                Debug.LogWarning("Önce şirket kurmalısın!");
            }
        }
    }

    // YENİ: Ofis Geliştirme
    public void OfficeUpgrade()
    {
        if (GameManager.Instance != null && GameManager.Instance.OfficeManager != null)
        {
            if (!string.IsNullOrEmpty(testCompanyId))
            {
                var office = GameManager.Instance.OfficeManager.GetOfficeByOwner(testCompanyId);
                if (office != null)
                {
                    GameManager.Instance.OfficeManager.UpgradeOffice(office.Id);
                }
                else
                {
                    Debug.LogWarning("Önce ofis kurmalısın!");
                }
            }
        }
    }

    
    // YENİ: Şube Kurulumu
    public void BranchCreate()
    {
        if (GameManager.Instance != null && GameManager.Instance.BranchManager != null)
        {
            if (!string.IsNullOrEmpty(testCompanyId))
            {
                GameManager.Instance.BranchManager.CreateBranch(testCompanyId, "city_1", "Test Şube");
            }
            else
            {
                Debug.LogWarning("Önce şirket kurmalısın!");
            }
        }
    }

    // YENİ: Holding Geliştirme
        // YENİ: Şube Geliştirme
    public void BranchUpgrade()
    {
        if (GameManager.Instance != null && GameManager.Instance.BranchManager != null)
        {
            if (!string.IsNullOrEmpty(testCompanyId))
            {
                // İlk bulduğu şubeyi yükseltir
                var branches = GameManager.Instance.BranchManager.GetBranchesByCompany(testCompanyId);
                if (branches.Count > 0)
                {
                    GameManager.Instance.BranchManager.UpgradeBranch(branches[0].Id);
                }
                else
                {
                    Debug.LogWarning("Şirkete ait hiç şube yok!");
                }
            }
            else
            {
                Debug.LogWarning("Önce şirket kurmalısın!");
            }
        }
    }

    
    // YENİ: Yönetici İşe Alımı (HR)
    public void ExecutiveHireHR()
    {
        if (GameManager.Instance != null && GameManager.Instance.ExecutiveManager != null)
        {
            if (!string.IsNullOrEmpty(testCompanyId))
            {
                GameManager.Instance.ExecutiveManager.HireExecutive(testCompanyId, "HR");
            }
            else
            {
                Debug.LogWarning("Önce şirket kurmalısın!");
            }
        }
    }

    private void OnExecutiveHired(ExecutiveHired e)
    {
        Debug.Log($"<color=white>[YÖNETİCİ]</color> {e.Role} pozisyonuna yönetici atandı! (Global bonuslar aktif)");
    }

    
    // YENİ: Araştırma Başlat
    public void ResearchStart()
    {
        if (GameManager.Instance != null && GameManager.Instance.ResearchManager != null)
        {
            var resSO = ScriptableObject.CreateInstance<ResearchSO>();
            resSO.Id = "res_tax_1";
            resSO.Name = "Vergi Optimizasyonu I";
            resSO.Cost = 20000;
            resSO.Duration = 5; // 5 Günde biter
            resSO.Prerequisites = new System.Collections.Generic.List<string>();

            GameManager.Instance.ResearchManager.StartResearch(resSO);
        }
    }

    private void OnResearchStarted(ResearchStarted e)
    {
        Debug.Log($"<color=orange>[ARAŞTIRMA]</color> Araştırma Kuyruğa Alındı! ID: {e.ResearchId}");
    }

    private void OnResearchCompleted(ResearchCompleted e)
    {
        Debug.Log($"<color=orange>[ARAŞTIRMA]</color> Araştırma Tamamlandı! Kilit Açıldı: {e.ResearchId} (Bonuslar aktif)");
    }

    public void HoldingUpgrade()
    {
        if (GameManager.Instance != null && GameManager.Instance.HoldingManager != null)
        {
            GameManager.Instance.HoldingManager.UpgradeHolding();
        }
    }

    private void OnBranchCreated(BranchCreated e)
    {
        Debug.Log($"<color=lime>[ŞUBE]</color> Şirkete Yeni Şube Eklendi! Şube ID: {e.BranchId}");
    }

    private void OnHoldingUpgraded(HoldingUpgraded e)
    {
        Debug.Log($"<color=magenta>[HOLDING]</color> Holding Seviye Atladı! Yeni Seviye: {e.NewLevel}");
    }

    private void OnHoldingStatsUpdated(HoldingStatsUpdated e)
    {
        Debug.Log($"<color=magenta>[HOLDING İSTATİSTİK]</color> Toplam Çalışan: {e.Data.TotalEmployees} | Toplam Ciro: {e.Data.TotalRevenue:C2}");
    }

    private void OnDayStarted(DayStarted e)
    {
        if (e.Day % 10 == 0) Debug.Log($"<color=yellow>[ZAMAN]</color> Gün {e.Day}");
    }

    private void OnMonthStarted(MonthStarted e)
    {
        Debug.Log($"<color=orange>[ZAMAN]</color> Yeni Ay Başladı: Ay {e.Month}");
    }

    private void OnEconomyUpdated(EconomyUpdated e)
    {
        Debug.Log($"<color=green>[EKONOMİ]</color> Net Değer: {e.EconomyData.NetWorth:C2}");
    }

    private void OnCompanyCreated(CompanyCreated e)
    {
        testCompanyId = e.CompanyId;
        Debug.Log($"<color=cyan>[ŞİRKET]</color> Şirket Kuruldu: {e.CompanyId}");
    }

    private void OnCompanyUpgraded(CompanyUpgraded e)
    {
        Debug.Log($"<color=cyan>[ŞİRKET]</color> Şirket Geliştirildi! Seviye: {e.NewLevel}");
    }

    private void OnCompanyFailed(CompanyCreationFailed e)
    {
        Debug.LogError($"<color=red>[HATA]</color> {e.Reason}");
    }

    private void OnEmployeeHired(EmployeeHired e)
    {
        Debug.Log($"<color=purple>[ÇALIŞAN]</color> Yeni Çalışan işe alındı! ID: {e.EmployeeId}");
    }

    private void OnEmployeeTrained(EmployeeTrained e)
    {
        Debug.Log($"<color=purple>[ÇALIŞAN]</color> Çalışan Eğitildi!");
    }
    
    private void OnEmployeeFailed(EmployeeActionFailed e)
    {
        Debug.LogError($"<color=red>[HATA]</color> {e.Reason}");
    }

    private void OnOfficeCreated(OfficeCreated e)
    {
        Debug.Log($"<color=blue>[OFİS]</color> Şirket Ofisi Kuruldu! (Artık daha fazla çalışan alabilirsiniz)");
    }

    private void OnOfficeUpgraded(OfficeUpgraded e)
    {
        Debug.Log($"<color=blue>[OFİS]</color> Ofis Geliştirildi! Yeni Seviye: {e.NewLevel} (Çalışan Mutluluk Bonusu Arttı)");
    }

    private void OnOfficeFailed(OfficeActionFailed e)
    {
        Debug.LogError($"<color=red>[HATA]</color> {e.Reason}");
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null && GameManager.Instance.EventManager != null)
        {
            var eventBus = GameManager.Instance.EventManager.EventBus;
            eventBus.Unsubscribe<DayStarted>(OnDayStarted);
            eventBus.Unsubscribe<MonthStarted>(OnMonthStarted);
            eventBus.Unsubscribe<EconomyUpdated>(OnEconomyUpdated);
            eventBus.Unsubscribe<CompanyCreated>(OnCompanyCreated);
            eventBus.Unsubscribe<CompanyUpgraded>(OnCompanyUpgraded);
            eventBus.Unsubscribe<CompanyCreationFailed>(OnCompanyFailed);
            eventBus.Unsubscribe<EmployeeHired>(OnEmployeeHired);
            eventBus.Unsubscribe<EmployeeTrained>(OnEmployeeTrained);
            eventBus.Unsubscribe<EmployeeActionFailed>(OnEmployeeFailed);
            eventBus.Unsubscribe<OfficeCreated>(OnOfficeCreated);
            eventBus.Unsubscribe<OfficeUpgraded>(OnOfficeUpgraded);
            eventBus.Unsubscribe<OfficeActionFailed>(OnOfficeFailed);
            eventBus.Unsubscribe<BranchCreated>(OnBranchCreated);
            eventBus.Unsubscribe<HoldingUpgraded>(OnHoldingUpgraded);
            eventBus.Unsubscribe<HoldingStatsUpdated>(OnHoldingStatsUpdated);
            eventBus.Unsubscribe<ExecutiveHired>(OnExecutiveHired);
            eventBus.Unsubscribe<ResearchStarted>(OnResearchStarted);
            eventBus.Unsubscribe<ResearchCompleted>(OnResearchCompleted);
        }
    }
}




