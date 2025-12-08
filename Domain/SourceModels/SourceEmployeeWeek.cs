namespace Domain.SourceModels;

public struct SourceEmployeeWeek
{
    public SourceEmployeeWeek() { }
    public string EmployeeName { get; set; }
    public List<SourceShiftEntry> Shifts { get; set; } = [];
}