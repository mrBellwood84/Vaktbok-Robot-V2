using Domain.Entities;
using Domain.Settings;

namespace Infrastructure.Persistence.Services
{
    public class EmployeeDbService : BaseDbService<Employee>
    {
        public EmployeeDbService(ConnectionStrings connectionStrings) 
            : base(connectionStrings)
        {
            QueryAll = "SELECT * FROM Employee;";
            Insert = @"
                INSERT INTO Employee (IdBinary, Name)
                VALUES (@IdBinary, @Name);";
        }
    }
}
