using Agent;
using Application.Pipelines;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Create startup and host

var pathString = @"E:/TestFolder";
var fileString = "test.txt";
var fullPath = Path.Combine(pathString, fileString);

var text = "Hello world";

Directory.CreateDirectory(pathString);
File.WriteAllText(fullPath, text);

/*
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
    
    */