using EmpireX.Data;

namespace EmpireX.Events
{
    public struct EconomyUpdated { public EconomyData EconomyData; }
    public struct TransactionOccurred 
    { 
        public double Amount; 
        public string Reason;
        public bool IsRevenue;
    }
}
