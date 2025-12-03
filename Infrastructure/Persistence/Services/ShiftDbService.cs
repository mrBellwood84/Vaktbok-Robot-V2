using Domain.Entities;
using Domain.Settings;

namespace Infrastructure.Persistence.Services
{
    public class ShiftDbService : BaseDbService<Shift>
    {
        public ShiftDbService(ConnectionStrings connectionStrings) 
            : base(connectionStrings)
        {
            QueryAll = "SELECT * FROM Shift;";
            QueryByIdBinary = @"
                SELECT * FROM Shift
                WHERE IdBinary = @IdBinary;";

            Insert = @"
                INSERT INTO Shift (IdBinary, EmployeeId, WorkdayId, ShiftCodeId, ShiftRemarkId, StartTime, EndTime)
                VALUES (@IdBinary, @EmployeeId, @WorkdayId, @ShiftCodeId, @ShiftRemarkId, @StartTime, @EndTime);";
        }
    }
}
