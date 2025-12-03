using Agent.Extensions;
using Domain.Settings;
using Infrastructure.Logging;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Agent;

public class Startup
{
    private IConfiguration Configuration { get; }
    private string _rootConnectionString = string.Empty;

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

        // set root connection strings for later use in startup
        _rootConnectionString = connectionStrings.Root;

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

    public void InitializeInfrastructure() 
    {
        Console.Clear();
        AppLogger.LogInfo("Initializing application!");

        // Initialize database
        InitializeDatabase.Setup(_rootConnectionString);

        // Initialize cache
        AppLogger.LogDev("Initializing cache not added yet!");
        AppLogger.LogDev("Press any key to continue...");
        Console.ReadKey();
    }
}