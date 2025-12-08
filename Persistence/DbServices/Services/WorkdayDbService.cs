using Domain.Entities;
using Domain.Settings;

namespace Persistence.DbServices.Services
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
                INSERT INTO Workday (IdBinary, Day, Week, Year, Date)
                VALUES (@IdBinary, @Day, @Week, @Year, @Date);";
        }
    }
}
