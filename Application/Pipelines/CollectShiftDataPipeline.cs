using Application.DataServices.Interfaces;
using Common.Logging;
using Common.PdfWriter;
using Domain.Entities;
using Domain.Misc;
using Domain.Settings;
using Domain.SourceModels;
using Persistence.DbServices.Interfaces;
using WebHarvester.Cropper;
using WebHarvester.Harvest.Interfaces;

namespace Application.Pipelines;

public class CollectShiftDataPipeline(
    IBrowserHost  browserHost, 
    ILoginBot loginBot,
    IShiftBookWeeksBot shiftBookWeeksBot,
    IShiftBookDailyBot shiftBookDailyBot,
    IDataServiceRegistry dataServiceRegistry,
    IShiftNoRemarkDataService noRemarkDataService,
    IBaseDbService<FilePath> filepathDbService,
    FileSettings fileSettings)
{
    private ShiftWeekReport _report =  new();
    
    public async Task RunPipelineAsync()
    {
        Console.Clear();
        try
        {
            // initialize browserhost and add page item to bots 
            await browserHost.StartBrowserSession();
            loginBot.Page = browserHost.Page;
            shiftBookWeeksBot.Page = browserHost.Page;
            shiftBookDailyBot.Page = browserHost.Page;

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

            // collect weekly data
            await CollectWeeklyData();

            // get remark data for missing remarks
            await ResolveRemarks();

            PrintFullWeeklyReport();
            Console.WriteLine("DEV :: End of code this far ");
            Console.ReadLine();
        }
        finally
        {
            await browserHost.CloseBrowserSessionAsync();
        }
    }

    /// <summary>
    /// Collects and processes weekly shift data by iterating through available weeks, saving relevant information to
    /// </summary>
    /// <remarks>This method performs multiple actions for each week, including ordering data, collecting
    /// shift information, saving weekly data as PDF files, updating database records, and printing reports. The
    /// operation continues until all weeks have been processed. This method should be awaited to ensure completion
    /// before proceeding with dependent operations.</remarks>
    /// <returns>A task that represents the asynchronous operation of collecting and storing weekly shift data.</returns>
    private async Task CollectWeeklyData()
    {
        // loop through weeks and collect data
        while (true)
        {
            // order names in table
            await shiftBookWeeksBot.ClickOrderTableByName();

            // collection of new shifts
            List<Shift> shifts = [];

            // collect week data
            var weekData = await shiftBookWeeksBot.CollectWeekData();
            AppLogger.LogInfo($"Week data collected for week: {shiftBookWeeksBot.CurrentWeekNumber}");

            foreach (var data in weekData)
            {
                var newShifts = await ParseEmployeeWeekly(data);
                shifts.AddRange(newShifts);
            }

            if (shifts.Count > 0)
            {
                // save page as PDF
                var filePath = await PdfWriter.WorkbookWeeklyToFile(
                    shiftBookWeeksBot.Page,
                    shiftBookWeeksBot.CurrentWeekNumber.ToString(),
                    fileSettings.DocumentDirectory);

                // create and store filepath
                var filePathEntity = new FilePath
                {
                    Guid = Guid.NewGuid(),
                    Path = filePath
                };
                await filepathDbService.CreateAsync(filePathEntity);

                // add filepath id to models
                foreach (var item in shifts)
                    item.FilePathGuid = filePathEntity.Guid;

                // save models to database
                await dataServiceRegistry.ShiftDataService.AddRangeAsync(shifts);
            }


            PrintWeeklyReport();
            AppLogger.LogSuccess("Harvest and cropping complete!\n");

            var endpointReached = await shiftBookWeeksBot.CheckEndpointReached();
            if (endpointReached) break;

            await shiftBookWeeksBot.ClickNextWeek();
        }
    }

    private async Task ResolveRemarks()
    {
        AppLogger.LogInfo("Starting remark resolution...");
        AppLogger.LogAdd("Loading data with missing remarks");
        await noRemarkDataService.LoadData();
        AppLogger.LogAdd("Navigate to page");
        await shiftBookDailyBot.GotoShiftDaily();
        AppLogger.LogSuccess("Starting remark collection loop");

        int index = 1;
        int lenght = noRemarkDataService.AllDates.Count;

        foreach (var date in noRemarkDataService.AllDates)
        {
            AppLogger.LogInfo($"Processing {index++} of {lenght} - Date: {date.ToShortDateString()}");

            // goto selected date and collect remark data
            await shiftBookDailyBot.NavigateToDate(date);
            var remarkData = await shiftBookDailyBot.GetTableData();

            // get shift data with no remark for the current date
            // iterate shifts with no remark. Get remark from collected data. If no remkark found on name set remark to blank
            // check if remark exists in database, and get guid. Else create new remark and get guid
            // update shift remark in database!!!
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
    private async Task<List<Shift>> ParseEmployeeWeekly(SourceEmployeeWeek weekData)
    {
        // crop week data
        var cropper = new ShiftBookWeekCropper(
            weekData,
            dataServiceRegistry.EmployeeDataService.Data,
            dataServiceRegistry.WorkdayDataService.Data,
            dataServiceRegistry.ShiftCodeDataService.Data);
        cropper.Crop();

        // save new employee, new workday, new shiftcode data
        if (cropper.NewEmployee != null)
        {
            await dataServiceRegistry.EmployeeDataService.AddSingleAsync(cropper.NewEmployee);
            _report.NewEmployees++;
        }

        if (cropper.NewWorkdays.Count > 0)
        {
            await dataServiceRegistry.WorkdayDataService.AddRangeAsync(cropper.NewWorkdays);
            _report.NewWorkdays += cropper.NewWorkdays.Count;
        }

        if (cropper.NewShiftCodes.Count > 0)
        {
            await dataServiceRegistry.ShiftCodeDataService.AddRangeAsync(cropper.NewShiftCodes);
            _report.NewShiftCodes += cropper.NewShiftCodes.Count;
        }
        
        // check if shift exists, save if not exist or different!
        List<Shift> newShifts = [];
        foreach (var shift in cropper.Shifts)
        {
            var result = CheckShiftExists(shift);
            if (result == null) continue;
            newShifts.Add(shift);
        }

        return newShifts;
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
    private Shift CheckShiftExists(Shift shift)
    {
        // linq query shift on employee and workdate, get latest
        var exist = dataServiceRegistry.ShiftDataService.Data
            .Where(x => x.EmployeeGuid == shift.EmployeeGuid && x.WorkdayGuid == shift.WorkdayGuid)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefault();

        // return shift if it does not exist in cache / db
        if (exist == null)
        {
            _report.NewShifts++;
            return shift;
        }

        // return shift if shiftcode is different
        if (exist.ShiftCodeGuid != shift.ShiftCodeGuid)
        {
            _report.UpdatedShifts++;
            return shift;
        }
        
        // return shift if any time values are changed
        if (exist.Time != shift.Time)
        {
            _report.UpdatedShifts++;
            return shift;
        }

        // return null at this point
        return null;
    }

    /// <summary>
    /// Generates and logs a summary of weekly report statistics, including counts of new and updated entities, then
    /// clears the report data.
    /// </summary>
    /// <remarks>This method logs only non-zero counts for each report category. After logging, the report
    /// data is reset for the next reporting cycle. This method does not return a value and is intended for internal use
    /// within the reporting workflow.</remarks>
    private void PrintWeeklyReport()
    {
        if (_report.NewEmployees > 0)
            AppLogger.LogAdd($"New employees: {_report.NewEmployees}");
        if (_report.NewWorkdays > 0)
            AppLogger.LogAdd($"New Workdays: {_report.NewWorkdays}");
        if (_report.NewShiftCodes > 0)
            AppLogger.LogAdd($"New ShiftCodes: {_report.NewShiftCodes}");
        if (_report.NewShifts > 0)
            AppLogger.LogAdd($"New shifts: {_report.NewShifts}");
        if (_report.UpdatedShifts > 0)
            AppLogger.LogAdd($"UpdatedShifts: {_report.UpdatedShifts}");
        
        _report.Clear();
    }

    /// <summary>
    /// Logs a detailed summary of all new and updated data for the current weekly report, including employees,
    /// workdays, shift codes, and shifts.
    /// </summary>
    /// <remarks>This method writes informational and addition logs for each category with new or updated
    /// entries. It is intended for internal use to provide visibility into changes made during the weekly reporting
    /// process.</remarks>
    private void PrintFullWeeklyReport()
    {
        AppLogger.LogInfo("All new data added:");
        if (_report.AllNewEmployees > 0) 
            AppLogger.LogAdd($"New employees: {_report.AllNewEmployees}");
        if (_report.AllNewWorkdays > 0)
            AppLogger.LogAdd($"New Workdays: {_report.AllNewWorkdays}");
        if (_report.AllNewShiftCodes > 0)
            AppLogger.LogAdd($"New ShiftCodes: {_report.AllNewShiftCodes}");
        if (_report.AllNewShifts > 0)
            AppLogger.LogAdd($"New Shifts: {_report.AllNewShifts}");
        if (_report.UpdatedShifts > 0)
            AppLogger.LogAdd($"UpdatedShifts: {_report.AllUpdatedShifts}");
            
    }
}