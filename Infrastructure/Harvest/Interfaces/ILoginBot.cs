using Microsoft.Playwright;

namespace WebHarvester.Harvest.Interfaces;

public interface ILoginBot
{
    IPage Page { get; set; }
    Task RunLoginProcedureAsync();
}
