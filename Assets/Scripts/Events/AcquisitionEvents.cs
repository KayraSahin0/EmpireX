namespace EmpireX.Events
{
    public struct CompanyBought { public string CompanyId; public double Price; }
    public struct CompanySold { public string CompanyId; public double Price; }
    public struct CompaniesMerged { public string MainCompanyId; public string AbsorbedCompanyId; }
}
