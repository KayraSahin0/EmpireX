namespace EmpireX.Events
{
    public struct MissionProgressed { public string MissionId; public double CurrentProgress; public double Target; }
    public struct MissionCompleted { public string MissionId; }
    public struct MissionRewardClaimed { public string MissionId; public double RewardAmount; }
    public struct MissionsRefreshed { }
}
