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
            QueryById = @"
                SELECT * FROM Workday
                WHERE Id = @Id;";

            Insert = @"
                INSERT INTO Workday (Id, Day, Week, Year, Date)
                VALUES (@Id, @Day, @Week, @Year, @Date);";
        }
    }
}
