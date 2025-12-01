using Microsoft.Playwright;

namespace Infrastructure.Scraper.Interfaces
{
    public interface ILoginBot
    {
        IPage Page { get; set; }
        Task RunLoginProcedureAsync();
    }
}