namespace Domain.SourceModels;

public class SourceShiftEntry
{
    public string Year { get; set; }
    public string Month { get; set; }
    public string Date { get; set; }
    public string Day { get; set; }
    
    public string ShiftCode { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
}