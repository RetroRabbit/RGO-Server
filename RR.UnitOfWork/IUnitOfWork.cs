using Npgsql;
using RR.UnitOfWork.Interfaces;
using RR.UnitOfWork.Interfaces.ATS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork;

public interface IUnitOfWork
{
    IAuditLogRepository AuditLog { get; }
    IEmployeeAddressRepository EmployeeAddress { get; }
    IEmployeeCertificationRepository EmployeeCertification { get; }
    IEmployeeDocumentRepository EmployeeDocument { get; }
    IEmployeeDataRepository EmployeeData { get; }
    IEmployeeDateRepository EmployeeDate { get; }
    IEmployeeProjectRepository EmployeeProject { get; }
    IEmployeeQualificationRepository EmployeeQualification { get; }
    IEmployeeEvaluationRepository EmployeeEvaluation { get; }
    IEmployeeEvaluationAudienceRepository EmployeeEvaluationAudience { get; }
    IEmployeeEvaluationRatingRepository EmployeeEvaluationRating { get; }
    IEmployeeEvaluationTemplateRepository EmployeeEvaluationTemplate { get; }
    IEmployeeEvaluationTemplateItemRepository EmployeeEvaluationTemplateItem { get; }
    IEmployeeRepository Employee { get; }
    IEmployeeRoleRepository EmployeeRole { get; }
    IEmployeeTypeRepository EmployeeType { get; }
    IRoleAccessRepository RoleAccess { get; }
    IRoleRepository Role { get; }
    IPropertyAccessRepository PropertyAccess { get; }
    IFieldCodeRepository FieldCode { get; }
    IFieldCodeOptionsRepository FieldCodeOptions { get; }
    IRoleAccessLinkRepository RoleAccessLink { get; }
    IChartRepository Chart { get; }
    IChartRoleLinkRepositories ChartRoleLink { get; }
    IEmployeeBankingRepository EmployeeBanking { get; }
    IClientRepository Client { get; }
    IMonthlyEmployeeTotalRepository MonthlyEmployeeTotal { get; }
    IErrorLoggingRepository ErrorLogging { get; }
    ICandidateRepository Candidate { get; }
    IWorkExperienceRepository WorkExperience { get; }
    Task RawSql(string sql, params NpgsqlParameter[] parameters);
    Task<string> RawSqlGet(string sql, params NpgsqlParameter[] parameters);
    Task<List<string>> GetColumnNames(string tableName);
}
