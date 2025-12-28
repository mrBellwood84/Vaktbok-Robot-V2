using Domain.Interfaces;

namespace Domain.Entities;

public class FilePath : IHasGuid
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
    
    public string Path { get; set; }
    public DateTime CreatedAt  { get; set; }
}