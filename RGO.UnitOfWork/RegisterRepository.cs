using Microsoft.Extensions.DependencyInjection;
using RGO.UnitOfWork.Interfaces;
using RGO.UnitOfWork.Repositories;

namespace RGO.UnitOfWork;

public static class RegisterRepositoryExtension
{
    public static void RegisterRepository(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmployeeAddressRepository, EmployeeAddressRepository>();
        services.AddScoped<IEmployeeCertificationRepository, EmployeeCertificationRepository>();
        services.AddScoped<IEmployeeDataRepository, EmployeeDataRepository>();
        services.AddScoped<IEmployeeDocumentRepository, EmployeeDocumentRepository>();
        services.AddScoped<IEmployeeProjectRepository, EmployeeProjectRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IEmployeeRoleRepository, EmployeeRoleRepository>();
        services.AddScoped<IEmployeeTypeRepository, EmployeeTypeRepository>();
        services.AddScoped<IOnboardingDocumentsRepository, OnboardingDocumentsRepository>();
        services.AddScoped<IRoleAccessLinkRepository, RoleAccessLinkRepository>();
        services.AddScoped<IRoleAccessRepository, RoleAccessRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IChartRepository, ChartRepository>();
        services.AddScoped<IChartRoleLinkRepositories, ChartRoleLinkRepository>();
        services.AddScoped<IFieldCodeRepository, FieldCodeRepository>();
        services.AddScoped<IFieldCodeOptionsRepository, FieldCodeOptionsRepository>();
    }
}
