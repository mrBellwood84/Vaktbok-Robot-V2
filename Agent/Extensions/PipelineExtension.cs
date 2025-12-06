using Application.Pipelines;
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