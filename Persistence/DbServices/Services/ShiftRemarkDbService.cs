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
            QueryById = @"
                SELECT * FROM ShiftRemark
                WHERE Id = @Id;";

            Insert = @"
                INSERT INTO ShiftRemark (Id, Remark)
                VALUES (@Id, @Remark);";
        }
    }
}
