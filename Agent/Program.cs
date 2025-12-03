using Agent;
using Microsoft.Extensions.Hosting;

// Create startup and host

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

// var collectPipeline = host.Services.GetRequiredService<CollectShiftDataPipeline>();
// await collectPipeline.RunPipelineAsync();
