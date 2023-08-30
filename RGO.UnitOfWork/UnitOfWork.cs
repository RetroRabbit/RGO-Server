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
    public IEmployeeRepository Employee { get; }
    public IEmployeeRoleRepository EmployeeRole { get; }
    public IEmployeeTypeRepository EmployeeType { get; }
    public IOnboardingDocumentsRepository OnboardingDocuments { get; }
    public IRoleAccessRepository RoleAccess { get; }
    public IRoleRepository Role { get; }
    public IFieldCodeRepository FieldCode { get; }
<<<<<<< Updated upstream
    public IChartRepository Chart { get; }
    public IChartRoleLinkRepositories ChartRoleLink { get; }
=======
    public IFieldCodeOptionsRepository FieldCodeOptions { get; }
>>>>>>> Stashed changes


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
        Employee = new EmployeeRepository(_db);
        EmployeeRole = new EmployeeRoleRepository(_db);
        EmployeeType = new EmployeeTypeRepository(_db);
        OnboardingDocuments = new OnboardingDocumentsRepository(_db);
        RoleAccess = new RoleAccessRepository(_db);
        Role = new RoleRepository(_db);
        FieldCode = new FieldCodeRepository(_db);
<<<<<<< Updated upstream
        Chart= new ChartRepository(_db);
        ChartRoleLink = new ChartRoleLinkRepository(_db);
=======
        FieldCodeOptions = new FieldCodeOptionsRepository(_db);
>>>>>>> Stashed changes
    }

    public async Task Save()
    {
        await _db.SaveChangesAsync();
    }
}