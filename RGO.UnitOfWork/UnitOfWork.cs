using Microsoft.EntityFrameworkCore;
using Npgsql;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;
using RGO.UnitOfWork.Repositories;

namespace RGO.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    public IAuditLogRepository AuditLog { get; }
    public IEmployeeAddressRepository EmployeeAddress { get; }
    public IEmployeeCertificationRepository EmployeeCertification { get; }
    public IEmployeeDataRepository EmployeeData { get; }
    public IEmployeeDocumentRepository EmployeeDocument { get; }
    public IEmployeeProjectRepository EmployeeProject { get; }
    public IEmployeeEvaluationRepository EmployeeEvaluation { get; }
    public IEmployeeEvaluationAudienceRepository EmployeeEvaluationAudience { get; }
    public IEmployeeEvaluationRatingRepository EmployeeEvaluationRating { get; }
    public IEmployeeEvaluationTemplateRepository EmployeeEvaluationTemplate { get; }
    public IEmployeeEvaluationTemplateItemRepository EmployeeEvaluationTemplateItem { get; }
    public IEmployeeRepository Employee { get; }
    public IEmployeeRoleRepository EmployeeRole { get; }
    public IEmployeeTypeRepository EmployeeType { get; }
    public IOnboardingDocumentsRepository OnboardingDocuments { get; }
    public IRoleAccessRepository RoleAccess { get; }
    public IRoleRepository Role { get; }
    public IPropertyAccessRepository PropertyAccess { get; }
    public IRoleAccessLinkRepository RoleAccessLink { get; }
    public IChartRepository Chart { get; }
    public IChartRoleLinkRepositories ChartRoleLink { get; }
    public IFieldCodeRepository FieldCode { get; }
    public IFieldCodeOptionsRepository FieldCodeOptions { get; }
    public IEmployeeBankingRepository EmployeeBanking { get; }

    private readonly DatabaseContext _db;

    public UnitOfWork(DatabaseContext db)
    {
        _db = db;
        AuditLog = new AuditLogRepository(_db);
        EmployeeAddress = new EmployeeAddressRepository(_db);
        EmployeeCertification = new EmployeeCertificationRepository(_db);
        EmployeeDocument = new EmployeeDocumentRepository(_db);
        EmployeeData = new EmployeeDataRepository(_db);
        EmployeeProject = new EmployeeProjectRepository(_db);
        EmployeeEvaluation = new EmployeeEvaluationRepository(_db);
        EmployeeEvaluationAudience = new EmployeeEvaluationAudienceRepository(_db);
        EmployeeEvaluationRating = new EmployeeEvaluationRatingRepository(_db);
        EmployeeEvaluationTemplate = new EmployeeEvaluationTemplateRepository(_db);
        EmployeeEvaluationTemplateItem = new EmployeeEvaluationTemplateItemRepository(_db);
        Employee = new EmployeeRepository(_db);
        EmployeeRole = new EmployeeRoleRepository(_db);
        EmployeeType = new EmployeeTypeRepository(_db);
        OnboardingDocuments = new OnboardingDocumentsRepository(_db);
        RoleAccess = new RoleAccessRepository(_db);
        Role = new RoleRepository(_db);
        PropertyAccess = new PropertyAccessRepository(_db);
        RoleAccessLink = new RoleAccessLinkRepository(_db);
        Chart= new ChartRepository(_db);
        ChartRoleLink = new ChartRoleLinkRepository(_db);
        FieldCode = new FieldCodeRepository(_db);
        FieldCodeOptions = new FieldCodeOptionsRepository(_db);
        EmployeeBanking = new EmployeeBankingRepository(_db);

    }

    public async Task RawSql(string sql, params NpgsqlParameter[] parameters)
    {
        await _db.Database.ExecuteSqlRawAsync(sql, parameters);
    }

    public async Task<string> RawSqlGet(string sql, params NpgsqlParameter[] parameters)
    {
        try
        {
            using (var command = _db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.AddRange(parameters);

                if (command.Connection.State == System.Data.ConnectionState.Closed)
                {
                    await command.Connection.OpenAsync();
                }

                object result = await command.ExecuteScalarAsync();

                return result?.ToString();
            }
        }
        catch (Exception)
        {
            return null;
        }
    }
}