using Application.DataServices.Interfaces;
using Domain.Interfaces;
using Persistence.Caching;
using Persistence.DbServices.Interfaces;

namespace Application.DataServices
{
    public class BaseDataService<TModel>(
        IBaseCacheService<TModel> cacheService,
        IBaseDbService<TModel> dbService) : IBaseDataService<TModel> where TModel : IHasIdBinary
    {
        /// <summary>
        /// Gets the collection of data items currently held in the cache.
        /// </summary>
        public List<TModel> Data { get => cacheService.Items; }

        /// <summary>
        /// Asynchronously adds a single model to the data store and optionally updates the cache with the newly added
        /// item.
        /// </summary>
        /// <param name="model">The model instance to add to the data store. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added model instance as
        /// retrieved from the data store.</returns>
        public async Task<TModel> AddSingleAsync(TModel model)
        {
            await dbService.CreateAsync(model);
            var data = await dbService.GetByIdBinaryAsync(model.IdBinary);
            cacheService.AddSingleItem(data);
            return data;
        }

        /// <summary>
        /// Asynchronously adds a range of items to the data store and updates the cache with the latest data.
        /// </summary>
        /// <remarks>After the items are added, the cache is refreshed to reflect the current state of the
        /// data store. This method is not thread-safe; concurrent calls may result in inconsistent cache
        /// state.</remarks>
        /// <param name="list">The list of items to add to the data store. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        public async Task AddRangeAsync(List<TModel> list)
        {
            await dbService.CreateBulkAsync(list);
            var data = await dbService.GetAllAsync();
            cacheService.ClearBuffer();
            cacheService.AddRange(data);
        }

        /// <summary>
        /// Initializes the cache by asynchronously loading all items from the database.
        /// </summary>
        /// <remarks>This method populates the cache with items retrieved from the database. Subsequent
        /// accesses to the cache will reflect the loaded data. This method should be called before performing
        /// operations that depend on the cache contents.</remarks>
        /// <returns>A task that represents the asynchronous cache initialization operation.</returns>
        public async Task InitCacheAsync()
        {
            var data = await dbService.GetAllAsync();
            cacheService.ClearBuffer();
            cacheService.AddRange(data);
        }
    }
}
