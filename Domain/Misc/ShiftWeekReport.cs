namespace Domain.Misc;

public struct ShiftWeekReport
{
    public ShiftWeekReport() { }

    public int NewEmployees { get; set; } = 0;
    public int NewWorkdays { get; set; } = 0;
    public int NewShiftCodes { get; set; } = 0;
    public int NewShifts { get; set; } = 0;
    public int UpdatedShifts { get; set; } = 0;

    public int AllNewEmployees { get; set; } = 0;
    public int AllNewWorkdays { get; set; } = 0;
    public int AllNewShiftCodes { get; set; } = 0;
    public int AllNewShifts { get; set; } = 0;
    public int AllUpdatedShifts { get; set; } = 0;

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
