using Domain.Entities;

namespace Application.DataServices.Interfaces
{
    public interface IShiftDataService
    {
        Task<Shift> AddSingleAsync(Shift model);
        bool CheckExists(byte[] employeeId, byte[] WorkdayId);
        Shift GetSingle(byte[] employeeId, byte[] WorkdayId);
        Task InitCacheAsync();
    }
}