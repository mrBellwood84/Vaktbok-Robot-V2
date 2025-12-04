namespace Persistence.Caching
{
    /// <summary>
    /// Provides a base implementation for a cache service that stores a collection of items of a specified type.
    /// </summary>
    /// <typeparam name="TModel">The type of items to be stored in the cache.</typeparam>
    public class BaseCacheService<TModel> : IBaseCacheService<TModel>
    {
        /// <summary>
        /// Gets or sets the collection of items represented by the model.
        /// </summary>
        public List<TModel> Items { get; set; } = [];
    }
}
