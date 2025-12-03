using Domain.Entities;
using Domain.Settings;

namespace Infrastructure.Persistence.Services
{
    public class ShiftCodeDbService : BaseDbService<ShiftCode>
    {
        public ShiftCodeDbService(ConnectionStrings connectionStrings) : base(connectionStrings)
        {
            QueryAll = "SELECT * FROM ShiftCode;";
            Insert = @"
                INSERT INTO ShiftCode (IdBinary, Code)
                VALUES (@IdBinary, @Code);";
        }
    }
}
