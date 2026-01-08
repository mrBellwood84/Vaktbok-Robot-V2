using Domain.Entities;
using Domain.Settings;

namespace Persistence.DbServices.Services;

public class ShiftCodeDbService : BaseDbService<ShiftCode>
{
    public ShiftCodeDbService(ConnectionStrings connectionStrings) : base(connectionStrings)
    {
        QueryAll = "SELECT * FROM ShiftCode;";
        QueryById = @"
                SELECT * FROM ShiftCode
                WHERE Id = @Id;";
        Insert = @"
                INSERT INTO ShiftCode (Id, Code)
                VALUES (@Id, @Code);";
    }
}
