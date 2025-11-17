using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Agent.Extensions;

public static class CommonServicesExtension
{
    /// <summary>
    /// Add all common servies
    /// </summary>
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        // add logging service
        services.AddLogging(config =>
        {
            config.AddConsole();
            config.SetMinimumLevel(LogLevel.Information);
        });
        
        return services;
    } 
}