using Common.Logging;
using Domain.Settings;
using WebHarvester.Harvest.Interfaces;

namespace Application.Pipelines;

/// <summary>
/// Provides a pipeline for automating the process of logging in, navigating, and generating weekly PDF reports using
/// browser automation and bot components.
/// </summary>
/// <remarks>The pipeline coordinates browser session management, login, navigation, and user-driven data
/// collection steps. It ensures that resources are properly managed and that the process is interactive, requiring user
/// input to proceed with data collection and report generation.</remarks>
/// <param name="browserHost">The browser host used to manage browser sessions and provide access to the automation page.</param>
/// <param name="loginBot">The bot responsible for performing automated login procedures within the browser session.</param>
/// <param name="shiftWeekBot">The bot used to navigate and interact with the shift book weeks interface for data collection and PDF generation.</param>
public class WeekyPdfPrintPipeline(
    IBrowserHost browserHost,
    ILoginBot loginBot,
    IShiftBookWeeksBot shiftWeekBot,
    FileSettings fileSettings)
{

    /// <summary>
    /// Executes the weekly PDF pipeline asynchronously, performing browser session initialization, login, navigation,
    /// and data collection steps.
    /// </summary>
    /// <remarks>This method clears the console, logs progress messages, and ensures the browser session is
    /// properly closed even if an error occurs during execution. The pipeline requires user interaction to proceed with
    /// data collection.</remarks>
    public async Task RunPipeLineAsync()
    {
        Console.Clear();
        AppLogger.LogInfo("Starting Weekly PDF Pipeline...\n");

        try
        {
            // initialize browser session
            await browserHost.StartBrowserSessionAsync();
            loginBot.Page = browserHost.Page;
            shiftWeekBot.Page = browserHost.Page;

            // run login procedure
            await loginBot.RunLoginProcedureAsync();
            AppLogger.LogSuccess("Login successful!\n");
            AppLogger.LogInfo("Navigation to Shift Book Weeks - start point");

            // set shift book to start point for collection
            await shiftWeekBot.GotoShiftBookWeeks();
            await shiftWeekBot.NavigateToStartPoint();
            
            CreateFolder();

            AppLogger.LogInfo("Press enter to start collecting data...");
            Console.ReadKey();
            Console.Clear();

            await PrintWeeklyPdfAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred during the pipeline execution.", ex);
            return;
        }
        finally
        {
            await browserHost.CloseBrowserSessionAsync();
        }
    }

    /// <summary>
    /// Generates and prints weekly PDF reports by iteratively processing each week until the endpoint is reached.
    /// </summary>
    /// <remarks>This method requires user interaction to proceed to the next week. The operation continues
    /// until the endpoint is reached, at which point the process completes.</remarks>
    private async Task PrintWeeklyPdfAsync()
    {
        while (true)
        {
            // click print button
            await shiftWeekBot.ClickPrintPageButton();
            // await user to click next week
            CreateFileName(shiftWeekBot.CurrentWeekNumber);
            AppLogger.LogInfo("Press enter to proceed to next week...");
            Console.ReadKey();

            // check if enpoint reached
            var endpointReached = await shiftWeekBot.CheckEndpointReached();
            if (endpointReached) break;
            await shiftWeekBot.ClickNextWeek();
        }
    }

    /// <summary>
    /// Generates a file name string based on the current date and the specified week number.
    /// </summary>
    /// <remarks>The generated file name is formatted as "yyyyMMdd__<week_n>", where "yyyyMMdd" is the current
    /// date. The file name is logged for informational purposes.</remarks>
    /// <param name="week_n">The week number to include in the generated file name. Must be a non-negative integer.</param>
    private void CreateFileName(int week_n)
    {
        var week_n_string = week_n.ToString().PadLeft(2, '0');
        var now = DateTime.Now.ToString("yyyyMMdd");
        var name= $"{now}_{week_n_string}";
        AppLogger.LogInfo($"Generated file name: {name}");
    }
    
    /// <summary>
    /// Creates a new folder in the document directory using a fixed subfolder name and the current date.
    /// </summary>
    /// <remarks>The folder is created under the path specified by <c>fileSettings.DocumentDirectory</c>, with
    /// a subfolder named "Official" and another subfolder for the current date in "yyyyMMdd" format. If the folder
    /// already exists, no exception is thrown and the operation has no effect. An informational log entry is written
    /// after the folder is created.</remarks>
    private void CreateFolder()
    {
        var dirName = "PdfPrint";
        var date = DateTime.Now.ToString("yyyyMMdd");
        var folderPath = Path.Combine(fileSettings.DocumentDirectory, dirName, date);

        Directory.CreateDirectory(folderPath);
        AppLogger.LogInfo($"Created folder path: {folderPath}");
    }
}
