namespace EmpireX.Events
{
    public struct EmployeeHired { public string EmployeeId; public string CompanyId; }
    public struct EmployeePromoted { public string EmployeeId; public int NewLevel; }
    public struct EmployeeTrained { public string EmployeeId; }
    public struct EmployeeActionFailed { public string Reason; }
}
