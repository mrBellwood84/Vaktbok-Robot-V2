namespace Domain.Entities;

public class Employee
{
    public string Id { get; init; } =  Guid.NewGuid().ToString();
    public string FullName { get; init; }
}