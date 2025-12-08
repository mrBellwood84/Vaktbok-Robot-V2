using Domain.Entities;
using Persistence.Caching;
using Persistence.DbServices.Interfaces;

namespace Application.DataServices.Services;

public class ShiftDataService(
    IBaseCacheService<Shift> cacheService,
    IBaseDbService<Shift> dbService) 
    : BaseDataService<Shift>(cacheService, dbService)
{ }
