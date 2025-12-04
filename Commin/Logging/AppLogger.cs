namespace Common.Logging;

public static class AppLogger
{
    private static readonly string[] _logPrefixes = [
        " [i]", // info, gray
        " [!]", // warning, yellow
        " [✓]", // success, green
        " [+]", // add, green
        " [x]", // failed, red
        ];

    public static void LogInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(_logPrefixes[0]);
        Console.ResetColor();
        Console.WriteLine($" {message}");
    }

    public static void LogWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(_logPrefixes[1]);
        Console.ResetColor();
        Console.WriteLine($" {message}");
    }

    public static void LogSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(_logPrefixes[2]);
        Console.ResetColor();
        Console.WriteLine($" {message}");
    }

    public static void LogAdd(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(_logPrefixes[3]);
        Console.ResetColor();
        Console.WriteLine($" {message}");
    }

    public static void LogFail(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(_logPrefixes[4]);
        Console.ResetColor();
        Console.WriteLine($" {message}");
    }

    public static void LogDev(string message)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($" DEV :: {message} ");
        Console.ResetColor();
    }

}

