using EmpireX.Data;

namespace EmpireX.Events
{
    public struct SaveStarted { public string SlotId; }
    public struct SaveCompleted { public string SlotId; }
    public struct SaveFailed { public string SlotId; public string Error; }
    
    public struct LoadStarted { public string SlotId; }
    public struct LoadCompleted { public string SlotId; public SaveData Data; }
    public struct LoadFailed { public string SlotId; public string Error; }
    
    public struct AutoSaveStarted { }
    public struct AutoSaveCompleted { }
}
