using Common.Logging;
using Domain.Settings;
using WebHarvester.Harvest.Interfaces;

namespace WebHarvester.Harvest.Bots;

public class ShiftBookDailyBot(
    BrowserSettings settings)
    : BaseBot(settings), IShiftBookDailyBot
{

    private const string ShiftBookButtonXPath = "//span[@class=\"quick-nav-icon icon-shiftbook\"]";
    private const string ShiftBookDailyViewXPath = "//div[@class=\"ui-icon icon-calendar-day\"]";

    private const string CalendarButtonXPath = "//div[@class=\"button-choice\"]/span";
    private const string CalendarPrevMonthButtonXPath = "(//gs-date-picker)[1]//li[@class=\"prev-month button no-select\"]";
    private const string CalendarNextMonthButtonXPath = "(//gs-date-picker)[1]//li[@class=\"next-month button no-select\"]";
    private const string CalendarMonthLabelXPath = "(//gs-date-picker)[1]//li[@class=\"current-month caption no-select\"]/span";
    private const string CalendarDaysXPath = @"(//gs-date-picker)[1]//table
                                                //td[contains(concat(' ', normalize-space(@class), ' '), ' week-day ') 
                                                and not(contains(concat(' ', normalize-space(@class), ' '), ' prev-month ')) 
                                                and not(contains(concat(' ', normalize-space(@class), ' '), ' next-month '))]";

    private const string DataTable1XPath = "(//table[@class=\"gs-grid grid-striped\"])[1]/tbody/tr";
    private const string DataTable2XPath = "(//table[@class=\"gs-grid grid-striped\"])[3]/tbody/tr";

    private static readonly Dictionary<string, int> _monthMap = new()
    {
        { "januar", 1 },
        { "februar", 2 },
        { "mars", 3 },
        { "april", 4 },
        { "mai", 5 },
        { "juni", 6 },
        { "juli", 7 },
        { "august", 8 },
        { "september", 9 },
        { "oktober", 10 },
        { "november", 11 },
        { "desember", 12 }
    };


    /// <summary>
    /// Navigates to the daily view of the shift book in the user interface asynchronously.
    /// </summary>
    public async Task GotoShiftDaily()
    {
        await ClickAsync(ShiftBookButtonXPath);
        await ClickAsync(ShiftBookDailyViewXPath);
    }

    /// <summary>
    /// Navigates the calendar UI to the specified date asynchronously.
    /// </summary>
    /// <param name="date">The date to which the calendar should be navigated.</param>
    /// <returns>A task that represents the asynchronous navigation operation.</returns>
    public async Task NavigateToDate(DateTime date)
    {
        await ClickAsync(CalendarButtonXPath);
        await NavigateToYear(date.Year);
        await NavigateToMonth(date.Month);
        await ClickDay(date.Day);
    }

    /// <summary>
    /// Asynchronously retrieves key-value pairs from two data tables, mapping each entry's name to its corresponding
    /// remark.
    /// </summary>
    /// <remarks>If duplicate names are present across both tables, the remark from the last occurrence will
    /// overwrite previous values for that name.</remarks>
    /// <returns>A dictionary containing the names as keys and their associated remarks as values. If no data is found, the
    /// dictionary is empty.</returns>
    public async Task<Dictionary<string, string>> GetTableData()
    {
        Dictionary<string, string> result = [];
        var table1 = await Page.Locator(DataTable1XPath).AllAsync();
        var table2 = await Page.Locator(DataTable2XPath).AllAsync();


        foreach (var row in table1)
        {
            var cells = await row.Locator("td").AllAsync();
            if (cells.Count < 4) continue;
            var name = await cells[3].InnerTextAsync();
            var remark = await cells[1].InnerTextAsync();
            result[name] = remark;
        }

        foreach (var row in table2)
        {
            var cells = await row.Locator("td").AllAsync();
            if (cells.Count < 4) continue;
            var name = await cells[3].InnerTextAsync();
            var remark = await cells[1].InnerTextAsync();
            result[name] = remark;
        }

        return result;
    }

    /// <summary>
    /// Navigates the calendar UI to display the specified year asynchronously.
    /// </summary>
    /// <remarks>If the calendar is already displaying the specified year, no navigation occurs. This method
    /// simulates user interaction by clicking navigation controls until the desired year is reached.</remarks>
    /// <param name="year">The year to which the calendar should be navigated.</param>
    private async Task NavigateToYear(int year)
    {
        var calendarLabel = await GetElementTextAsync(CalendarMonthLabelXPath);
        var currentYear = int.Parse(calendarLabel.Split(',').Last().Trim());
        if (currentYear == year) return;

        if (currentYear < year)
        {
            while (currentYear < year)
            {
                await ClickAsync(CalendarNextMonthButtonXPath);
                calendarLabel = await GetElementTextAsync(CalendarMonthLabelXPath);
                currentYear = int.Parse(calendarLabel.Split(',').Last().Trim());
            }
            return;
        }
        while (currentYear > year)
        {
            await ClickAsync(CalendarPrevMonthButtonXPath);
            calendarLabel = await GetElementTextAsync(CalendarMonthLabelXPath);
            currentYear = int.Parse(calendarLabel.Split(',').Last().Trim());
        }
    }

    /// <summary>
    /// Navigates the calendar UI to display the specified month asynchronously.
    /// </summary>
    /// <remarks>If the calendar is already displaying the specified month, no action is taken. The method
    /// simulates user interaction by clicking the appropriate navigation buttons to reach the desired month.</remarks>
    /// <param name="month">The target month to navigate to, represented as an integer (1 for January through 12 for December).</param>
    private async Task NavigateToMonth(int month)
    {
        var calendarLabel = await GetElementTextAsync(CalendarMonthLabelXPath);
        var currentMonthString = calendarLabel.Split(',').First().Trim().ToLower();
        var currentMonth = _monthMap[currentMonthString];
        var diff = month - currentMonth;

        if (diff == 0) return;
        if (diff > 0)
        {
            while (diff-- > 0)
                await ClickAsync(CalendarNextMonthButtonXPath);
            return;
        }

        while (diff++ < 0)
            await ClickAsync(CalendarPrevMonthButtonXPath);

    }

    private async Task ClickDay(int day)
    {
        var days = await Page.Locator(CalendarDaysXPath).AllAsync();
        await days[day - 1].ClickAsync();
    }
}

