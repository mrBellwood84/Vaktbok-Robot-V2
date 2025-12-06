using Microsoft.Playwright;

namespace WebHarvester.Harvest.Interfaces;

public interface IBrowserHost
{
    IPage Page { get; }

    /// <summary>
    /// Start a new browser session. This will create a new browser, context and page!
    /// </summary>
    Task StartBrowserSession();

    /// <summary>
    /// Close browser and set all fields to null!
    /// </summary>
    Task CloseBrowserSessionAsync();
}