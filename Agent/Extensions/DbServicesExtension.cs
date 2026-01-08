using Domain.Entities;
using Persistence.DbServices.Interfaces;
using Persistence.DbServices.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Agent.Extensions;

public static class DbServicesExtension
{
    /// <summary>
    /// Add all database services
    /// </summary>
    public static IServiceCollection AddDbServices(this IServiceCollection services)
    {
        services.AddScoped<IBaseDbService<Employee>, EmployeeDbService>();
        services.AddScoped<IBaseDbService<ShiftCode>, ShiftCodeDbService>();
        services.AddScoped<IShiftNoRemarkDbService, ShiftNoRemarkDbService>();
        services.AddScoped<IBaseDbService<ShiftRemark>, ShiftRemarkDbService>();
        services.AddScoped<IBaseDbService<Workday>, WorkdayDbService>();
        services.AddScoped<IBaseDbService<Shift>, ShiftDbService>();
        services.AddScoped<IBaseDbService<FilePath>, FilePathDbService>();
        return services;
    }
}