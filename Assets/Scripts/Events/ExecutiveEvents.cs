namespace EmpireX.Events
{
    public struct ExecutiveHired { public string ExecutiveId; public string Role; }
    public struct ExecutiveFired { public string ExecutiveId; }
    public struct ExecutiveActionFailed { public string Reason; }
}
