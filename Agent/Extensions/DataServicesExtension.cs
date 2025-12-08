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
        services.AddScoped<IBaseDataService<Employee>, EmployeeDataService>();
        services.AddScoped<IBaseDataService<ShiftCode>, ShiftCodeDataService>();
        services.AddScoped<IBaseDataService<Shift>, ShiftDataService>();
        services.AddScoped<IBaseDataService<ShiftRemark>, ShiftRemarkDataService>();
        services.AddScoped<IBaseDataService<Workday>, WorkdayDataService>();

        // add registry
        services.AddScoped<IDataServiceRegistry, DataServiceRegistry>();

        return services;
    }

}