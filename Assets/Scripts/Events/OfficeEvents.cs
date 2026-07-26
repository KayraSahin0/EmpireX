namespace EmpireX.Events
{
    public struct OfficeCreated { public string OfficeId; }
    public struct OfficeUpgraded { public string OfficeId; public int NewLevel; }
    public struct OfficeCustomized { public string OfficeId; }
    public struct OfficeActionFailed { public string Reason; }
}
