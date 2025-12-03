using Infrastructure.Scraper.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Pipelines;

public class CollectShiftDataPipeline(
    IBrowserHost  browserHost, 
    ILoginBot loginBot,
    IShiftBookWeeksBot shiftBookWeeksBot,
    ILogger<CollectShiftDataPipeline> logger)
{
    public async Task RunPipelineAsync()
    {
        Console.Clear();
        try
        {
            // initialize browserhost and add page item to bots 
            await browserHost.StartBrowserSession();
            loginBot.Page = browserHost.Page;
            shiftBookWeeksBot.Page = browserHost.Page;

            // run login procedure
            await loginBot.RunLoginProcedureAsync();
            Console.Clear();
            logger.LogInformation("Shift book weeks logged in");

            // set shift book to start point for collection
            await shiftBookWeeksBot.GotoShiftBookWeeks();
            await shiftBookWeeksBot.NavigateToStartPoint();

            // loop through weeks and collect data
            while (true)
            {
                // collect week data
                var weekData = await shiftBookWeeksBot.CollectWeekData();
                logger.LogInformation($"Parsing data for week: {shiftBookWeeksBot.CurrentWeekNumber}");

                // DEV :: store collected data to file here
                var fileName = $"test_json_week_{shiftBookWeeksBot.CurrentWeekNumber}.json";
                var json = JsonSerializer.Serialize(weekData, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync($"{fileName}", json);

                // process week data here...
                // DEV :: No parser added

                // Check new employees in dataset
                // Check workday exists in dataset
                // Check shiftcodes exists in dataset

                // add shift data here
                // DEV :: No database service exists yet
                // DEV :: Use bulk insert when adding to database



                logger.LogInformation("DEV :: Collected data here!. Click enter to continue to next week...");
                Console.ReadKey();

                var endpointReached = await shiftBookWeeksBot.CheckEndpointReached();
                if (endpointReached) break;

                await shiftBookWeeksBot.ClickNextWeek();
            }



            Console.WriteLine("DEV :: End of code this far ");
            Console.ReadLine();
        }
        finally
        {
            await browserHost.CloseBrowserSessionAsync();
        }
    }
}