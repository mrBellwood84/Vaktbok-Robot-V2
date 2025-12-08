using Domain.Entities;
using Persistence.Caching;
using Persistence.DbServices.Interfaces;

namespace Application.DataServices.Services;

public class ShiftRemarkDataService(
    IBaseCacheService<ShiftRemark> cacheService,
    IBaseDbService<ShiftRemark> dbService) 
    : BaseDataService<ShiftRemark>(cacheService, dbService)
{
}
