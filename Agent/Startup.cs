using Agent.Extensions;
using Application.DataServices.Interfaces;
using Domain.Settings;
using Common.Logging;
using Persistence.DbServices;
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
        var calendarSettings = Configuration.GetSection("CalendarSettings").Get<CalendarSettings>();
        var connectionStrings = Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();
        var credentials = Configuration.GetSection("Credentials").Get<Credentials>();
        var urls = Configuration.GetSection("Urls").Get<Urls>();

        // add configurations
        services.AddSingleton(browserSettings!);
        services.AddSingleton(calendarSettings!);
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

    public async Task InitializeInfrastructure(IServiceProvider provider)
    {
        var connectionString = provider.GetRequiredService<ConnectionStrings>().Root;
        var dataServiceRegistry = provider.GetRequiredService<IDataServiceRegistry>();

        Console.Clear();
        AppLogger.LogInfo("Initializing application!\n");

        // Initialize database
        InitializeDatabase.Setup(connectionString);

        // Initialize cache
        AppLogger.LogInfo("Initializing data cache...");
        await dataServiceRegistry.InitalizeCacheAsync();
        AppLogger.LogSuccess("Data cache initialized!\n");

        AppLogger.LogSuccess("Application initialized!\n");
    }
}