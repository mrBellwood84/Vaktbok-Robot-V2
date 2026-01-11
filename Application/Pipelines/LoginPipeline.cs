using Common.Logging;
using Microsoft.Extensions.Logging;
using WebHarvester.Harvest.Interfaces;

namespace Application.Pipelines;


public class LoginPipeline(IBrowserHost browserHost, ILoginBot loginBot) 
{
    public async Task RunLoginSession()
    {
        Console.Clear();
        AppLogger.LogInfo("Starting login session...\n");
        try
        {
            await browserHost.StartBrowserSessionAsync();
            loginBot.Page = browserHost.Page;
            await loginBot.RunLoginProcedureAsync();

            Console.Clear();
            AppLogger.LogSuccess("Login successful!");
            AppLogger.LogInfo("You are now logged in. Press any key to close brower!");
            Console.ReadKey();
        }
        finally
        {
            await browserHost.CloseBrowserSessionAsync();
        }
    }
}