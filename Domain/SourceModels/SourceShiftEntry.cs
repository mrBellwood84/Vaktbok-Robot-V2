namespace Domain.SourceModels;

public struct SourceShiftEntry
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Date { get; set; }
    public int Day { get; set; }
    public int WeekNumber { get; set; }

    public string CellContent { get; set; }

}