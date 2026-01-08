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

var loginPipeline = provider.GetRequiredService<LoginPipeline>();
var collectorPipeline = provider.GetRequiredService<CollectShiftDataPipeline>();

var noshiftService = provider.GetRequiredService<IShiftNoRemarkDataService>();

// await loginPipeline.RunLoginSession();
await collectorPipeline.RunPipelineAsync();

/*
await noshiftService.LoadData();
Console.WriteLine($"No Remark Shifts Count: {noshiftService.Data.Count}");
Console.WriteLine($"No Remark Dates Count: {noshiftService.AllDates.Count}");

foreach (var date in noshiftService.AllDates)
{
    Console.Write(date.ToLongDateString());
    Console.Write($"| {date.Month}\n");

*/