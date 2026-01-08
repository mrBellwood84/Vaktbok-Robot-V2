using Application.DataServices.Interfaces;
using Domain.Entities;
using Persistence.DbServices.Interfaces;

namespace Application.DataServices.Services;

public class ShiftNoRemarkDataService(
    IShiftNoRemarkDbService dbService) : IShiftNoRemarkDataService
{
    /// <summary>
    /// Gets or sets the collection of shift records without remarks.
    /// </summary>
    public List<ShiftNoRemark> Data { get; set; } = [];
    /// <summary>
    /// Gets or sets the collection of all dates associated with this instance.
    /// </summary>
    public HashSet<DateTime> AllDates { get; set; } = [];

    /// <summary>
    /// Asynchronously loads data from the database service and updates the Data and AllDates properties with the
    /// retrieved results.
    /// </summary>
    /// <remarks>After completion, the Data property contains all retrieved records, and the AllDates property
    /// contains a set of all unique dates present in the data. This method should be awaited to ensure that the
    /// properties are updated before accessing them.</remarks>
    /// <returns>A task that represents the asynchronous load operation.</returns>
    public async Task LoadData()
    {
        Data = await dbService.GetAllAsync();
        AllDates = Data.Select(x => x.Date).ToHashSet();
    }

    /// <summary>
    /// Updates the remark associated with a specific shift asynchronously.
    /// </summary>
    /// <param name="ShiftId">The unique identifier of the shift to update. Cannot be null or empty.</param>
    /// <param name="ShiftRemarkId">The identifier of the remark to associate with the shift. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    public async Task UpdateShiftRemarks(string ShiftId, string ShiftRemarkId)
    {
        await dbService.SetShiftRemarkAsync(ShiftId, ShiftRemarkId);
    }
}
