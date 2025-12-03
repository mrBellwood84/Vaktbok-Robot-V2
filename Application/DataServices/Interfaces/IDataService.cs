namespace Application.DataServices.Interfaces
{
    public interface IDataService<TModel>
    {
        Task<TModel> AddSingleAsync(TModel model);
        bool CheckExists(string key);
        TModel GetSingle(string key);
        Task InitCacheAsync();
    }
}
