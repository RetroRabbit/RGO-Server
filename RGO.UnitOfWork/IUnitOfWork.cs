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
    IEmployeeRepository Employee { get; }
    IEmployeeRoleRepository EmployeeRole { get; }
    IEmployeeTypeRepository EmployeeType { get; }
    IOnboardingDocumentsRepository OnboardingDocuments { get; }
    IRoleAccessRepository RoleAccess { get; }
    IRoleRepository Role { get; }

}