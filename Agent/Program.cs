using Agent;
using Application.Pipelines;
using Domain.Settings;
using Infrastructure.Scraper;
using Infrastructure.Scraper.Bots;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        var startup = new Startup();
        startup.ConfigureServices(services);
    })
    .Build();

var loginPipeline = host.Services.GetService<LoginPipeline>();
await loginPipeline.RunLoginSession();