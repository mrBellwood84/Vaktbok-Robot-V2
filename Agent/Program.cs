using Agent;
using Application.Pipelines;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        var startup = new Startup();
        startup.ConfigureServices(services);
    })
    .Build();

var collectPipeline = host.Services.GetRequiredService<CollectShiftDataPipeline>();
await collectPipeline.RunPipelineAsync();