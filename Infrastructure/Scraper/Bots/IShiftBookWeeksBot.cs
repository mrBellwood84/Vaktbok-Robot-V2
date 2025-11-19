using Microsoft.Playwright;

namespace Infrastructure.Scraper.Bots;

public interface IShiftBookWeeksBot
{
    /// <summary>
    /// This action goes to shift book week view
    /// </summary>
    Task GotoShiftBookWeeks();

    /// <summary>
    /// Navigate to page startpoint
    /// </summary>
    Task NavigateToStartPoint();

    IPage Page { get; set; }
}