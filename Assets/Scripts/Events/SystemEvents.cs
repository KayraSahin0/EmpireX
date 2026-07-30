using System;
using UnityEngine;

namespace EmpireX.Events
{
    public enum ErrorSeverity
    {
        Info,
        Warning,
        Error,
        Critical
    }

    public struct SystemErrorEvent
    {
        public string ErrorMessage;
        public string StackTrace;
        public LogType LogType;
        public ErrorSeverity Severity;
    }

    public struct ShowSystemPopupEvent
    {
        public string Title;
        public string Message;
        public ErrorSeverity Severity;
        
        public string Button1Text;
        public Action Button1Callback;
        
        public string Button2Text;
        public Action Button2Callback;
        
        // Sadece teknik ortamda gösterilecek detaylar
        public string TechnicalDetails;
    }
}
