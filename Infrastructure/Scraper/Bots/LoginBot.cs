using Domain.Settings;
using Infrastructure.Scraper.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Scraper.Bots;

/// <summary>
/// This bot preforms a login procedure on Helse Bergen internal pages
/// </summary>
/// <param name="settings"></param>
public class LoginBot(BrowserSettings settings, Credentials credentials, Urls urls, ILogger<LoginBot> logger)
    : BaseBot(settings), ILoginBot
{
    private const string HelseUserInputXPath = "//input[@type='email']";
    private const string HelsePasswordInputXPath = "//input[@type='password']";
    private const string HelseSubmitXPath = "//input[@type='submit']";

    private const string HelseStayLoginButtonXpath = "//input[@type='button']";

    private const string GatUserInputXPath = "//input[@name='username']";
    private const string GatPasswordInputXPath = "//input[@name='password']";
    private const string GatSubmitXPath = "//button[@id='btnLoginPage1']";

    private const string CookieCloseBtn = "//div[@id='cookieDisclaimerDiv']//button";

    public async Task RunLoginProcedureAsync()
    {
        logger.LogInformation("Starting login procedure!");
        // goto login page
        await GotoAsync(urls.Entry);

        // login to helse net
        await InputTextAsync(HelseUserInputXPath, credentials.IHelseUser);
        await ClickAsync(HelseSubmitXPath);
        await InputTextAsync(HelsePasswordInputXPath, credentials.IHelsePassword);
        await ClickAsync(HelseSubmitXPath);

        // wait for two-factor auth here...
        while (true)
        {
            var currentUrl = Page.Url;
            if (currentUrl == urls.LoginSleep) break;

            logger.LogInformation("Two-factor authentication required. Waiting for user confirmation via mobile app...");
            await Task.Delay(5000);
        }

        // click away the stay login dialog.
        await ClickAsync(HelseStayLoginButtonXpath);

        // login to gat
        await InputTextAsync(GatUserInputXPath, credentials.GatUser);
        await InputTextAsync(GatPasswordInputXPath, credentials.GatPassword);
        await ClickAsync(GatSubmitXPath);

        // close cookie button
        await ClickAsync(CookieCloseBtn);

        // log complete
        logger.LogInformation("Finished login procedure!");
    }
}