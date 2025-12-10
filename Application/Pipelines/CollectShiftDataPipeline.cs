using Application.DataServices.Interfaces;
using Common.Logging;
using Domain.Entities;
using Domain.SourceModels;
using WebHarvester.Cropper;
using WebHarvester.Harvest.Interfaces;

namespace Application.Pipelines;

public class CollectShiftDataPipeline(
    IBrowserHost  browserHost, 
    ILoginBot loginBot,
    IShiftBookWeeksBot shiftBookWeeksBot,
    IDataServiceRegistry dataServiceRegistry)
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
            AppLogger.LogSuccess("Login successful!\n");
            AppLogger.LogInfo("Navigation to Shift Book Weeks - start point");


            // set shift book to start point for collection
            await shiftBookWeeksBot.GotoShiftBookWeeks();
            await shiftBookWeeksBot.NavigateToStartPoint();

            AppLogger.LogInfo("Press enter to start collecting data...");
            Console.ReadKey();
            Console.Clear();

            // loop through weeks and collect data
            while (true)
            {
                // collect week data
                var weekData = await shiftBookWeeksBot.CollectWeekData();
                AppLogger.LogInfo($"Week data collected for week: {shiftBookWeeksBot.CurrentWeekNumber}");

                foreach (var data in weekData)
                {
                    await ParseEmployeeWeekly(data);
                }

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

    /// <summary>
    /// Processes weekly employee shift data, updating related employee, workday, shift code, and shift records as
    /// needed.
    /// </summary>
    /// <remarks>This method updates the underlying data services with any new or modified employee, workday,
    /// shift code, or shift information found in the provided weekly data. Only new or changed records are added;
    /// existing records are not duplicated.</remarks>
    /// <param name="weekData">The source data representing an employee's shifts and related information for a single week. Cannot be null.</param>
    private async Task ParseEmployeeWeekly(SourceEmployeeWeek weekData)
    {
        // crop week data
        var cropper = new ShiftBookWeekCropper(
            weekData,
            dataServiceRegistry.EmployeeDataService.Data,
            dataServiceRegistry.WorkdayDataService.Data,
            dataServiceRegistry.ShiftCodeDataService.Data);
        cropper.Crop();

        // save new employee, new workday, new shiftcode data
        if (cropper.NewEmployee != null) await dataServiceRegistry.EmployeeDataService.AddSingleAsync(cropper.NewEmployee);
        if (cropper.NewWorkdays.Count > 0) await dataServiceRegistry.WorkdayDataService.AddRangeAsync(cropper.NewWorkdays);
        if (cropper.NewShiftCodes.Count > 0) await dataServiceRegistry.ShiftCodeDataService.AddRangeAsync(cropper.NewShiftCodes);

        // check if shift exists, save if not exist or different!
        List<Shift> newShifts = [];
        foreach (var shift in cropper.Shifts)
        {
            var result = CheckShiftExists(shift);
            if (result == null) continue;
            newShifts.Add(shift);
        }

        // save if updated shift
        if (newShifts.Count > 0) await dataServiceRegistry.ShiftDataService.AddRangeAsync(newShifts);
    }

    /// <summary>
    /// Determines whether a shift with the same employee, workday, shift code, and time range already exists in the
    /// data store.
    /// </summary>
    /// <remarks>A shift is considered equivalent if it matches the employee, workday, shift code, and start
    /// and end times. This method returns null only if an exact match is found; otherwise, it returns the provided
    /// shift.</remarks>
    /// <param name="shift">The shift to check for existence. Must specify employee, workday, shift code, and start and end times.</param>
    /// <returns>The original shift if no matching shift exists; otherwise, null if an equivalent shift is found.</returns>
    private Shift? CheckShiftExists(Shift shift)
    {
        // linq query shift on employee and workdate, get latest
        var exist = dataServiceRegistry.ShiftDataService.Data
            .Where(x => x.EmployeeId.SequenceEqual(shift.EmployeeId) && x.WorkdayId.SequenceEqual(shift.WorkdayId))
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefault();

        // return shift if it does not exist in cache / db
        if (exist == null) return shift;

        // return shift if shiftcode is different
        if (!exist.ShiftCodeId.SequenceEqual(shift.ShiftCodeId)) return shift;
        
        // return shift if any time values are changed
        if (exist.Time != shift.Time) return shift;

        // return null at this point
        return null;
    }
}