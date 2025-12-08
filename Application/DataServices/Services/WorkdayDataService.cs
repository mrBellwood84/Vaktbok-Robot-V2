using Domain.Entities;
using Persistence.Caching;
using Persistence.DbServices.Interfaces;

namespace Application.DataServices.Services;

public class WorkdayDataService(
    IBaseCacheService<Workday> cacheService,
    IBaseDbService<Workday> dbService) 
    : BaseDataService<Workday>(cacheService, dbService)
{ }
