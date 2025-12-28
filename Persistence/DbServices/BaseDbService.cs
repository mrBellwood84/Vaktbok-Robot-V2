using Dapper;
using Domain.Settings;
using Persistence.DbServices.Interfaces;

namespace Persistence.DbServices;

/// <summary>
/// Base dbservice class
/// </summary>
public class BaseDbService<TModel>(ConnectionStrings connectionStrings)
    : DatabaseConnection(connectionStrings.Robot), IBaseDbService<TModel>
{
    /// <summary>
    /// Get all query
    /// </summary>
    internal string QueryAll { get; init; }
    /// <summary>
    /// Gets the SQL query used to retrieve an entity by its binary identifier.
    /// </summary>
    internal string QueryById { get; init; }
    /// <summary>
    /// Command for inserting
    /// </summary>
    internal string Insert { get; init; }

    /// <summary>
    /// Asynchronously retrieves all records of type TModel from the data source.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all TModel records
    /// retrieved from the data source. The list will be empty if no records are found.</returns>
    /// <exception cref="NotSupportedException">Thrown if the query used to retrieve all records is null or empty.</exception>
    public async Task<List<TModel>> GetAllAsync()
    {
        if (string.IsNullOrEmpty(QueryAll))
            throw new NotSupportedException("QueryAll can not be null or empty.");

        await using var connection = await CreateConnectionAsync();
        var data = await connection.QueryAsync<TModel>(QueryAll);
        return data.ToList();
    }

    public async Task<TModel> GetByIdAsync(Guid id)
    {
        await using var connection = await CreateConnectionAsync();
        var data = await connection.QuerySingleOrDefaultAsync<TModel>(QueryById, new { Id = id });
        return data;
    }

    /// <summary>
    /// Insert single entity to database
    /// </summary>
    public async Task CreateAsync(TModel model)
    {
        if (string.IsNullOrEmpty(Insert))
            throw new NullReferenceException("Insert cannot be null or empty.");

        await using var connection = await CreateConnectionAsync();
        await connection.ExecuteAsync(Insert, model);
    }

    /// <summary>
    /// Insert list of data to database
    /// </summary>
    public async Task CreateBulkAsync(IEnumerable<TModel> models)
    {
        if (string.IsNullOrEmpty(Insert))
            throw new NullReferenceException("Insert cannot be null or empty");

        await using var connection = await CreateConnectionAsync();
        await using var transaction = await connection.BeginTransactionAsync();
        await connection.ExecuteAsync(Insert, models, transaction);
        await transaction.CommitAsync();
    }
}