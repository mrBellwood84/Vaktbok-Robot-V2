using Domain.Settings;
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

            var onWeekNumber = await CheckWeekNumbers();
            if (onWeekNumber) return;
        }
    }

    private async Task<bool> CheckWeekNumbers()
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