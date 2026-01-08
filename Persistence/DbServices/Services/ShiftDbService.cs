using Domain.Entities;
using Domain.Settings;

namespace Persistence.DbServices.Services;

public class ShiftDbService : BaseDbService<Shift>
{
    public ShiftDbService(ConnectionStrings connectionStrings) 
        : base(connectionStrings)
    {
        QueryAll = "SELECT * FROM Shift;";
        QueryById = @"
                SELECT * FROM Shift
                WHERE Id = @Id;";

        Insert = @"
                INSERT INTO Shift (Id, EmployeeId, WorkdayId, ShiftCodeId, FilePathId, Time)
                VALUES (@Id, @EmployeeId, @WorkdayId, @ShiftCodeId, @FilePathId, @Time);";
    }
}
