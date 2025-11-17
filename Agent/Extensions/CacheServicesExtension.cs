using Microsoft.Extensions.DependencyInjection;

namespace Agent.Extensions;

public static class CacheServicesExtension
{
    /// <summary>
    /// Add all cache services
    /// </summary>
    public static IServiceCollection AddCacheServices(this IServiceCollection services)
    {
        return services;
    }
}