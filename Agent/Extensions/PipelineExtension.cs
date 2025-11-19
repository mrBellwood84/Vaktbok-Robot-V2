using Application.Pipelines;
using Infrastructure.Scraper.Bots;
using Microsoft.Extensions.DependencyInjection;

namespace Agent.Extensions;

public static class PipelineExtension
{
    /// <summary>
    /// Add all pipelines
    /// </summary>
    public static IServiceCollection AddPipelines(this IServiceCollection services)
    {
        services.AddSingleton<LoginPipeline>();
        services.AddSingleton<CollectShiftDataPipeline>();
        
        return services;
    }
}