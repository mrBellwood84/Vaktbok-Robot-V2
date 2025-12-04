using Domain.Entities;
using Domain.Settings;

namespace Persistence.DbServices.Services
{
    public class ShiftRemarkDbService : BaseDbService<ShiftRemark>
    {
        public ShiftRemarkDbService(ConnectionStrings connectionStrings) 
            : base(connectionStrings)
        {
            QueryAll = "SELECT * FROM ShiftRemark;";
            QueryByIdBinary = @"
                SELECT * FROM ShiftRemark
                WHERE IdBinary = @IdBinary;";

            Insert = @"
                INSERT INTO ShiftRemark (IdBinary, Remark)
                VALUES (@IdBinary, @Remark);";
        }
    }
}
