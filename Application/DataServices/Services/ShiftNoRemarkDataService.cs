using Application.DataServices.Interfaces;
using Domain.Entities;
using Persistence.DbServices.Interfaces;

namespace Application.DataServices.Services;

public class ShiftNoRemarkDataService(
    IBaseDbService<ShiftRemark> remarkDbService,
    IShiftNoRemarkDbService noRemarkDbService) : IShiftNoRemarkDataService
{
    /// <summary>
    /// Gets or sets the collection of shift records without remarks.
    /// </summary>
    public List<ShiftNoRemark> Data { get; set; } = [];
    /// <summary>
    /// Gets or sets the collection of all dates associated with this instance.
    /// </summary>
    public HashSet<DateTime> AllDates { get; set; } = [];

    /// <summary>1
    /// Stores a mapping of remark names to their unique identifiers.
    /// </summary>
    public Dictionary<string, string> RemarksDict = [];

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
        Data = await noRemarkDbService.GetAllAsync();
        AllDates = Data.Select(x => x.Date).ToHashSet();
        var remarks = await remarkDbService.GetAllAsync();
        RemarksDict = remarks.ToDictionary(x => x.Remark, x => x.Id);
    }

    /// <summary>
    /// Retrieves a list of shifts scheduled for the specified date.
    /// </summary>
    /// <param name="date">The date for which to retrieve scheduled shifts.</param>
    /// <returns>A list of <see cref="ShiftNoRemark"/> objects representing the shifts scheduled on the specified date. Returns
    /// an empty list if no shifts are found.</returns>
    public List<ShiftNoRemark> GetShiftsByDate(DateTime date) => Data.Where(x => x.Date == date).ToList();

    /// <summary>
    /// Retrieves the unique identifier associated with the specified remark, creating a new entry if the remark does
    /// not already exist.
    /// </summary>
    /// <remarks>If the remark does not exist, a new unique identifier is generated, stored, and returned.
    /// Subsequent calls with the same remark will return the same identifier.</remarks>
    /// <param name="remark">The text of the remark for which to retrieve or create a unique identifier. Cannot be null.</param>
    /// <returns>A <see cref="Guid"/> representing the unique identifier for the specified remark.</returns>
    public async Task<string> GetRemarkGuid(string remark)
    {
        if (RemarksDict.TryGetValue(remark, out string value))
            return value;

        var newRemark = new ShiftRemark { Guid = Guid.NewGuid(), Remark = remark };
        await remarkDbService.CreateAsync(newRemark);
        RemarksDict[remark] = newRemark.Id;
        return newRemark.Id;
    }

    /// <summary>
    /// Updates the remark associated with a specific shift asynchronously.
    /// </summary>
    /// <param name="ShiftId">The unique identifier of the shift to update. Cannot be null or empty.</param>
    /// <param name="ShiftRemarkId">The identifier of the remark to associate with the shift. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    public async Task UpdateShiftRemarks(string ShiftId, string ShiftRemarkId)
    {
        await noRemarkDbService.SetShiftRemarkAsync(ShiftId, ShiftRemarkId);
    }
}
