using Dapper;
using Domain.Entities;
using Domain.Settings;
using Persistence.DbServices.Interfaces;

namespace Persistence.DbServices.Services;

public class ShiftNoRemarkDbService : BaseDbService<ShiftNoRemark>, IShiftNoRemarkDbService
{
    internal string UpdateShift;

    public ShiftNoRemarkDbService(ConnectionStrings connectionStrings)
        : base(connectionStrings)
    {
        QueryAll = "Call GetNoRemarkShifts";
        UpdateShift = @"
            UPDATE Shift SET ShiftRemarkId = @ShiftRemarkId
            WHERE Id = @ShiftId";
    }

    public async Task SetShiftRemarkAsync(string ShiftId, string ShiftRemarkId)
    {
        await using var connection = await CreateConnectionAsync();
        await connection.ExecuteAsync(UpdateShift, new { ShiftId, ShiftRemarkId });
    }
}
