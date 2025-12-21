using Agent;
using Application.Pipelines;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var startup = new Startup();
using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        startup.ConfigureServices(services);
    })
    .Build();

// create provider
var provider = host.Services;

// initialize infrastructure
await startup.InitializeInfrastructure(provider);

var loginPipeline = provider.GetRequiredService<LoginPipeline>();
var collectorPipeline = provider.GetRequiredService<CollectShiftDataPipeline>();

await collectorPipeline.RunPipelineAsync();
