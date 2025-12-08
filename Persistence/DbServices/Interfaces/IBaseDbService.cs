namespace Persistence.DbServices.Interfaces
{
    public interface IBaseDbService<TModel>
    {
        Task<List<TModel>> GetAllAsync();
        Task<TModel> GetByIdBinaryAsync(byte[] id);
        Task CreateAsync(TModel model);
        Task CreateBulkAsync(IEnumerable<TModel> models);
    }
}