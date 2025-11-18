using Microsoft.Playwright;

namespace Infrastructure.Scraper.Interfaces;

public interface ILoginBot
{
    Task RunLoginProcedureAsync();
    IPage Page { get; set; }
}