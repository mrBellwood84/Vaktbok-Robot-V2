using Common.Logging;

namespace Domain.Misc;

public struct ShiftWeekReport
{
    public ShiftWeekReport() { }

    public int NewEmployees { get; set; } = 0;
    public int NewWorkdays { get; set; } = 0;
    public int NewShiftCodes { get; set; } = 0;
    public int NewShifts { get; set; } = 0;
    public int UpdatedShifts { get; set; } = 0;

    private int AllNewEmployees { get; set; } = 0;
    private int AllNewWorkdays { get; set; } = 0;
    private int AllNewShiftCodes { get; set; } = 0;
    private int AllNewShifts { get; set; } = 0;
    private int AllUpdatedShifts { get; set; } = 0;

    public void ReportNewEntities()
    {
        if (NewEmployees > 0)
            AppLogger.LogAdd($"New Employees added: {NewEmployees}");
        if (NewWorkdays > 0)
            AppLogger.LogAdd($"New Workdays added: {NewWorkdays}");
        if (NewShiftCodes > 0)
            AppLogger.LogAdd($"New Shifts added: {NewShiftCodes}");
        if (NewShifts > 0)
            AppLogger.LogAdd($"New Shifts added: {NewShifts}");
        if (UpdatedShifts > 0)
            AppLogger.LogAdd($"UpdatedShifts added: {UpdatedShifts}");
    }

    public void ReportAllEntities()
    {
        AppLogger.LogInfo("All added entities:");
        if (AllNewEmployees > 0)
            AppLogger.LogAdd($"Employees added: {AllNewEmployees}");
        if (AllNewWorkdays > 0)
            AppLogger.LogAdd($"Workdays added: {AllNewWorkdays}");
        if (AllNewShiftCodes > 0)
            AppLogger.LogAdd($"Shifts added: {AllNewShiftCodes}");
        if (AllNewShifts > 0)
            AppLogger.LogAdd($"Shifts added: {AllNewShifts}");
        if (AllUpdatedShifts > 0)
            AppLogger.LogAdd($"Shifts updated: {AllUpdatedShifts}");
    }

    public void Clear()
    {
        AllNewEmployees += NewEmployees;
        AllNewWorkdays += NewWorkdays;
        AllNewShiftCodes += NewShiftCodes;
        AllNewShifts += NewShifts;
        AllUpdatedShifts += UpdatedShifts;
        
        NewEmployees = 0;
        NewWorkdays = 0;
        NewShiftCodes = 0;
        NewShifts = 0;
        UpdatedShifts = 0;
    }
    
    
}   

