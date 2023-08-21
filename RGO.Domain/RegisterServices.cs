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
        services.AddScoped<IEmployeeTypeService, EmployeeTypeService>();
        services.AddScoped<IOnboardingDocumentService, OnboardingDocumentService>();
        services.AddScoped<IRoleAccessService, RoleAccessService>();
        services.AddScoped<IRoleService, RoleService>();
    }
}
