using System;
using UnityEngine;
using EmpireX.Events;

namespace EmpireX.Core
{
    public class ErrorManager : BaseManager
    {
        // Sonsuz döngüleri engellemek için
        private bool _isProcessingError = false;

        public ErrorManager(IEventBus eventBus) : base(eventBus) { }

        public override void Initialize()
        {
            Application.logMessageReceived += OnLogMessageReceived;
        }

        public override void Dispose()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            // Eğer halihazırda bir hatayı işliyorsak, sonsuz döngüyü engelle
            if (_isProcessingError) return;

            // Sadece Error veya Exception olanları dikkate alıyoruz. (Warning ve Log'ları geçiyoruz)
            if (type != LogType.Error && type != LogType.Exception)
                return;
                
            // Telemetry veya kendi loglarımızdan gelen hataları engelle
            if (condition.Contains("[TelemetryManager]") || condition.Contains("[ErrorManager]"))
                return;

            try
            {
                _isProcessingError = true;

                // Hatanın kritik olup olmadığına karar ver
                ErrorSeverity severity = DetermineSeverity(condition, type);

                SystemErrorEvent errorEvent = new SystemErrorEvent
                {
                    ErrorMessage = condition,
                    StackTrace = stackTrace,
                    LogType = type,
                    Severity = severity
                };

                // UI'ı uyar ve Telemetriyi tetikle
                _eventBus.Publish(errorEvent);
            }
            finally
            {
                _isProcessingError = false;
            }
        }

        private ErrorSeverity DetermineSeverity(string condition, LogType type)
        {
            // Exception her zaman kritiktir, çünkü oyun akışını (örneğin NullReference) kırar.
            if (type == LogType.Exception)
                return ErrorSeverity.Critical;

            // Error'lar kritik olmayan hatalardır (eksik dosya, yükleme sorunu vs.)
            // İleride özel şartlara göre Error olanlar da kritik sayılabilir.
            return ErrorSeverity.Error;
        }
    }
}
