using Microsoft.Extensions.DependencyInjection;
using RGO.Services.Interfaces;
using RGO.Services.Services;

namespace RGO.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmployeeService, EmployeeService>();

        return services;
    }
}
