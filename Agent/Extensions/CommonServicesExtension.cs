using Microsoft.Extensions.DependencyInjection;

namespace Agent.Extensions;

public static class CommonServicesExtension
{
    /// <summary>
    /// Add all common servies
    /// </summary>
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        return services;
    }
}