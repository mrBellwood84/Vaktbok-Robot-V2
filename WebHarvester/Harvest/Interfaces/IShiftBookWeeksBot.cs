using Domain.SourceModels;

namespace WebHarvester.Harvest.Interfaces
{
    public interface IShiftBookWeeksBot : IBaseBot
    {
        int CurrentWeekNumber { get; set; }

        Task<bool> CheckEndpointReached();
        Task ClickNextWeek();
        Task ClickOrderTableByName();
        Task ClickPrintPageButton();
        Task<List<SourceEmployeeWeek>> CollectWeekData();
        Task GotoShiftBookWeeks();
        Task NavigateToStartPoint();
    }
}