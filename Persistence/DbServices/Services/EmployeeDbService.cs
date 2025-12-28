using Domain.Entities;
using Domain.Settings;

namespace Persistence.DbServices.Services;

public class EmployeeDbService : BaseDbService<Employee>
{
    public EmployeeDbService(ConnectionStrings connectionStrings) 
        : base(connectionStrings)
    {
        QueryAll = "SELECT * FROM Employee;";
        QueryById = @"
            SELECT * FROM Employee
            WHERE Id = @Id;";
    
        Insert = @"
            INSERT INTO Employee (Id, Name)
            VALUES (@Id, @Name);";
    }
}
