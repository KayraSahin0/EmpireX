using UnityEngine;

namespace EmpireX.Core
{
    [CreateAssetMenu(fileName = "TelemetryConfig", menuName = "EmpireX/Config/TelemetryConfig")]
    public class TelemetryConfig : ScriptableObject
    {
        [Header("Discord Entegrasyonu")]
        [Tooltip("Hata raporlarının gönderileceği Discord Webhook URL'si")]
        public string DiscordWebhookUrl;
    }
}
