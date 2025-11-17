using Agent;
using Domain.Settings;
using Infrastructure.Scraper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        var startup = new Startup();
        startup.ConfigureServices(services);
    })
    .Build();

var browserHost = host.Services.GetRequiredService<BrowserHost>();
var urls = host.Services.GetRequiredService<Urls>();

await browserHost.StartBrowserSession();
await browserHost.Page.GotoAsync(urls.Entry);
    
Console.WriteLine("Press any key to exit");
Console.ReadLine();

await browserHost.CloseBrowserSessionAsync();
