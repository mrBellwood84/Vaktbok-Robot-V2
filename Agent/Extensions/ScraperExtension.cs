using WebHarvester.Harvest;
using WebHarvester.Harvest.Bots;
using WebHarvester.Harvest.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Agent.Extensions;

public static class ScraperExtension
{
    /// <summary>
    /// Add all scrapers / robots
    /// </summary>
    public static IServiceCollection AddScrapers(this IServiceCollection services)
    {
        // Adding browser host as singleton!!
        services.AddSingleton<IBrowserHost, BrowserHost>();

        // add bots
        services.AddTransient<ILoginBot, LoginBot>();
        services.AddTransient<IShiftBookWeeksBot, ShiftBookWeeksBot>();

        return services;
    }
}