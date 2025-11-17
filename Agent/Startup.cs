using Agent.Extensions;
using Domain.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Agent;

public class Startup
{
    private IConfiguration Configuration { get; }

    public Startup()
    {
        var exePath = AppContext.BaseDirectory;
        Configuration = new ConfigurationBuilder()
            .SetBasePath(exePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.secret.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Set appsettings as DI items
        var browserSettings = Configuration.GetSection("BrowserSettings").Get<BrowserSettings>();
        var connectionStrings = Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();
        var credentials =  Configuration.GetSection("Credentials").Get<Credentials>();
        var urls = Configuration.GetSection("Urls").Get<Urls>();

        services.AddSingleton(browserSettings!);
        services.AddSingleton(connectionStrings!);
        services.AddSingleton(credentials!);
        services.AddSingleton(urls!);
        
        // Add Services
        services.AddCacheServices();
        services.AddCommonServices();
        services.AddDataServices();
        services.AddDbServices();
        services.AddPipelines();
        services.AddScrapers();
    }
}