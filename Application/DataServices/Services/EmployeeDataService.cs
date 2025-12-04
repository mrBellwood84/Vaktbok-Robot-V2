using Application.DataServices.Interfaces;
using Domain.Entities;
using Persistence.Caching;
using Persistence.DbServices.Interfaces;

namespace Application.DataServices.Services
{
    public class EmployeeDataService(
        IBaseCacheService<Employee> cacheService,
        IBaseDbService<Employee> dbService) : IDataService<Employee>
    {
        public async Task<Employee> AddSingleAsync(Employee model)
        {
            await dbService.InsertAsync(model);
            var data = await dbService.GetByIdBinaryAsync(model.IdBinary);
            cacheService.Items.Add(data);
            return data;
        }

        public bool CheckExists(string key) => cacheService.Items.Any(e => e.Name == key);
        public Employee GetSingle(string key) => cacheService.Items.First(e => e.Name == key)!;
        public async Task InitCacheAsync() => cacheService.Items = await dbService.GetAllAsync();
        
    }
}
