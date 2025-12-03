using Domain.Entities;

namespace Infrastructure.Caching
{
    public class EmployeeCacheService : BaseCacheService<Employee> { }
    public class ShiftCacheService : BaseCacheService<Shift> { }
    public class ShiftCodeCacheService : BaseCacheService<ShiftCode> { }
    public class ShiftRemarkCacheService : BaseCacheService<ShiftRemark> { }
    public class WorkdayCacheService : BaseCacheService<Workday> { }
}
