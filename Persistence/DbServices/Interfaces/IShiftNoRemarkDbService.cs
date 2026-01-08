using Domain.Entities;

namespace Persistence.DbServices.Interfaces
{
    public interface IShiftNoRemarkDbService
    {
        Task<List<ShiftNoRemark>> GetAllAsync();
        Task SetShiftRemarkAsync(string ShiftId, string ShiftRemarkId);
    }
}