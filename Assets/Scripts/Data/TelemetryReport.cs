using System;

namespace EmpireX.Data
{
    [Serializable]
    public class TelemetryReport
    {
        public string ReportId;
        public string ErrorType;
        public string ErrorMessage;
        public string StackTrace;
        public string OccurredAt;
        
        public string AppVersion;
        public string UnityVersion;
        public string OperatingSystem;
        public string DeviceModel;
        
        public string Severity;
    }
}
