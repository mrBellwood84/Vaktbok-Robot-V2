using Domain.Settings;
using Domain.SourceModels;
using Infrastructure.Scraper.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Scraper.Bots;

public class ShiftBookWeeksBot(
    BrowserSettings browserSettings,
    CalendarSettings calendarSettings,
    ILogger<ShiftBookWeeksBot> logger)
    : BaseBot(browserSettings), IShiftBookWeeksBot
{
    private const string ShiftBookButtonXPath = "//span[@class=\"quick-nav-icon icon-shiftbook\"]";
    private const string ShiftBookWeekViewXPath = "//div[@class=\"ui-icon icon-calendar-week\"]";

    private const string WeekInfoContainerXPath = "//span[contains(text(), \"Uke\")]";
    private const string NextWeekButtonXPath = "(//div[@role=\"button\"])[2]";

    private const string SortByNameXPath = "(//td[@role=\"columnheader\"])[1]";
    private const string TableRowsXPath = "(//table)[8]/tbody/tr";

    private const string PrintWeekButtonXPath = "//div[@data-bind=\"click: PrintLogic()\"]";

    private const string CalendarBackButtonXPath = "(//li[@class=\"prev-month button no-select\"])[1]";
    private const string CalendarForwardButtonXPath = "(//li[@class=\"next-month button no-select\"])[1]";
    private const string CalendarWeekNumbersXPath = "(//table[@class=\"week-table\"])[1]//td[@class=\"week-number\"]";
    private const string CalendarYearContainerXPath = "(//li[@class=\"current-month caption no-select\"])[1]//span";


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

            var onWeekNumber = await CheckCalendarWeekNumber();
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
        var rawYearWeekContent = await Page.Locator(WeekInfoContainerXPath).TextContentAsync();
        var splitContent = rawYearWeekContent!.Split(',');
        var weekContent = int.Parse(splitContent[0].Split(' ').Last().Trim());
        var yearContent = int.Parse(splitContent[1].Split(' ').Last().Trim());

        if (yearContent > calendarSettings.YearEnd) return true;
        if (yearContent == calendarSettings.YearEnd && weekContent >= calendarSettings.WeekNumberEnd) return true;
        return false;
    }

    /// <summary>
    /// Collect week data from current table
    /// </summary>
    public async Task<List<SourceEmployeeWeek>> CollectWeekData()
    {
        List<SourceEmployeeWeek> result = [];

        // collect dates to list

        // collect all shifts role

        // get name from row start
        // get work info from table cell

        // collect raw data 
        return result;
    }

    /// <summary>
    /// Checks if calendar contains selected week number. 
    /// Clicks navigation buttons forward/backward as needed.
    /// </summary>
    private async Task<bool> CheckCalendarWeekNumber()
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


}