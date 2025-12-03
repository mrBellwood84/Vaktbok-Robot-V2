using Application.DataServices.Interfaces;
using Domain.Entities;
using Infrastructure.Caching;
using Infrastructure.Persistence.Interfaces;

namespace Application.DataServices.Services
{
    public class WorkdayDataService(
        IBaseCacheService<Workday> cacheService,
        IBaseDbService<Workday> dbService) : IDataService<Workday>
    {
        public async Task<Workday> AddSingleAsync(Workday model)
        {
            await dbService.InsertAsync(model);
            var data = await dbService.GetByIdBinaryAsync(model.IdBinary);
            cacheService.Items.Add(data);
            return data;
        }

        public bool CheckExists(string key) => cacheService.Items.Any(e => e.Key == key);
        public Workday GetSingle(string key) => cacheService.Items.First(e => e.Key == key);
        public async Task InitCacheAsync() => cacheService.Items = await dbService.GetAllAsync();
    }
}
