using Application.DataServices.Interfaces;
using Domain.Entities;
using Persistence.Caching;
using Persistence.DbServices.Interfaces;

namespace Application.DataServices.Services
{
    public class ShiftRemarkDataService(
        IBaseCacheService<ShiftRemark> cacheService,
        IBaseDbService<ShiftRemark> dbService) : IDataService<ShiftRemark>
    {
        public async Task<ShiftRemark> AddSingleAsync(ShiftRemark model)
        {
            await dbService.InsertAsync(model);
            var data = await dbService.GetByIdBinaryAsync(model.IdBinary);
            cacheService.Items.Add(data);
            return data;
        }

        public bool CheckExists(string key) => cacheService.Items.Any(e => e.Remark == key);
        public ShiftRemark GetSingle(string key) => cacheService.Items.First(e => e.Remark == key);
        public async Task InitCacheAsync() => cacheService.Items = await dbService.GetAllAsync();
    }
}
