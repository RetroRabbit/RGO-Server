using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RGO.UnitOfWork.Interfaces;
using RGO.UnitOfWork.Repositories;

namespace RGO.UnitOfWork;

public static class DependencyInjection
{
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(configuration["ConnectionStrings:Default"]));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        // services.AddScoped<IEmployeeAddressRepository, EmployeeAddressRepository>();
        // services.AddScoped<IEmployeeCertificationRepository, EmployeeCertificationRepository>();
        // services.AddScoped<IEmployeeDataRepository, EmployeeDataRepository>();
        // services.AddScoped<IEmployeeDocumentRepository, EmployeeDocumentRepository>();
        // services.AddScoped<IEmployeeProjectRepository, EmployeeProjectRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IEmployeeRoleRepository, EmployeeRoleRepository>();
        services.AddScoped<IEmployeeTypeRepository, EmployeeTypeRepository>();
        // services.AddScoped<IOnboardingDocumentsRepository, OnboardingDocumentsRepository>();
        services.AddScoped<IRoleAccessRepository, RoleAccessRepository>();
        // services.AddScoped<IRoleRepository, RoleRepository>();

        return services;
    }
}
