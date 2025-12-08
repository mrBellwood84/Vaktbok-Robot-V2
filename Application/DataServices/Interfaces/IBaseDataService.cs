using Domain.Interfaces;

namespace Application.DataServices.Interfaces
{
    public interface IBaseDataService<TModel> where TModel : IHasIdBinary
    {
        List<TModel> Data { get; }

        Task AddRangeAsync(List<TModel> list);
        Task<TModel> AddSingleAsync(TModel model);
        Task InitCacheAsync();
    }
}