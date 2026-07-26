using EmpireX.Data;

namespace EmpireX.Events
{
    public struct HoldingCreated { public string HoldingId; public string Name; }
    public struct HoldingUpgraded { public int NewLevel; }
    public struct HoldingStatsUpdated { public HoldingData Data; }
    public struct HoldingActionFailed { public string Reason; }
}
