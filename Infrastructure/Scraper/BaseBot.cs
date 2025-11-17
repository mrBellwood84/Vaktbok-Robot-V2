using Domain.Settings;
using Infrastructure.Scraper.Exceptions;
using Microsoft.Playwright;

namespace Infrastructure.Scraper;

public class BaseBot(BrowserSettings settings)
{
    public IPage Page { get; set; } = null!;
    
    public async Task GotoAsync(string url)
    {
        if (Page == null) 
            throw new NullReferenceException("Page is null! Set page to instance before calling GotoAsync!");
        
        try
        {
            await Page.GotoAsync(url, new PageGotoOptions()
            {
                Timeout = settings.PageLoadTimeout
            });
        }
        catch (TimeoutException e)
        {
            var message = $"Could not go to {url}.\nTimeout: {e.Message}";
            throw new BrowserPageTimeoutException(message, e);
        }
    }
}