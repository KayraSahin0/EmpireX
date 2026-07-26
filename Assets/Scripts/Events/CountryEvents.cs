namespace EmpireX.Events
{
    public struct CountryDataCreated { public string CountryId; }
    public struct CountryEconomyChanged { public string CountryId; public float NewInflation; public float NewTaxRate; }
}
