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
            QueryByIdBinary = @"
                SELECT * FROM Employee
                WHERE IdBinary = @IdBinary;";

            Insert = @"
                INSERT INTO Employee (IdBinary, Name)
                VALUES (@IdBinary, @Name);";
        }
    }
}
