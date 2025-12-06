namespace Domain.SourceModels;

public struct SourceShiftEntry
{
    public DateTime ShiftDate { get; set; }
    public int Day { get; set; }
    public int WeekNumber { get; set; }
    public int Year { get; set; }

    public string CellContent { get; set; }
}