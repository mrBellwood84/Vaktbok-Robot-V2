using Microsoft.Playwright;

namespace WebHarvester.Harvest.Interfaces
{
    public interface IShiftBookDailyBot
    {
        IPage Page { get; set; }

        Task<Dictionary<string, string>> GetTableData();
        Task GotoShiftDaily();
        Task NavigateToDate(DateTime date);
    }
}