using System;
using System.Collections;
using UnityEngine;
using EmpireX.Events;

namespace EmpireX.Core
{
    /// <summary>
    /// SceneManager sınıfı.
    /// </summary>
    public class SceneManager : BaseManager
    {
        public SceneManager(IEventBus eventBus) : base(eventBus) { }

        public override void Initialize()
        {
            // Initialization logic
        }
        
        public override void Dispose()
        {
            // Cleanup logic
        }

        public void LoadSceneAsync(string sceneName, Action onComplete = null)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartCoroutine(LoadSceneRoutine(sceneName, onComplete));
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
                onComplete?.Invoke();
            }
        }

        private IEnumerator LoadSceneRoutine(string sceneName, Action onComplete)
        {
            // 1. Loading UI Oluştur
            ILoadingPanel loadingPanel = null;
            var prefab = Resources.Load<GameObject>("UI/LoadingPanel");
            if (prefab != null)
            {
                var go = UnityEngine.Object.Instantiate(prefab);
                UnityEngine.Object.DontDestroyOnLoad(go);
                loadingPanel = go.GetComponent<ILoadingPanel>();
            }

            if (loadingPanel != null)
            {
                loadingPanel.Show();
                // Animasyonun bitmesi ve panelin görünmesi için biraz bekle
                yield return new WaitForSecondsRealtime(0.5f);
            }

            // 2. Asenkron Yükleme Başlat
            AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            float simulatedProgress = 0f;

            // 3. Yükleme Barını Güncelle
            while (!operation.isDone)
            {
                // Unity'nin progress değeri 0.9'a kadar çıkar
                float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);
                
                // Animasyon hissiyatı için yumuşak artış
                simulatedProgress = Mathf.MoveTowards(simulatedProgress, targetProgress * 0.9f, Time.unscaledDeltaTime * 1.2f);
                
                if (loadingPanel != null)
                {
                    loadingPanel.UpdateProgress(simulatedProgress);
                }

                if (operation.progress >= 0.9f && simulatedProgress >= 0.9f)
                {
                    break; // Yükleme bitti, %90'dayız
                }

                yield return null;
            }

            // 4. Veri senkronizasyonu veya simülasyon başlangıcı için sahneyi aktif et
            operation.allowSceneActivation = true;

            // Geçişin tamamlanması için kısa bir süre bekle
            yield return null;

            // 5. Yüzde 100'e tamamla
            while (simulatedProgress < 1f)
            {
                simulatedProgress += Time.unscaledDeltaTime * 2f;
                if (loadingPanel != null)
                {
                    loadingPanel.UpdateProgress(Mathf.Clamp01(simulatedProgress));
                }
                yield return null;
            }

            yield return new WaitForSecondsRealtime(0.2f); // %100'ü görsün diye hafif bekleme

            // 6. Loading Ekranını Kapat
            if (loadingPanel != null)
            {
                loadingPanel.Hide(() => 
                {
                    onComplete?.Invoke();
                });
            }
            else
            {
                onComplete?.Invoke();
            }
        }
    }
}
