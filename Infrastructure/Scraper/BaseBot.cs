using Domain.Settings;
using Infrastructure.Scraper.Exceptions;
using Microsoft.Playwright;

namespace Infrastructure.Scraper;

public class BaseBot(BrowserSettings settings)
{
    public IPage Page { get; set; } = null!;
    
    
    /// <summary>
    /// Go to url 
    /// </summary>
    protected async Task GotoAsync(string url)
    {
        _check_page_exists();
        
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

    /// <summary>
    /// Preform mouse click on element when founc
    /// </summary>
    protected async Task ClickAsync(string xpath)
    {
        _check_page_exists();

        try
        {
            await Page.Locator(xpath).ClickAsync(new LocatorClickOptions()
            {
                Timeout = settings.ContentLoadTimeout
            });
        }
        catch (TimeoutException e)
        {
            var message = $"Could click on element {xpath}.\nTimeout: {e.Message}";
            throw new BrowserPageTimeoutException(message, e);
        }
    }
    
    /// <summary>
    /// Fill text input 
    /// </summary>
    protected async Task InputTextAsync(string xpath, string text)
    {
        _check_page_exists();
        try
        {
            var locator = Page.Locator(xpath);
            var attempts = 3;
            while (attempts-- > 0)
            {
                await locator.WaitForAsync(new LocatorWaitForOptions()
                {
                    Timeout = settings.ContentLoadTimeout
                });
                
                await locator.FillAsync(text, new LocatorFillOptions()
                {
                    Timeout = settings.ContentLoadTimeout
                });

                await Task.Delay(50);
                
                var inputValue = await locator.InputValueAsync();
                if (inputValue == text) return;
            }
            throw new BrowserPageTimeoutException($"Could not input text {xpath}");
        }
        catch (TimeoutException e)
        {
            var message = $"Could input text on element {xpath}.\nTimeout: {e.Message}";
            throw new BrowserElementNotFoundException(message, e);
        }
    }
    
    /// <summary>
    /// Throw null reference exception if page not set!
    /// </summary>
    private void _check_page_exists()
    {
        if (Page == null)
            throw new NullReferenceException("Page is null! Set page to null!");
    }
}