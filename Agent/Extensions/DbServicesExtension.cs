using Domain.Entities;
using Infrastructure.Persistence.Interfaces;
using Infrastructure.Persistence.Services;
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
        services.AddScoped<IBaseDbService<ShiftRemark>, ShiftRemarkDbService>();
        services.AddScoped<IBaseDbService<Workday>, WorkdayDbService>();
        services.AddScoped<IBaseDbService<Shift>, ShiftDbService>();
        return services;
    }
}