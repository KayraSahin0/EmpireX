namespace EmpireX.Events
{
    public struct TickStarted { public long Tick; }
    public struct TickCompleted { public long Tick; }
    
    public struct DayStarted { public int Day; }
    public struct DayEnded { public int Day; }
    
    public struct WeekStarted { public int Week; }
    public struct WeekEnded { public int Week; }
    
    public struct MonthStarted { public int Month; }
    public struct MonthEnded { public int Month; }
    
    public struct YearStarted { public int Year; }
    public struct YearEnded { public int Year; }
}
