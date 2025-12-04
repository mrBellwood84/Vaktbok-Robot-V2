using Domain.Entities;
using Domain.Settings;

namespace Persistence.DbServices.Services
{
    public class ShiftCodeDbService : BaseDbService<ShiftCode>
    {
        public ShiftCodeDbService(ConnectionStrings connectionStrings) : base(connectionStrings)
        {
            QueryAll = "SELECT * FROM ShiftCode;";
            QueryByIdBinary = @"
                SELECT * FROM ShiftCode
                WHERE IdBinary = @IdBinary;";
            Insert = @"
                INSERT INTO ShiftCode (IdBinary, Code)
                VALUES (@IdBinary, @Code);";
        }
    }
}
