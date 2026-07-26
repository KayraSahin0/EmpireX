namespace EmpireX.Events
{
    public struct ResearchStarted { public string ResearchId; }
    public struct ResearchCompleted { public string ResearchId; }
    public struct ResearchActionFailed { public string Reason; }
}
