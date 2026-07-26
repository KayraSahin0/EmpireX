namespace EmpireX.Events
{
    public struct CompanyIPO { public string CompanyId; public double RaisedCapital; }
    public struct DividendsPaid { public string CompanyId; public double TotalAmount; }
    public struct StockPriceChanged { public string CompanyId; public double OldPrice; public double NewPrice; }
}
