namespace Persistence.DbServices.Interfaces
{
    public interface IBaseDbService<TModel>
    {
        Task<List<TModel>> GetAllAsync();
        Task<TModel> GetByIdAsync(Guid id);
        Task CreateAsync(TModel model);
        Task CreateBulkAsync(IEnumerable<TModel> models);
    }
}