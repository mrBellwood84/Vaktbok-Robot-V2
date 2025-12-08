
namespace Persistence.Caching
{
    public interface IBaseCacheService<TModel>
    {
        List<TModel> Items { get; }

        void AddRange(IEnumerable<TModel> items);
        void AddSingleItem(TModel model);
        void ClearBuffer();
    }
}