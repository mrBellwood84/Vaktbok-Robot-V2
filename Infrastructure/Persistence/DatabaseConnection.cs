using MySql.Data.MySqlClient;

namespace Infrastructure.Persistence;

/// <summary>
/// This base class holds method for creating database connection. Is inherited by DbService BaseClass and all
/// derived subclasses.
/// </summary>
public class DatabaseConnection(string connectionString)
{
    /// <summary>
    /// Create a mysql connection
    /// </summary>
    /// <returns></returns>
    protected async Task<MySqlConnection> CreateConnectionAsync() 
    {
        var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();
        return connection;
    }
}