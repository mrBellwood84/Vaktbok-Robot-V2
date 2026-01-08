using Domain.Settings;
using Microsoft.Playwright;
using WebHarvester.Harvest.Exceptions;
using WebHarvester.Harvest.Interfaces;

namespace WebHarvester.Harvest;

public class BaseBot(BrowserSettings settings) : IBaseBot
{
    public IPage Page { get; set; } = null!;
    
    /// <summary>
    /// Navigates the browser page to the specified URL and waits for the page to finish loading.
    /// </summary>
    /// <param name="url">The URL to navigate to. Must be a valid absolute URI.</param>
    /// <returns>A task that represents the asynchronous navigation operation.</returns>
    /// <exception cref="BrowserPageTimeoutException">Thrown if the page fails to load within the configured timeout period.</exception>
    public async Task GotoAsync(string url)
    {
        Check_page_exists();
        
        try
        {
            await Page.GotoAsync(url, new PageGotoOptions()
            {
                Timeout = settings.PageLoadTimeout,
                WaitUntil = WaitUntilState.Load
            });
        }
        catch (TimeoutException e)
        {
            var message = $"Could not go to {url}.\nTimeout: {e.Message}";
            throw new BrowserPageTimeoutException(message, e);
        }
    }
    /// <summary>
    /// Attempts to locate and click the element identified by the specified XPath expression asynchronously.
    /// </summary>
    /// <param name="xpath">The XPath expression used to locate the element to be clicked. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous click operation.</returns>
    /// <exception cref="BrowserElementNotFoundException">Thrown if the element cannot be found or clicked within the configured timeout period.</exception>
    protected async Task ClickAsync(string xpath)
    {
        Check_page_exists();

        try
        {
            var button = Page.Locator(xpath);
            await button.WaitForAsync(new LocatorWaitForOptions()
            {
                Timeout = settings.ContentLoadTimeout
            });
            await button.ClickAsync(new LocatorClickOptions()
            {
                Timeout = settings.ContentLoadTimeout
            });
        }
        catch (TimeoutException e)
        {
            var message = $"Could click on element {xpath}.\nTimeout: {e.Message}";
            throw new BrowserElementNotFoundException(message, e);
        }
    }
    
    /// <summary>
    /// Attempts to input the specified text into the element identified by the given XPath expression, retrying if
    /// necessary until the input is successful or a timeout occurs.
    /// </summary>
    /// <param name="xpath">The XPath expression used to locate the target element on the page. Cannot be null or empty.</param>
    /// <param name="text">The text to input into the located element. If null or empty, the input field will be cleared.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="BrowserElementNotFoundException">Thrown if the element cannot be found, is not interactable, or the text cannot be input after multiple attempts.</exception>
    protected async Task InputTextAsync(string xpath, string text)
    {
        Check_page_exists();
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
            throw new BrowserElementNotFoundException($"Could not input text {xpath}");
        }
        catch (TimeoutException e)
        {
            var message = $"Could input text on element {xpath}.\nTimeout: {e.Message}";
            throw new BrowserElementNotFoundException(message, e);
        }
    }

    /// <summary>
    /// Asynchronously retrieves the text content of the first element that matches the specified XPath expression.
    /// </summary>
    /// <param name="xpath">The XPath expression used to locate the element whose text content is to be retrieved. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the text content of the matched
    /// element.</returns>
    /// <exception cref="BrowserElementNotFoundException">Thrown if the element is found but its text content is null or empty.</exception>
    /// <exception cref="BrowserElementNoContentException">Thrown if the operation times out while waiting for the element or its content to become available.</exception>
    protected async Task<string> GetElementTextAsync(string xpath)
    {
        Check_page_exists();

        try
        {
            var elem = Page.Locator(xpath);
            await elem.WaitForAsync(new LocatorWaitForOptions()
            {
                Timeout = settings.ContentLoadTimeout
            });
            var text = await elem.TextContentAsync(new LocatorTextContentOptions()
            {
                Timeout = settings.ContentLoadTimeout
            });
            
            if (string.IsNullOrEmpty(text))
                throw new BrowserElementNotFoundException($"Could not get text on element {xpath}");
            return text;
        }
        catch (TimeoutException e)
        {
            var message = $"Could get text on element {xpath}.\nTimeout: {e.Message}";
            throw new BrowserElementNoContentException(message, e);
        }
    }
    

    /// <summary>
    /// Checks whether the Page property is set and throws an exception if it is null.
    /// </summary>
    /// <exception cref="NullReferenceException">Thrown if the Page property is null.</exception>
    private void Check_page_exists()
    {
        if (Page == null)
            throw new NullReferenceException("Page is null! Set page to null!");
    }
}