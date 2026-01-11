namespace WebHarvester.Harvest.Interfaces
{
    public interface IShiftBookDailyBot : IBaseBot
    {
        Task<Dictionary<string, string>> GetTableData();
        Task GotoShiftDaily();
        Task NavigateToDate(DateTime date);
    }
}