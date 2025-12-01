namespace Domain.SourceModels;

public class SourceEmployeeWeek
{
    public string EmployeeName { get; set; }
    public int WeekNumber { get; set; }
    public int Year { get; set; }

    public List<SourceShiftEntry> Shifts { get; set; } = [];
}