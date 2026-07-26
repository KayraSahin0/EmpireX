using UnityEngine;
using EmpireX.Core;
using EmpireX.Events;
using System.Linq;

namespace EmpireX.Test
{
    /// <summary>
    /// Geliştirme sürecinde oyunun arka planında dönen tüm verileri 
    /// görsel olarak okuyabilmenizi sağlayan test menüsü.
    /// </summary>
    public class DebugStatsUI : MonoBehaviour
    {
        private Vector2 scrollPosition;
        private bool showMenu = true;
        private Rect windowRect = new Rect(20, 60, 400, 700);

        private Rect costsWindowRect = new Rect(Screen.width - 270, 20, 250, 350);

        private string _lastEventTitle = "Yok";
        private string _lastEventDesc = "Henüz bir olay yaşanmadı.";

        private void Start()
        {
            if (GameManager.Instance != null && GameManager.Instance.EventManager != null)
            {
                GameManager.Instance.EventManager.EventBus.Subscribe<RandomEventTriggered>(e => 
                {
                    _lastEventTitle = e.EventName;
                    _lastEventDesc = e.Description;
                });
            }
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(20, 20, 150, 30), showMenu ? "Gizle (Stats Menu)" : "Göster (Stats Menu)"))
            {
                showMenu = !showMenu;
            }

            if (showMenu)
            {
                windowRect = GUI.Window(0, new Rect(20, 60, 400, Screen.height - 80), DrawStatsWindow, "Oyun Verileri (Live Data)");
                
                // Ekran çözünürlüğü değiştiğinde sağ üstte kalmasını sağlamak için X pozisyonunu sürekli güncelleyelim
                costsWindowRect.x = Screen.width - 270;
                costsWindowRect = GUI.Window(1, costsWindowRect, DrawCostsWindow, "İşlem Maliyetleri");
            }
        }

        private void DrawCostsWindow(int windowID)
        {
            GUILayout.Space(5);
            
            GUILayout.Label("<b>--- KURULUM ---</b>");
            GUILayout.Label("Yeni Şirket: $100,000");
            GUILayout.Label("Yeni Şube: $15,000");
            GUILayout.Label("Yeni Ofis: $50,000");

            GUILayout.Space(10);
            GUILayout.Label("<b>--- İSTİHDAM ---</b>");
            GUILayout.Label("Çalışan İşe Alım: $1,000");
            GUILayout.Label("Çalışan Eğitimi: Lvl x $500");
            GUILayout.Label("Yönetici (C-Level): $50,000");

            GUILayout.Space(10);
            GUILayout.Label("<b>--- GELİŞTİRME ---</b>");
            GUILayout.Label("Şube Yükseltme: Lvl x $20,000");
            GUILayout.Label("Holding Seviye Atlatma: Lvl x $1,000,000");
            GUILayout.Label("Araştırma Başlatma: $20,000");

            GUI.DragWindow();
        }

        private void DrawStatsWindow(int windowID)
        {
            // Pencere içerisinde scroll (kaydırma) özelliği açılır
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            
            if (GameManager.Instance == null || GameManager.Instance.SaveManager == null || GameManager.Instance.SaveManager.CurrentData == null)
            {
                GUILayout.Label("Oyun henüz yüklenmedi veya GameManager sistemde yok.");
                GUILayout.EndScrollView();
                GUI.DragWindow();
                return;
            }

            var data = GameManager.Instance.SaveManager.CurrentData;

            // Zaman Sistemi
            GUILayout.Label($"ZAMAN: Yıl {data.TimeData.Year}, Ay {data.TimeData.Month}, Gün {data.TimeData.Day}");
            
            // Son Rastgele Olay
            GUILayout.Space(10);
            GUILayout.Label("<b>--- SON RASTGELE OLAY ---</b>");
            GUILayout.Label($"<b>{_lastEventTitle}</b>");
            GUILayout.Label(_lastEventDesc);

            // Ekonomi Sistemi
            GUILayout.Space(10);
            GUILayout.Label("--- EKONOMİ ---");
            GUILayout.Label($"Toplam Servet (NetWorth): ${data.EconomyData.NetWorth:N2}");
            GUILayout.Label($"Enflasyon: %{data.EconomyData.Inflation * 100:F1}");
            GUILayout.Label($"Vergi Oranı: %{data.EconomyData.TaxRate * 100:F1}");
            GUILayout.Label($"Kredi Faiz Oranı: %{data.EconomyData.InterestRate * 100:F1}");

            // Holding Sistemi
            GUILayout.Space(10);
            GUILayout.Label("--- HOLDING ---");
            GUILayout.Label($"Seviye: {data.HoldingData.Level}");
            GUILayout.Label($"Ana Kasa (Cash): ${data.HoldingData.Cash:N2}");
            GUILayout.Label($"Toplam Ciro (Aylık): ${data.HoldingData.TotalRevenue:N2}");
            GUILayout.Label($"Toplam Gider (Aylık): ${data.HoldingData.TotalExpense:N2}");
            GUILayout.Label($"Bünyedeki Şirket Sayısı: {data.HoldingData.CompanyIds.Count}");

            // --- İSTATİSTİKLER ---
            GUILayout.Space(10);
            GUILayout.Label("<b>--- GENEL İSTATİSTİKLER ---</b>");
            GUILayout.Label($"Toplam Yaratılan Gelir (Tüm Zamanlar): ${data.StatisticsData.TotalRevenue:N2}");
            GUILayout.Label($"Toplam Gider (Tüm Zamanlar): ${data.StatisticsData.TotalExpense:N2}");
            GUILayout.Label($"Net Kâr (Tüm Zamanlar): ${data.StatisticsData.TotalProfit:N2}");
            
            if (data.StatisticsData.NetWorthHistory != null && data.StatisticsData.NetWorthHistory.Count > 0)
            {
                GUILayout.Label($"Geçmiş Ay Kayıt Sayısı: {data.StatisticsData.NetWorthHistory.Count}");
                GUILayout.Label($"Son Ay Net Değer: ${data.StatisticsData.NetWorthHistory.Last():N2}");
            }

            // --- HABERLER ---
            GUILayout.Space(10);
            GUILayout.Label("<b>--- SON HABERLER ---</b>");
            
            if (GameManager.Instance != null && GameManager.Instance.NewsManager != null)
            {
                var latestNews = GameManager.Instance.NewsManager.GetLatestNews(3);
                if (latestNews.Count == 0)
                {
                    GUILayout.Label("Henüz bir haber yok.");
                }
                else
                {
                    foreach (var news in latestNews)
                    {
                        GUILayout.Label($"[{news.Title}] {news.Description}");
                    }
                }
            }

            // Şirket Sistemi
            GUILayout.Space(10);
            GUILayout.Label($"<b>--- ŞİRKETLER ({data.Companies.Count}) ---</b>");
            foreach (var comp in data.Companies)
            {
                bool isOurs = data.HoldingData.CompanyIds.Contains(comp.Id);
                string tag = isOurs ? "<color=green>[BİZİM]</color>" : "<color=red>[RAKİP]</color>";
                string publicTag = comp.IsPublic ? $"<color=yellow>[BORSADA - Hisse: ${comp.SharePrice:F2}]</color>" : "";
                
                GUILayout.Label($"{tag} {publicTag} - {comp.Name} (Lvl {comp.Level}) | Kasa: ${comp.Cash:N0} | Ciro: ${comp.Revenue:N0} | Pazar: %{comp.MarketShare*100:F1} | Değer: ${comp.Value:N0}");
            }

            // Şube Sistemi
            GUILayout.Space(10);
            GUILayout.Label($"--- ŞUBELER ({data.Branches.Count}) ---");
            foreach (var branch in data.Branches)
            {
                GUILayout.Label($"- {branch.Id.Substring(0,4)}... | İstihdam: {branch.Employees} | Ciro: ${branch.Revenue:N0}");
            }

            // Personel Sistemi
            GUILayout.Space(10);
            GUILayout.Label($"--- PERSONEL ({data.Employees.Count}) ---");
            if (data.Employees.Count > 0)
            {
                var avgStress = data.Employees.Average(e => e.Stress);
                var avgHappy = data.Employees.Average(e => e.Happiness);
                GUILayout.Label($"Ortalama Stres: %{avgStress:F1}");
                GUILayout.Label($"Ortalama Mutluluk: %{avgHappy:F1}");
            }

            // Yönetici (Executive) Sistemi
            GUILayout.Space(10);
            GUILayout.Label($"--- YÖNETİCİLER ({data.Executives.Count}) ---");
            foreach (var exec in data.Executives)
            {
                GUILayout.Label($"- Pozisyon: {exec.ExecutiveTypeId} (Lvl {exec.Level}) | Maaş: ${exec.Salary:N0}");
            }

            // Şehir Sistemi
            GUILayout.Space(10);
            GUILayout.Label($"--- ŞEHİRLER ({data.Cities.Count}) ---");
            foreach (var city in data.Cities)
            {
                GUILayout.Label($"- {city.Name} | Rekabet: %{city.Competition*100:F1} | Kira: ${city.Rent:N0} | Talep Oranı: {city.Demand:F2}");
            }

            // Araştırma Sistemi
            GUILayout.Space(10);
            GUILayout.Label($"--- ARAŞTIRMALAR ({data.Researches.Count}) ---");
            foreach (var res in data.Researches)
            {
                string status = res.IsUnlocked ? "Tamamlandı" : $"Devam Ediyor ({res.RemainingTime} gün kaldı)";
                GUILayout.Label($"- ID: {res.Id} | Durum: {status}");
            }

            // Başarım (Achievement) Sistemi
            GUILayout.Space(10);
            GUILayout.Label($"<b>--- BAŞARIMLAR (Açılan: {data.HoldingData.AchievementIds.Count}) ---</b>");
            if (data.HoldingData.AchievementIds.Count == 0)
            {
                GUILayout.Label("Henüz hiçbir başarım kazanılmadı.");
            }
            else
            {
                foreach (var achId in data.HoldingData.AchievementIds)
                {
                    GUILayout.Label($"- [AÇILDI] Başarım ID: {achId}");
                }
            }

            GUILayout.EndScrollView();
            GUI.DragWindow(); // Farenizle pencereyi tutup ekranın istediğiniz yerine sürükleyebilirsiniz
        }
    }
}
