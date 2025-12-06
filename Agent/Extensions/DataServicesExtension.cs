using Application.DataServices;
using Application.DataServices.Interfaces;
using Application.DataServices.Services;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Agent.Extensions;

public static class DataServicesExtension
{
    /// <summary>
    /// Add all data services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        // add single data services
        services.AddScoped<IDataService<Employee>, EmployeeDataService>();
        services.AddScoped<IDataService<ShiftCode>, ShiftcodeDataService>();
        services.AddScoped<IShiftDataService, ShiftDataService>();
        services.AddScoped<IDataService<ShiftRemark>, ShiftRemarkDataService>();
        services.AddScoped<IDataService<Workday>, WorkdayDataService>();

        // add registry
        services.AddScoped<IDataServiceRegistry, DataServiceRegistry>();

        return services;
    }

}