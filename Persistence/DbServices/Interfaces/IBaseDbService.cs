namespace Persistence.DbServices.Interfaces
{
    public interface IBaseDbService<TModel>
    {
        Task<List<TModel>> GetAllAsync();
        Task<TModel> GetByIdBinaryAsync(byte[] id);
        Task InsertAsync(TModel model);
        Task InsertBulkAsync(IEnumerable<TModel> models);
    }
}