using Domain.Entities;

namespace Application.DataServices.Interfaces
{
    public interface IDataServiceRegistry
    {
        IBaseDataService<Employee> EmployeeDataService { get; }
        IBaseDataService<ShiftCode> ShiftCodeDataService { get; }
        IBaseDataService<Shift> ShiftDataService { get; }
        IBaseDataService<ShiftRemark> ShiftRemarkDataService { get; }
        IBaseDataService<Workday> WorkdayDataService { get; }

        Task InitalizeCacheAsync();
    }
}