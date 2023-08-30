using Microsoft.Extensions.DependencyInjection;
using RGO.Services.Interfaces;
using RGO.Services.Services;

namespace RGO.Services;

public static class RegisterServicesExtension
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IEmployeeAddressService, EmployeeAddressService>();
        services.AddScoped<IEmployeeCertificationService, EmployeeCertificationService>();
        services.AddScoped<IEmployeeDocumentService, EmployeeDocumentService>();
        services.AddScoped<IEmployeeDataService, EmployeeDataService>();
        services.AddScoped<IEmployeeTypeService, EmployeeTypeService>();
        services.AddScoped<IOnboardingDocumentService, OnboardingDocumentService>();
        services.AddScoped<IRoleAccessService, RoleAccessService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPropertyAccessService, PropertyAccessService>();
        services.AddScoped<IChartService, ChartService>();
    }
}
