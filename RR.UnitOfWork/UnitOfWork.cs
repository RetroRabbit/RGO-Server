using System.Data;
using HRIS.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using RR.UnitOfWork.Entities;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces;
using RR.UnitOfWork.Interfaces.ATS;
using RR.UnitOfWork.Interfaces.HRIS;
using RR.UnitOfWork.Repositories;
using RR.UnitOfWork.Repositories.ATS;
using RR.UnitOfWork.Repositories.HRIS;

namespace RR.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _db;

    public UnitOfWork(DatabaseContext db)
    {
        _db = db;
        EmployeeAddress = new EmployeeAddressRepository(_db);
        EmployeeCertification = new EmployeeCertificationRepository(_db);
        EmployeeDocument = new EmployeeDocumentRepository(_db);
        EmployeeData = new EmployeeDataRepository(_db);
        EmployeeDate = new EmployeeDateRepository(_db);
        EmployeeProject = new EmployeeProjectRepository(_db);
        EmployeeQualification = new EmployeeQualificationRepository(_db);
        EmployeeEvaluation = new EmployeeEvaluationRepository(_db);
        EmployeeEvaluationAudience = new EmployeeEvaluationAudienceRepository(_db);
        EmployeeEvaluationRating = new EmployeeEvaluationRatingRepository(_db);
        EmployeeEvaluationTemplate = new EmployeeEvaluationTemplateRepository(_db);
        EmployeeEvaluationTemplateItem = new EmployeeEvaluationTemplateItemRepository(_db);
        Employee = new EmployeeRepository(_db);
        EmployeeRole = new EmployeeRoleRepository(_db);
        EmployeeType = new EmployeeTypeRepository(_db);
        RoleAccess = new RoleAccessRepository(_db);
        Role = new RoleRepository(_db);
        PropertyAccess = new PropertyAccessRepository(_db);
        RoleAccessLink = new RoleAccessLinkRepository(_db);
        Chart = new ChartRepository(_db);
        ChartRoleLink = new ChartRoleLinkRepository(_db);
        FieldCode = new FieldCodeRepository(_db);
        FieldCodeOptions = new FieldCodeOptionsRepository(_db);
        EmployeeBanking = new EmployeeBankingRepository(_db);
        Client = new ClientRepository(_db);
        MonthlyEmployeeTotal = new MonthlyEmployeeTotalRepository(_db);
        ErrorLogging = new ErrorLoggingRepository(_db);
        Candidate = new CandidateRepository(_db);
        ClientProject = new ClientProjectRepository(_db);
        WorkExperience = new WorkExperienceRepository(_db);
        EmployeeSalaryDetails = new EmployeeSalaryDetailsRepository(_db);
        Termination = new TerminationRepository(_db);
    }

    public IEmployeeAddressRepository EmployeeAddress { get; }
    public IEmployeeCertificationRepository EmployeeCertification { get; }
    public IEmployeeDataRepository EmployeeData { get; }
    public IEmployeeDateRepository EmployeeDate { get; }
    public IEmployeeDocumentRepository EmployeeDocument { get; }
    public IEmployeeProjectRepository EmployeeProject { get; }
    public IEmployeeQualificationRepository EmployeeQualification { get; }
    public IEmployeeEvaluationRepository EmployeeEvaluation { get; }
    public IEmployeeEvaluationAudienceRepository EmployeeEvaluationAudience { get; }
    public IEmployeeEvaluationRatingRepository EmployeeEvaluationRating { get; }
    public IEmployeeEvaluationTemplateRepository EmployeeEvaluationTemplate { get; }
    public IEmployeeEvaluationTemplateItemRepository EmployeeEvaluationTemplateItem { get; }
    public IEmployeeRepository Employee { get; }
    public IEmployeeRoleRepository EmployeeRole { get; }
    public IEmployeeTypeRepository EmployeeType { get; }
    public IRoleAccessRepository RoleAccess { get; }
    public IRoleRepository Role { get; }
    public IPropertyAccessRepository PropertyAccess { get; }
    public IRoleAccessLinkRepository RoleAccessLink { get; }
    public IChartRepository Chart { get; }
    public IChartRoleLinkRepositories ChartRoleLink { get; }
    public IFieldCodeRepository FieldCode { get; }
    public IFieldCodeOptionsRepository FieldCodeOptions { get; }
    public IEmployeeBankingRepository EmployeeBanking { get; }
    public IClientRepository Client { get; }
    public IMonthlyEmployeeTotalRepository MonthlyEmployeeTotal { get; }
    public IErrorLoggingRepository ErrorLogging { get; }
    public ICandidateRepository Candidate { get; }
    public IClientProjectRepository ClientProject { get; }
    public IEmployeeSalaryDetails EmployeeSalaryDetails { get; }
    public IWorkExperienceRepository WorkExperience { get; }
    public ITerminationRepository Termination { get; }

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

                if (command.Connection!.State == ConnectionState.Closed) await command.Connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();

                return result?.ToString()!;
            }
        }
        catch (Exception)
        {
            return null!;
        }
    }

    public Task<List<string>> GetColumnNames(string tableName)
    {
        var columnsFunc = _db.GetColumnNames(tableName);
        return Task.FromResult(columnsFunc);
    }

    public async Task<int> GetActiveEmployeeId(string email, string role)
    {
        var employee = await (from e in _db.employees
            join er in _db.employeeRoles on e.Id equals er.EmployeeId
            join r in _db.roles on er.RoleId equals r.Id
            where e.Email == email
                  && r.Description == role
            select e).FirstOrDefaultAsync();

        return employee?.Id ?? throw new Exception("Unauthorized Access");
    }
}
