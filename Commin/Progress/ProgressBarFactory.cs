using ShellProgressBar;

namespace Common.Progress;

/// <summary>
/// Provides factory methods for creating and configuring instances of the ProgressBar class with predefined options.
/// </summary>
/// <remarks>This static class simplifies the creation of ProgressBar instances by applying consistent styling and
/// configuration. Use this class to ensure all progress bars in your application share the same appearance and
/// behavior.</remarks>
public static class ProgressBarFactory
{
    /// <summary>
    /// Creates a new progress bar with the specified message and total step count.
    /// </summary>
    /// <param name="message">The message to display alongside the progress bar. This typically describes the operation being tracked.</param>
    /// <param name="count">The total number of steps or units of work to represent in the progress bar. Must be greater than zero.</param>
    /// <returns>A new instance of the <see cref="ProgressBar"/> initialized with the specified message and step count.</returns>
    public static ProgressBar CreateProgressBar(string message, int count)
    {
        return new ProgressBar(count, message, CreateProgressBarOptions());
    }

    private static ProgressBarOptions CreateProgressBarOptions()
    {
        return new ProgressBarOptions
        {
            ForegroundColor = ConsoleColor.DarkCyan,
            BackgroundColor = ConsoleColor.White,
            ForegroundColorDone = ConsoleColor.DarkGreen,
            ProgressBarOnBottom = true
        };
    }
}