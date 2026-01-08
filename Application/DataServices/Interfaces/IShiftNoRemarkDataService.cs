using Domain.Entities;

namespace Application.DataServices.Interfaces
{
    public interface IShiftNoRemarkDataService
    {
        HashSet<DateTime> AllDates { get; set; }
        List<ShiftNoRemark> Data { get; set; }

        Task LoadData();
        Task UpdateShiftRemarks(string ShiftId, string ShiftRemarkId);
    }
}