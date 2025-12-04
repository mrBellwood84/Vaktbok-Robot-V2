namespace Persistence.Caching
{
    public interface IBaseCacheService<TModel>
    {
        List<TModel> Items { get; set; }
    }
}