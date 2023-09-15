using Npgsql;
using RGO.UnitOfWork.Interfaces;
using System.Net;

namespace RGO.UnitOfWork;

public interface IUnitOfWork
{
    IAuditLogRepository AuditLog { get; }
    IEmployeeAddressRepository EmployeeAddress { get; }
    IEmployeeCertificationRepository EmployeeCertification { get; }
    IEmployeeDocumentRepository EmployeeDocument { get; }
    IEmployeeDataRepository EmployeeData { get; }
    IEmployeeProjectRepository EmployeeProject { get; }
    IEmployeeEvaluationRepository EmployeeEvaluation { get; }
    IEmployeeEvaluationRatingRepository EmployeeEvaluationRating { get; }
    IEmployeeEvaluationTemplateRepository EmployeeEvaluationTemplate { get; }
    IEmployeeEvaluationTemplateItemRepository EmployeeEvaluationTemplateItem { get; }
    IEmployeeRepository Employee { get; }
    IEmployeeRoleRepository EmployeeRole { get; }
    IEmployeeTypeRepository EmployeeType { get; }
    IOnboardingDocumentsRepository OnboardingDocuments { get; }
    IRoleAccessRepository RoleAccess { get; }
    IRoleRepository Role { get; }
    IPropertyAccessRepository PropertyAccess { get; }
    IFieldCodeRepository FieldCode { get; }
    IFieldCodeOptionsRepository FieldCodeOptions { get; }
    IRoleAccessLinkRepository RoleAccessLink { get; }
    IChartRepository Chart { get; }
    IChartRoleLinkRepositories ChartRoleLink { get; }

    Task RawSql(string sql, params NpgsqlParameter[] parameters);
    Task<string> RawSqlGet(string sql, params NpgsqlParameter[] parameters);
}