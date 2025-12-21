using Domain.Entities;
using Domain.Settings;

namespace Persistence.DbServices.Services;

public class FilePathDbService : BaseDbService<FilePath>
{
    public FilePathDbService(ConnectionStrings connectionStrings) 
        : base(connectionStrings)
    {
        QueryAll = @"SELECT * FROM FilePath";
        Insert = @"
            INSERT INTO FilePath (IdBinary, Path)
            VALUES (@IdBinary, @Path)";
    }
}