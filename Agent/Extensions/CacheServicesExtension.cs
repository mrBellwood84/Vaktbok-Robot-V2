using Domain.Entities;
using Persistence.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace Agent.Extensions;

public static class CacheServicesExtension
{
    /// <summary>
    /// Add all cache services
    /// </summary>
    public static IServiceCollection AddCacheServices(this IServiceCollection services)
    {
        services.AddSingleton<IBaseCacheService<Employee>, EmployeeCacheService>();
        services.AddSingleton<IBaseCacheService<Shift>, ShiftCacheService>();
        services.AddSingleton<IBaseCacheService<ShiftCode>, ShiftCodeCacheService>();
        services.AddSingleton<IBaseCacheService<ShiftRemark>, ShiftRemarkCacheService>();
        services.AddSingleton<IBaseCacheService<Workday>, WorkdayCacheService>();

        return services;
    }
}