using Domain.Entities;
using Domain.Settings;

namespace Infrastructure.Persistence.Services
{
    public class WorkdayDbService : BaseDbService<Workday>
    {
        public WorkdayDbService(ConnectionStrings connectionStrings) 
            : base(connectionStrings)
        {
            QueryAll = "SELECT * FROM Workday;";
            QueryByIdBinary = @"
                SELECT * FROM Workday
                WHERE IdBinary = @IdBinary;";

            Insert = @"
                INSERT INTO Workday (IdBinary, Day, Week, Date, Month, Year)
                VALUES (@IdBinary, @Day, @Week, @Date, @Month, @Year);";
        }
    }
}
