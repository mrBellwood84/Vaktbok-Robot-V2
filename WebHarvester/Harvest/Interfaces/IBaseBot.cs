using Microsoft.Playwright;

namespace WebHarvester.Harvest.Interfaces;

public interface IBaseBot
{
    IPage Page { get; set; }
    public Task GotoAsync(string url);
}