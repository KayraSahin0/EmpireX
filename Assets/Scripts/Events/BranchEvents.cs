namespace EmpireX.Events
{
    public struct BranchCreated { public string BranchId; public string CompanyId; }
    public struct BranchUpgraded { public string BranchId; public int NewLevel; }
    public struct BranchActionFailed { public string Reason; }
}
