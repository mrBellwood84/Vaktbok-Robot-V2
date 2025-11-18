using Domain.Settings;
using Infrastructure.Scraper.Interfaces;
using Microsoft.Playwright;
using BrowserType = Domain.Enums.BrowserType;

namespace Infrastructure.Scraper;

/// <summary>
/// Host browser session. Can create and close browser session!
/// </summary>
/// <param name="settings"></param>
public class BrowserHost(BrowserSettings settings) : IBrowserHost
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IBrowserContext _context;
    private IPage _page;
    
    public IPage Page => _page ?? throw new NullReferenceException("Page does not exists");

    /// <summary>
    /// Start a new browser session. This will create a new browser, context and page!
    /// </summary>
    public async Task StartBrowserSession()
    {
        await CreateBrowserAsync();
        await CreateContext();
        await CreatePage();
    }
    
    /// <summary>
    /// Close browser and set all fields to null!
    /// </summary>
    public async Task CloseBrowserSessionAsync()
    {
        await _browser!.CloseAsync();
        _playwright!.Dispose();
        _playwright = null;
        _browser = null;
        _context = null;
        _page = null;
    } 
    
    /// <summary>
    /// Create playwright instance and browser for session
    /// </summary>
    private async Task CreateBrowserAsync()
    {
        // create options
        var options = new BrowserTypeLaunchOptions
        {
            Headless = settings.Headless,
            SlowMo = settings.SlowMo,
            Args =
            [
                "--no-first-run",
                "--no-default-browser-check",
                "--disable-blink-features=AutomationControlled",
                "--ignore-certificate-errors",
                "--ignore-ssl-errors",
                "--allow-insecure-localhost",
                "--disable-web-security"
            ]
        };
        
        // create new instance of playwright
        _playwright = await Playwright.CreateAsync();
        
        // create browser with switch statement, dependent on browser type in settings
        _browser = settings.BrowserType switch
        {
            BrowserType.Chrome => await _playwright.Chromium.LaunchAsync(options),
            BrowserType.Firefox => await _playwright.Firefox.LaunchAsync(options),
            BrowserType.Webkit => await _playwright.Chromium.LaunchAsync(options),
            _ => throw new NotSupportedException($"Browser type {settings.BrowserType} not supported")
        };
    }

    /// <summary>
    /// Create browser context based on existing browser
    /// </summary>
    private async Task CreateContext()
    {
        var contextOpts = new BrowserNewContextOptions
        {
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                        "AppleWebKit/537.36 (KHTML, like Gecko) " +
                        "Chrome/118.0 Safari/537.36",
            Locale = "nb-NO",
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
            IgnoreHTTPSErrors = true,
        };
        
        _context = await _browser!.NewContextAsync(contextOpts);
    }

    /// <summary>
    /// Create page within created browser and context
    /// </summary>
    private async Task CreatePage()
    {
        _page = await _context!.NewPageAsync();
        await _page.SetExtraHTTPHeadersAsync(new Dictionary<string, string>
        {
            ["Accept-Language"] = "nb-NO,nb;q=0.9,en;q=0.8"
        });
    }
}