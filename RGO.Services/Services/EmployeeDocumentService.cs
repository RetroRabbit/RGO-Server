using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeDocumentService : IEmployeeDocumentService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeService _employeeService;

    public EmployeeDocumentService(IUnitOfWork db, IEmployeeService employeeService)
    {
        _db = db;
        _employeeService = employeeService;
    }

    public async Task<EmployeeDocumentDto> SaveEmployeeDocument(SimpleEmployeeDocumentDto employeeDocDto)
    {
        var employee = await _employeeService.GetById(employeeDocDto.EmployeeId);

        if (employee == null) throw new Exception("employee not found");

        var employeeDocument = new EmployeeDocumentDto(
                                                       employeeDocDto.Id,
                                                       employee.Id,
                                                       null,
                                                       employeeDocDto.FileName,
                                                       employeeDocDto.FileCategory,
                                                       employeeDocDto.File,
                                                       DocumentStatus.PendingApproval,
                                                       DateTime.Now,
                                                       null,
                                                       false);

        var newEmployeeDocument = await _db.EmployeeDocument.Add(new EmployeeDocument(employeeDocument));

        return newEmployeeDocument;
    }

    public async Task<EmployeeDocumentDto> GetEmployeeDocument(int employeeId, string filename)
    {
        var ifEmployeeExists = await CheckEmployee(employeeId);

        if (!ifEmployeeExists) throw new Exception("Employee not found");

        var employeeDocument = await _db.EmployeeDocument
                                        .Get(employeeDocument =>
                                                 employeeDocument.EmployeeId == employeeId &&
                                                 employeeDocument.FileName.Equals(filename,
                                                  StringComparison.CurrentCultureIgnoreCase))
                                        .AsNoTracking()
                                        .Include(employeeDocument => employeeDocument.Employee)
                                        .Select(employeeDocument => employeeDocument.ToDto())
                                        .Take(1)
                                        .FirstOrDefaultAsync();

        if (employeeDocument == null) throw new Exception("Employee certification record not found");
        return employeeDocument;
    }

    public async Task<List<EmployeeDocumentDto>> GetAllEmployeeDocuments(int employeeId)
    {
        var ifEmployeeExists = await CheckEmployee(employeeId);

        if (!ifEmployeeExists) throw new Exception("Employee not found");

        var employeeDocuments = await _db.EmployeeDocument
                                         .Get(employeeDocument => employeeDocument.EmployeeId == employeeId)
                                         .AsNoTracking()
                                         .Include(employeeDocument => employeeDocument.Employee)
                                         .Select(employeeDocument => employeeDocument.ToDto())
                                         .ToListAsync();

        return employeeDocuments;
    }

    public async Task<EmployeeDocumentDto> UpdateEmployeeDocument(EmployeeDocumentDto employeeDocumentDto)
    {
        var ifEmployeeExists = await CheckEmployee(employeeDocumentDto.EmployeeId);

        if (!ifEmployeeExists) throw new Exception("Employee not found");

        var employeeDocument = new EmployeeDocument(employeeDocumentDto);
        var updatedEmployeeDocument = await _db.EmployeeDocument.Update(employeeDocument);

        return updatedEmployeeDocument;
    }

    public async Task<EmployeeDocumentDto> DeleteEmployeeDocument(int documentId)
    {
        var deletedEmployeeDocument = await _db.EmployeeDocument.Delete(documentId);

        return deletedEmployeeDocument;
    }

    public async Task<List<EmployeeDocumentDto>> GetEmployeeDocumentsByStatus(int employeeId, DocumentStatus status)
    {
        var ifEmployeeExists = await CheckEmployee(employeeId);

        if (!ifEmployeeExists) throw new Exception("Employee not found");

        var employeeDocuments = await _db.EmployeeDocument
                                         .Get(employeeDocument => employeeDocument.EmployeeId == employeeId &&
                                                                  employeeDocument.Status.Equals(status))
                                         .AsNoTracking()
                                         .Include(employeeDocument => employeeDocument.Employee)
                                         .Select(employeeDocument => employeeDocument.ToDto())
                                         .ToListAsync();

        return employeeDocuments;
    }

    public async Task<bool> CheckEmployee(int employeeId)
    {
        var employee = await _db.Employee
                                .Get(employee => employee.Id == employeeId)
                                .FirstOrDefaultAsync();

        if (employee == null)
            return false;
        return true;
    }
}