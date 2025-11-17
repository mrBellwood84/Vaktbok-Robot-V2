using Microsoft.Extensions.DependencyInjection;

namespace Agent.Extensions;

public static class DbServicesExtension
{
    /// <summary>
    /// Add all database services
    /// </summary>
    public static IServiceCollection AddDbServices(this IServiceCollection services)
    {
        return services;
    }
}