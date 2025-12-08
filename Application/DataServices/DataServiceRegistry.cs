using Application.DataServices.Interfaces;
using Domain.Entities;

namespace Application.DataServices
{
    public class DataServiceRegistry(
        IBaseDataService<Employee> employeeDataService,
        IBaseDataService<ShiftCode> shiftCodeDataService,
        IBaseDataService<Shift> shiftDataService,
        IBaseDataService<ShiftRemark> shiftRemarkDataService,
        IBaseDataService<Workday> workdayDataService) : IDataServiceRegistry
    {
        public IBaseDataService<Employee> EmployeeDataService => employeeDataService;
        public IBaseDataService<ShiftCode> ShiftCodeDataService => shiftCodeDataService;
        public IBaseDataService<Shift> ShiftDataService => shiftDataService;
        public IBaseDataService<ShiftRemark> ShiftRemarkDataService => shiftRemarkDataService;
        public IBaseDataService<Workday> WorkdayDataService => workdayDataService;

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
