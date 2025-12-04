using Common.Logging;
using DbUp;
using System.Reflection;

namespace Persistence.DbServices;

public static class InitializeDatabase
{
    public static void Setup(string rootConnectionString)
    {
        AppLogger.LogInfo("Starting database migration...");

        var upgrader = DeployChanges.To
            .MySqlDatabase(rootConnectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            AppLogger.LogFail("Database migration failed");
            throw new Exception("Database migration failed", result.Error);
        }

        AppLogger.LogSuccess("Database migration successful\n");
    }
}
