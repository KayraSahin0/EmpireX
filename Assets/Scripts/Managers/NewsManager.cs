using System;
using System.Linq;
using System.Collections.Generic;
using EmpireX.Events;
using EmpireX.Data;

namespace EmpireX.News
{
    public class NewsManager : EmpireX.Core.BaseManager
    {
        private List<NewsData> _news;
        private TimeData _timeData;

        public NewsManager(IEventBus eventBus) : base(eventBus)
        {
        }

        public override void Initialize()
        {
            _eventBus.Subscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Subscribe<RandomEventTriggered>(OnRandomEventTriggered);
            _eventBus.Subscribe<CompanyCreated>(OnCompanyCreated);
            _eventBus.Subscribe<CompanyUpgraded>(OnCompanyUpgraded);
            _eventBus.Subscribe<ResearchCompleted>(OnResearchCompleted);
            _eventBus.Subscribe<CountryEconomyChanged>(OnCountryEconomyChanged);
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<LoadCompleted>(OnLoadCompleted);
            _eventBus.Unsubscribe<RandomEventTriggered>(OnRandomEventTriggered);
            _eventBus.Unsubscribe<CompanyCreated>(OnCompanyCreated);
            _eventBus.Unsubscribe<CompanyUpgraded>(OnCompanyUpgraded);
            _eventBus.Unsubscribe<ResearchCompleted>(OnResearchCompleted);
            _eventBus.Unsubscribe<CountryEconomyChanged>(OnCountryEconomyChanged);
        }

        private void OnLoadCompleted(LoadCompleted e)
        {
            _news = e.Data.News;
            _timeData = e.Data.TimeData;
        }

        public void AddNews(string title, string description, int type)
        {
            if (_news == null) return;

            var newsItem = new NewsData
            {
                Id = Guid.NewGuid().ToString(),
                Title = title,
                Description = description,
                Type = type,
                Date = _timeData != null ? _timeData.Tick : 0
            };

            _news.Insert(0, newsItem); 

            // Hafızayı korumak için en eski haberleri sil (Sadece son 50 haberi tut)
            if (_news.Count > 50)
            {
                _news.RemoveAt(_news.Count - 1);
            }

            _eventBus.Publish(new NewsGenerated { NewsId = newsItem.Id });
        }

        private void OnRandomEventTriggered(RandomEventTriggered e)
        {
            AddNews(e.EventName, e.Description, 4); // 4 = World/Random News
        }

        private void OnCompanyCreated(CompanyCreated e)
        {
            AddNews("Yeni Şirket Kuruldu", "Piyasaya yeni bir şirket giriş yaptı, piyasa oyuncuları tetikte.", 2); // 2 = Company News
        }

        private void OnCompanyUpgraded(CompanyUpgraded e)
        {
            AddNews("Şirket Büyümesi", $"Sektördeki bir şirket operasyonlarını genişleterek Seviye {e.NewLevel} kapasitesine ulaştı.", 2);
        }

        private void OnResearchCompleted(ResearchCompleted e)
        {
            AddNews("Ar-Ge Başarısı", "Holdinge bağlı araştırma laboratuvarı yeni bir projeyi başarıyla tamamladı.", 1); // 1 = Holding News
        }

        private void OnCountryEconomyChanged(CountryEconomyChanged e)
        {
            // Sadece enflasyon veya vergi ciddi oranda değiştiğinde haber yap
            // Basit bir örnek olarak rastgelelik ekleyelim, her gün haber dolmasın.
            Random rnd = new Random();
            if (rnd.NextDouble() < 0.05) // %5 ihtimalle haber değeri taşır
            {
                AddNews("Ekonomide Dalgalanma", "Makroekonomik veriler güncellendi. Piyasalar yeni vergi ve enflasyon oranlarını değerlendiriyor.", 3); // 3 = Economy News
            }
        }
        
        public List<NewsData> GetLatestNews(int count)
        {
            return _news != null ? _news.Take(count).ToList() : new List<NewsData>();
        }
    }
}
