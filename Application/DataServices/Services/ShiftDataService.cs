using Application.DataServices.Interfaces;
using Domain.Entities;
using Persistence.Caching;
using Persistence.DbServices.Interfaces;

namespace Application.DataServices.Services
{
    public class ShiftDataService(
        IBaseCacheService<Shift> cacheService,
        IBaseDbService<Shift> dbService) : IShiftDataService
    {
        public async Task<Shift> AddSingleAsync(Shift model)
        {
            await dbService.InsertAsync(model);
            var data = await dbService.GetByIdBinaryAsync(model.IdBinary);
            cacheService.Items.Add(data);
            return data;
        }

        public bool CheckExists(byte[] employeeId, byte[] WorkdayId) =>
            cacheService.Items.Any(e =>
                e.EmployeeId.SequenceEqual(employeeId) &&
                e.WorkdayId.SequenceEqual(WorkdayId));

        public Shift GetSingle(byte[] employeeId, byte[] WorkdayId) =>
            cacheService.Items
            .Where(e =>
                e.EmployeeId.SequenceEqual(employeeId) &&
                e.WorkdayId.SequenceEqual(WorkdayId))
            .OrderByDescending(e => e.CreatedAt)
            .First()!;

        public async Task InitCacheAsync() => cacheService.Items = await dbService.GetAllAsync();
    }
}
