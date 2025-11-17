using Domain.Enums;

namespace Domain.Settings;

/// <summary>
/// Settings for browser when creating and running instances and loading content in pages.
/// </summary>
public class BrowserSettings
{
    public bool Headless { get; init; }
    public int SlowMo { get; init; }
    public BrowserType BrowserType { get; init; }
    public int PageLoadTimeout { get; init; }
    public int ContentLoadTimeout { get; init; }
}