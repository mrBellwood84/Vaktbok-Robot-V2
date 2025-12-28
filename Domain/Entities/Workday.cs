using Domain.Interfaces;

namespace Domain.Entities;

public class Workday : IHasGuid
{
    private Guid _guid;
    
    public string Id
    {
        get => _guid.ToString(); 
        set => _guid = Guid.Parse(value); 
    }
    public Guid Guid
    {
        get => _guid; 
        set => _guid = value;
    }
    
    public short Day { get; set; }
    public short Week { get; set; }
    public short Year { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
}

