using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using EmpireX.Events;
using EmpireX.Data;

namespace EmpireX.Core
{
    public class TelemetryManager : BaseManager
    {
        private readonly ConfigSystem _configSystem;
        private string _telemetryDirectory;
        
        private bool _isSending = false;
        private float _retryInterval = 60f; // 60 saniyede bir offline dosyaları kontrol et
        private Coroutine _retryCoroutine;
        
        public TelemetryManager(IEventBus eventBus, ConfigSystem configSystem) : base(eventBus)
        {
            _configSystem = configSystem;
            _telemetryDirectory = Path.Combine(Application.persistentDataPath, "Telemetry");
        }

        public override void Initialize()
        {
            if (!Directory.Exists(_telemetryDirectory))
            {
                Directory.CreateDirectory(_telemetryDirectory);
            }
            
            _eventBus.Subscribe<SystemErrorEvent>(OnSystemError);
            
            // Projenin lifecycle yapısına göre Update'i Coroutine ile simüle ediyoruz
            _retryCoroutine = GameManager.Instance.StartCoroutine(RetryOfflineReportsRoutine());
        }

        public override void Dispose()
        {
            _eventBus.Unsubscribe<SystemErrorEvent>(OnSystemError);
            
            if (_retryCoroutine != null && GameManager.Instance != null)
            {
                GameManager.Instance.StopCoroutine(_retryCoroutine);
            }
        }

        private void OnSystemError(SystemErrorEvent e)
        {
            SendReport(e);
        }

        public void SendReport(SystemErrorEvent errorEvent)
        {
            TelemetryReport report = new TelemetryReport
            {
                ReportId = Guid.NewGuid().ToString(),
                ErrorType = errorEvent.LogType.ToString(),
                ErrorMessage = errorEvent.ErrorMessage,
                StackTrace = errorEvent.StackTrace,
                OccurredAt = DateTime.UtcNow.ToString("O"),
                AppVersion = Application.version,
                UnityVersion = Application.unityVersion,
                OperatingSystem = SystemInfo.operatingSystem,
                DeviceModel = SystemInfo.deviceModel,
                Severity = errorEvent.Severity.ToString()
            };

            string json = JsonUtility.ToJson(report, true);
            
            // Webhook ConfigSystem'den okunuyor
            var config = _configSystem.GetConfig<TelemetryConfig>();
            string webhookUrl = config != null ? config.DiscordWebhookUrl : "";
            
            if (string.IsNullOrEmpty(webhookUrl))
            {
                Debug.LogWarning("[TelemetryManager] Discord Webhook URL eksik. Rapor sadece diske yazılıyor.");
                SaveReportToDisk(report.ReportId, json);
                return;
            }

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                SaveReportToDisk(report.ReportId, json);
                return;
            }

            GameManager.Instance.StartCoroutine(SendDiscordWebhookRoutine(webhookUrl, report.ReportId, json));
        }

        private IEnumerator SendDiscordWebhookRoutine(string url, string reportId, string jsonPayload)
        {
            _isSending = true;

            // Discord Webhook formatına çevir
            string discordPayload = $"{{\"content\": \"**Yeni Hata Raporu ({Application.version})**\\n```json\\n{jsonPayload}\\n```\"}}";

            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(discordPayload);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    // Gönderim başarılıysa diskteki kopyasını sil
                    DeleteReportFromDisk(reportId);
                }
                else
                {
                    // Gönderim başarısızsa kaydet (belki de internet koptu)
                    SaveReportToDisk(reportId, jsonPayload);
                }
            }
            
            _isSending = false;
        }

        private void SaveReportToDisk(string reportId, string jsonPayload)
        {
            try
            {
                string path = Path.Combine(_telemetryDirectory, $"{reportId}.json");
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, jsonPayload);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[TelemetryManager] Rapor diske yazılamadı: {ex.Message}");
            }
        }

        private void DeleteReportFromDisk(string reportId)
        {
            try
            {
                string path = Path.Combine(_telemetryDirectory, $"{reportId}.json");
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch { /* Ignore delete errors */ }
        }

        private IEnumerator RetryOfflineReportsRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(_retryInterval);
            
            while (true)
            {
                yield return wait;

                if (_isSending || Application.internetReachability == NetworkReachability.NotReachable)
                    continue;

                var config = _configSystem.GetConfig<TelemetryConfig>();
                string webhookUrl = config != null ? config.DiscordWebhookUrl : "";
                
                if (string.IsNullOrEmpty(webhookUrl))
                    continue;

                string[] files = null;
                try
                {
                    files = Directory.GetFiles(_telemetryDirectory, "*.json");
                }
                catch
                {
                    // Hata dinleyicisini sonsuz döngüye sokmamak için log atlaması yapılabilir
                }

                if (files != null)
                {
                    foreach (string file in files)
                    {
                        string reportId = Path.GetFileNameWithoutExtension(file);
                        string jsonPayload = "";
                        try
                        {
                            jsonPayload = File.ReadAllText(file);
                        }
                        catch { continue; }
                        
                        if (!string.IsNullOrEmpty(jsonPayload))
                        {
                            // Sırayla gönder (Spam engellemek için)
                            yield return GameManager.Instance.StartCoroutine(SendDiscordWebhookRoutine(webhookUrl, reportId, jsonPayload));
                            yield return new WaitForSeconds(1f); 
                        }
                    }
                }
            }
        }
    }
}
