using Domain.Entities;
using Persistence.Caching;
using Persistence.DbServices.Interfaces;

namespace Application.DataServices.Services;

public class ShiftCodeDataService(
    IBaseCacheService<ShiftCode> cacheService,
    IBaseDbService<ShiftCode> dbService) 
    : BaseDataService<ShiftCode>(cacheService, dbService)
{ }