using UnityEngine;

namespace EmpireX.UI
{
    public class NewsPanel : BasePanel
    {
        public Transform NewsContainer;
        public GameObject NewsPrefab;

        public override void Show()
        {
            base.Show();
            PopulateNews();
        }

        private void PopulateNews()
        {
            if (NewsContainer == null) return;
            
            // Önceki haberleri temizle
            foreach (Transform child in NewsContainer)
            {
                Destroy(child.gameObject);
            }

            // TODO: İleride Data veya NewsManager üzerinden gerçek haberler çekilecek
            // Şimdilik prefab eklenebilsin diye basit bir döngü
            for (int i = 0; i < 2; i++)
            {
                if (NewsPrefab != null)
                {
                    Instantiate(NewsPrefab, NewsContainer);
                }
            }
        }

        public void OnBackClicked()
        {
            UINavigation.Instance.GoBack();
        }
    }
}
