using Infrastructure.Scraper.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Pipelines;

public class LoginPipeline(IBrowserHost browserHost, ILoginBot loginBot, ILogger<LoginPipeline> logger)
{
    public async Task RunLoginSession()
    {
        Console.Clear();
        try
        {
            await browserHost.StartBrowserSession();
            loginBot.Page = browserHost.Page;
            await loginBot.RunLoginProcedureAsync();
            
            Console.Clear();
            logger.LogInformation("Logged in session started!");
            logger.LogInformation("Press any key to exit!");
            Console.ReadKey();
        }
        finally
        {
            await browserHost.CloseBrowserSessionAsync();
        }
    }
}