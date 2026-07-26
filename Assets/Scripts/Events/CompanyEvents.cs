namespace EmpireX.Events
{
    public struct CompanyCreated { public string CompanyId; }
    public struct CompanyCreationFailed { public string Reason; }
    public struct CompanyUpgraded { public string CompanyId; public int NewLevel; }
}
