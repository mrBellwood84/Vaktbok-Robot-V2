using Domain.Entities;

namespace Application.DataServices.Interfaces
{
    public interface IDataServiceRegistry
    {
        IDataService<Employee> EmployeeDataService { get; }
        IDataService<ShiftCode> ShiftCodeDataService { get; }
        IShiftDataService ShiftDataService { get; }
        IDataService<ShiftRemark> ShiftRemarkDataService { get; }
        IDataService<Workday> WorkdayDataService { get; }

        Task InitalizeCacheAsync();
    }
}