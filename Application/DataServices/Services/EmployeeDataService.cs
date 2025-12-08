using Domain.Entities;
using Persistence.Caching;
using Persistence.DbServices.Interfaces;

namespace Application.DataServices.Services;

public class EmployeeDataService(
    IBaseCacheService<Employee> cacheService,
    IBaseDbService<Employee> dbService) 
    : BaseDataService<Employee>(cacheService, dbService)
{ }