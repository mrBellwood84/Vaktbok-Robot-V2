using Application.DataServices.Interfaces;
using Domain.Entities;
using Persistence.Caching;
using Persistence.DbServices.Interfaces;

namespace Application.DataServices.Services
{
    public class ShiftcodeDataService(
        IBaseCacheService<ShiftCode> cacheService,
        IBaseDbService<ShiftCode> dbService) : IDataService<ShiftCode>
    {
        public async Task<ShiftCode> AddSingleAsync(ShiftCode model)
        {
            await dbService.InsertAsync(model);
            var data = await dbService.GetByIdBinaryAsync(model.IdBinary);
            cacheService.Items.Add(data);
            return data;
        }

        public bool CheckExists(string key) => cacheService.Items.Any(e => e.Code == key);
        public ShiftCode GetSingle(string key) => cacheService.Items.First(e => e.Code == key);
        public async Task InitCacheAsync() => cacheService.Items = await dbService.GetAllAsync();
    }
}
