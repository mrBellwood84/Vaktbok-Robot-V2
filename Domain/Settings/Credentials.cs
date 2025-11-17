namespace Domain.Settings;

/// <summary>
/// Login credentials for IHelse and Gat login
/// </summary>
public class Credentials
{
    public string IHelseUser { get; init; }
    public string IHelsePassword { get; init; }
    
    public string GatUser { get; init; }
    public string GatPassword { get; init; }
}