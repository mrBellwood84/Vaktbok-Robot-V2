namespace Infrastructure.Persistence.Interfaces
{
    public interface IBaseDbService<TModel>
    {
        Task<List<TModel>> GetAllAsync();
        Task InsertAsync(TModel model);
        Task InsertBulkAsync(IEnumerable<TModel> models);
    }
}