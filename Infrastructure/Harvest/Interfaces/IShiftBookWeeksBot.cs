using Domain.SourceModels;
using Microsoft.Playwright;

namespace WebHarvester.Harvest.Interfaces;

public interface IShiftBookWeeksBot
{
    IPage Page { get; set; }
    int CurrentWeekNumber { get; set; }
    Task<bool> CheckEndpointReached();
    Task ClickNextWeek();
    Task<List<SourceEmployeeWeek>> CollectWeekData();
    Task GotoShiftBookWeeks();
    Task NavigateToStartPoint();
}
