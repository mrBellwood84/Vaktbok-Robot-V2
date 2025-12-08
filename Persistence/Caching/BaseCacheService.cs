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
        public List<TModel> Items { get; } = [];

        /// <summary>
        /// Adds a single item to the collection.
        /// </summary>
        /// <param name="model">The item to add to the collection. Cannot be null.</param>
        public void AddSingleItem(TModel model)
        {
            Items.Add(model);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the current list.
        /// </summary>
        /// <param name="items">The collection of items to add to the list. Cannot be null.</param>
        public void AddRange(IEnumerable<TModel> items)
        {
            Items.AddRange(items);
        }

        /// <summary>
        /// Removes all items from the buffer.
        /// </summary>
        /// <remarks>After calling this method, the buffer will be empty. This operation does not raise an
        /// exception if the buffer is already empty.</remarks>
        public void ClearBuffer()
        {
            Items.Clear();
        }
    }
}
