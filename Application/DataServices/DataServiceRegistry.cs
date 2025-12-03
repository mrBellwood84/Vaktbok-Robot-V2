using Application.DataServices.Interfaces;
using Domain.Entities;

namespace Application.DataServices
{
    public class DataServiceRegistry(
        IDataService<Employee> employeeDataService,
        IDataService<ShiftCode> shiftCodeDataService,
        IShiftDataService shiftDataService,
        IDataService<ShiftRemark> shiftRemarkDataService,
        IDataService<Workday> workdayDataService) : IDataServiceRegistry
    {
        public IDataService<Employee> EmployeeDataService => employeeDataService;
        public IDataService<ShiftCode> ShiftCodeDataService => shiftCodeDataService;
        public IShiftDataService ShiftDataService => shiftDataService;
        public IDataService<ShiftRemark> ShiftRemarkDataService => shiftRemarkDataService;
        public IDataService<Workday> WorkdayDataService => workdayDataService;

        public async Task InitalizeCacheAsync()
        {
            await EmployeeDataService.InitCacheAsync();
            await ShiftCodeDataService.InitCacheAsync();
            await ShiftDataService.InitCacheAsync();
            await ShiftRemarkDataService.InitCacheAsync();
            await WorkdayDataService.InitCacheAsync();
        }
    }
}
