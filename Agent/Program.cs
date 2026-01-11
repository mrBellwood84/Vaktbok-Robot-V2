using Agent;
using Application.DataServices.Interfaces;
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

// get pipelines
var loginPipeline = provider.GetRequiredService<LoginPipeline>();
var collectorPipeline = provider.GetRequiredService<CollectShiftDataPipeline>();
var pdfPipeline = provider.GetRequiredService<WeekyPdfPrintPipeline>();


// display menu and run selected pipeline
var option = StartMenu.Print();
switch (option)
{
    case 1:
        await loginPipeline.RunLoginSession();
        break;
    case 2:
        await collectorPipeline.RunPipelineAsync();
        break;
    case 3:
        await pdfPipeline.RunPipeLineAsync();
        break;
    default:
        Console.WriteLine("Ingen gyldig valg ble gjort. Avslutter programmet.");
        break;
}

