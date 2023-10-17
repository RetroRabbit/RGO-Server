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
        services.AddScoped<IEmployeeEvaluationService, EmployeeEvaluationService>();
        services.AddScoped<IEmployeeEvaluationAudienceService, EmployeeEvaluationAudienceService>();
        services.AddScoped<IEmployeeEvaluationRatingService, EmployeeEvaluationRatingService>();
        services.AddScoped<IEmployeeEvaluationTemplateService, EmployeeEvaluationTemplateService>();
        services.AddScoped<IEmployeeEvaluationTemplateItemService, EmployeeEvaluationTemplateItemService>();
        services.AddScoped<IEmployeeDocumentService, EmployeeDocumentService>();
        services.AddScoped<IEmployeeDataService, EmployeeDataService>();
        services.AddScoped<IEmployeeDateService, EmployeeDateService>();
        services.AddScoped<IEmployeeTypeService, EmployeeTypeService>();
        services.AddScoped<IOnboardingDocumentService, OnboardingDocumentService>();
        services.AddScoped<IRoleAccessService, RoleAccessService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPropertyAccessService, PropertyAccessService>();
        services.AddScoped<IRoleAccessLinkService, RoleAccessLinkService>();
        services.AddScoped<IEmployeeRoleService, EmployeeRoleService>();
        services.AddScoped<IChartService, ChartService>();
        services.AddScoped<IFieldCodeService,FieldCodeService>();
        services.AddScoped<IFieldCodeOptionsService, FieldCodeOptionsService>();
        services.AddScoped<IEmployeeBankingService, EmployeeBankingService>();
        services.AddScoped<IClientService, ClientService>();
    }
}
