using Microsoft.Extensions.DependencyInjection;

namespace Agent.Extensions;

public static class DataServicesExtension
{
    /// <summary>
    /// Add all data services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        return services;
    }
    
}