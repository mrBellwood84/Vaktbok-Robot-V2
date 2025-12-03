using Domain.Settings;
using Domain.SourceModels;
using Infrastructure.Scraper.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace Infrastructure.Scraper.Bots;

public class ShiftBookWeeksBot(
    BrowserSettings browserSettings,
    CalendarSettings calendarSettings)
    : BaseBot(browserSettings), IShiftBookWeeksBot
{
    // XPaths for scraping
    private const string ShiftBookButtonXPath = "//span[@class=\"quick-nav-icon icon-shiftbook\"]";
    private const string ShiftBookWeekViewXPath = "//div[@class=\"ui-icon icon-calendar-week\"]";

    private const string WeekInfoContainerXPath = "//span[contains(text(), \"Uke\")]";
    private const string NextWeekButtonXPath = "(//div[@role=\"button\"])[2]";

    private const string HeaderRowXPath = "(//table)[7]//tr[1]/td";
    private const string TableRowsXPath = "(//table)[8]/tbody/tr";

    private const string PrintWeekButtonXPath = "//div[@data-bind=\"click: PrintLogic()\"]";

    private const string CalendarBackButtonXPath = "(//li[@class=\"prev-month button no-select\"])[1]";
    private const string CalendarForwardButtonXPath = "(//li[@class=\"next-month button no-select\"])[1]";
    private const string CalendarWeekNumbersXPath = "(//table[@class=\"week-table\"])[1]//td[@class=\"week-number\"]";
    private const string CalendarYearContainerXPath = "(//li[@class=\"current-month caption no-select\"])[1]//span";


    // current week number and year
    public int CurrentWeekNumber { get; set; } = 0;
    private int currentYear = 0;

    // column headers for current week
    private SourceWorkbookWeekHeader[] currentWeekHeaders = null;



    /// <summary>
    /// This action goes to shift book week view
    /// </summary>
    public async Task GotoShiftBookWeeks()
    {
        await ClickAsync(ShiftBookButtonXPath);
        await ClickAsync(ShiftBookWeekViewXPath);
    }

    /// <summary>
    /// Navigate to page startpoint
    /// </summary>
    public async Task NavigateToStartPoint()
    {
        // open select calendar
        await ClickAsync(WeekInfoContainerXPath);
        while (true)
        {
            // get year as integer
            var rawYearContent = (await GetElementTextAsync(CalendarYearContainerXPath));
            var year = int.Parse(rawYearContent.Split(',')[1].Trim());

            // navigate year back if startyear is lower
            if (year > calendarSettings.YearStart)
            {
                await ClickAsync(CalendarBackButtonXPath);
                continue;
            }
            // or move forward if current calendar year is lower than selected start year
            if (year < calendarSettings.YearStart)
            {
                await ClickAsync(CalendarForwardButtonXPath);
                continue;
            }

            var onWeekNumber = await ClickToStartWeekNumber();
            if (onWeekNumber) return;
        }
    }

    /// <summary>
    /// Click to next week in shift book weekly view
    /// </summary>
    public async Task ClickNextWeek() => await ClickAsync(NextWeekButtonXPath);

    /// <summary>
    /// Determines whether the calendar endpoint has been reached based on the current year and week information
    /// extracted from the page.
    /// </summary>
    /// <remarks>This method evaluates the endpoint by comparing the extracted year and week values against
    /// the configured end year and week in <c>calendarSettings</c>. It is typically used to control navigation or
    /// processing flow when iterating through calendar data.</remarks>
    /// <returns>A <see langword="true"/> value if the current year and week are at or beyond the configured calendar endpoint;
    /// otherwise, <see langword="false"/>.</returns>
    public async Task<bool> CheckEndpointReached()
    {
        if (currentYear > calendarSettings.YearEnd) return true;
        if (currentYear == calendarSettings.YearEnd 
            && CurrentWeekNumber >= calendarSettings.WeekNumberEnd) return true;
        return false;
    }

    /// <summary>
    /// Collect week data from current table
    /// </summary>
    public async Task<List<SourceEmployeeWeek>> CollectWeekData()
    {
        List<SourceEmployeeWeek> result = [];

        // Set current week, year and headers
        await SetCurrentWeekAndYear();
        await SetTableColumnHeaders();

        // collect all shifts role
        var tableRows = await Page.Locator(TableRowsXPath).AllAsync();
        foreach (var row in tableRows)
        {
            var data = await ParseTableRow(row);
            result.Add(data);
        }

        // collect raw data 
        return result;
    }

    /// <summary>
    /// Asynchronously retrieves the current week number and year from the page and updates the corresponding fields.
    /// </summary>
    /// <remarks>This method parses week and year information from the page content using the specified
    /// locator. The fields for week number and year are updated based on the extracted values. Ensure that the page
    /// contains the expected format for week and year information before calling this method.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task SetCurrentWeekAndYear()
    {
        // set current week number and year from page
        var rawYearWeekContent = await Page.Locator(WeekInfoContainerXPath).TextContentAsync();
        var splitContent = rawYearWeekContent!.Split(',');
        var weekContent = int.Parse(splitContent[0].Split(' ').Last().Trim());
        var yearContent = int.Parse(splitContent[1].Split(' ').Last().Trim());

        CurrentWeekNumber = weekContent;
        currentYear = yearContent;
    }

    /// <summary>
    /// Attempts to locate and click the calendar week number that matches the configured start week number.
    /// </summary>
    /// <remarks>If the matching week number is not found in the current view, the method navigates backward
    /// or forward in the calendar as appropriate before returning <see langword="false"/>. This method is intended to
    /// be used in calendar navigation scenarios where selecting a specific start week is required.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the matching
    /// week number was found and clicked; otherwise, <see langword="false"/>.</returns>
    private async Task<bool> ClickToStartWeekNumber()
    {
        var weekNumbers = await Page.Locator(CalendarWeekNumbersXPath).AllAsync();
        foreach (var elem in weekNumbers)
        {
            // get week number in select calendar as integer
            var textContent = await elem.TextContentAsync();
            var numberContent = int.Parse(textContent!);

            // click on element if week number is a match, then return true for found 
            if (numberContent == calendarSettings.WeekNumberStart)
            {
                await elem.ClickAsync();
                return true;
            }

            // if first week number is larger than selected start week,
            // click backward and report false for not found
            if (numberContent > calendarSettings.WeekNumberStart)
            {
                await ClickAsync(CalendarBackButtonXPath);
                return false;
            }
        }

        // if weeknumber was not found in this list,
        // click forward to check next month then report false for not found
        await ClickAsync(CalendarForwardButtonXPath);
        return false;
    }

    /// <summary>
    /// Retrieves the column headers for the current table week as an array of parsed header objects.
    /// </summary>
    /// <remarks>This method asynchronously extracts header information from the page and parses each header
    /// into a <see cref="SourceWorkbookWeekHeader"/> instance. The returned array may contain default or partially
    /// populated elements if header data is missing or malformed.</remarks>
    /// <returns>An array of <see cref="SourceWorkbookWeekHeader"/> objects representing the headers for each day of the week.
    /// The array contains seven elements, one for each day.</returns>
    private async Task SetTableColumnHeaders()
    {
        var result = new SourceWorkbookWeekHeader[7];

        var headerRows = await Page.Locator(HeaderRowXPath).AllAsync();
        for (int i = 0; i < headerRows.Count; i++)
        {
            var rawTextContent = await headerRows[i].TextContentAsync();
            var splitText = rawTextContent.ToLower().Split(' ');
            
            if (splitText[0] == "navn") continue;

            var dateSplit = splitText[1].Split('.');

            var data = new SourceWorkbookWeekHeader();
            data.GetDayInteger(splitText[0]);
            data.Date = int.Parse(dateSplit[0]);
            data.Month = int.Parse(dateSplit[1]);
            data.Year = int.Parse(dateSplit[2]);

            result[i - 1] = data;
        }

        currentWeekHeaders = result;
    }

    /// <summary>
    /// Parses a table row element to extract employee shift data for a specific week.
    /// </summary>
    /// <remarks>Each cell in the row is interpreted as either the employee's name or a shift entry, based on
    /// its position. The method assumes that the row structure matches the expected format for weekly shift
    /// data.</remarks>
    /// <param name="row">The table row locator representing an employee's weekly shift data. Must not be null.</param>
    /// <returns>A <see cref="SourceEmployeeWeek"/> object containing the employee's name and a collection of shift entries
    /// parsed from the row.</returns>
    private async Task<SourceEmployeeWeek> ParseTableRow(ILocator row)
    {
        var result = new SourceEmployeeWeek();
        var cells = await row.Locator("td").AllAsync();

        for (int i = 0; i < cells.Count; i++)
        {
            var rawTextContent = await cells[i].TextContentAsync();
            if (i == 0)
            {
                result.EmployeeName = rawTextContent!.Trim();
                continue;
            }

            // parse shift entry
            var shiftEntry = new SourceShiftEntry();
            var headerInfo = currentWeekHeaders[i - 1];

            // set header info
            shiftEntry.Year = headerInfo.Year;
            shiftEntry.Month = headerInfo.Month;
            shiftEntry.Date = headerInfo.Date;
            shiftEntry.Day = headerInfo.Day;
            shiftEntry.WeekNumber = CurrentWeekNumber;

            // set cell content
            shiftEntry.CellContent = rawTextContent!.Trim();

            result.Shifts.Add(shiftEntry);
        }

        return result;
    }
}
