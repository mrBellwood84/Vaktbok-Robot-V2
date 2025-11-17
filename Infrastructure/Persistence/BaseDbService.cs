using Dapper;
using Domain.Settings;

namespace Infrastructure.Persistence;

/// <summary>
/// Base dbservice class
/// </summary>
public class BaseDbService<TModel>(ConnectionStrings connectionStrings)
    : DatabaseConnection(connectionStrings.Default)
{
    /// <summary>
    /// Get all query
    /// </summary>
    internal string QueryAll { get; init; }
    
    /// <summary>
    /// Command for inserting
    /// </summary>
    internal string Insert { get; init; }

    public async Task<List<TModel>> GetAllAsync()
    {
        if (string.IsNullOrEmpty(QueryAll))
            throw new NotSupportedException("QueryAll can not be null or empty.");
        
        await using var connection = await CreateConnectionAsync();
        var data =  await connection.QueryAsync<TModel>(QueryAll);
        return data.ToList();
    }

    /// <summary>
    /// Insert single entity to database
    /// </summary>
    public async Task InsertAsync(TModel model)
    {
        if (string.IsNullOrEmpty(Insert))
            throw new NullReferenceException("Insert cannot be null or empty.");
        
        await using var connection = await CreateConnectionAsync();
        await connection.ExecuteAsync(QueryAll, model);
    }
    
    /// <summary>
    /// Insert list of data to database
    /// </summary>
    public async Task InsertBulkAsync(IEnumerable<TModel> models)
    {
        if (string.IsNullOrEmpty(Insert))
            throw new NullReferenceException("Insert cannot be null or empty");

        await using var connection = await CreateConnectionAsync();
        await using var transaction = await connection.BeginTransactionAsync();
        await connection.ExecuteAsync(Insert, models, transaction);
        await transaction.CommitAsync();
    }
}