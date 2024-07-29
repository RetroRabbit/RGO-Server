﻿using HRIS.Models;
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

    public async Task<EmployeeDocumentDto> SaveEmployeeDocument(SimpleEmployeeDocumentDto employeeDocDto, string email, int documentType)
    {
        var employee = await _employeeService.GetEmployeeById(employeeDocDto.EmployeeId);

        if (employee == null)
            throw new CustomException("employee not found");

        var sameEmail = email.Equals(employee.Email);
        var isAdmin = await IsAdmin(email);
        var status = isAdmin && !sameEmail ? DocumentStatus.ActionRequired : DocumentStatus.PendingApproval;
        var docType = DocumentType.StarterKit;

        switch (documentType)
        {
            case 0:
                docType = DocumentType.StarterKit;
                break;
            case 1:
                docType = DocumentType.MyDocuments;
                break;
            case 2:
                docType = DocumentType.Administrative;
                break;
            case 3:
                docType = DocumentType.EmployeeDocuments;
                break;
            case 4:
                docType = DocumentType.AdditionalDocuments;
                break;
            default:
                docType = DocumentType.StarterKit;
                break;
        }

        if (docType != DocumentType.StarterKit)
            employeeDocDto.FileCategory = 0;

        var employeeDocument = new EmployeeDocumentDto
        {
            Id = employeeDocDto.Id,
            EmployeeId = employee.Id,
            Reference = employeeDocDto.Reference,
            FileName = employeeDocDto.FileName,
            FileCategory = employeeDocDto.FileCategory,
            EmployeeFileCategory = (EmployeeFileCategory)employeeDocDto.EmployeeFileCategory,
            AdminFileCategory = (AdminFileCategory)employeeDocDto.AdminFileCategory,
            Blob = employeeDocDto.Blob,
            Status = status,
            UploadDate = DateTime.Now,
            CounterSign = false,
            DocumentType = docType,
        };

        return (await _db.EmployeeDocument.Add(new EmployeeDocument(employeeDocument))).ToDto();
    }

    public async Task<EmployeeDocumentDto> addNewAdditionalDocument(SimpleEmployeeDocumentDto employeeDocDto, string email, int documentType)
    {
        var employee = await _employeeService.GetEmployeeById(employeeDocDto.EmployeeId);

        if (employee == null)
            throw new CustomException("employee not found");

        var sameEmail = email.Equals(employee.Email);
        var isAdmin = await IsAdmin(email);
        var status = isAdmin && !sameEmail ? DocumentStatus.ActionRequired : DocumentStatus.PendingApproval;
        var docType = documentType == 0 ? DocumentType.StarterKit : DocumentType.MyDocuments;

        var employeeDocument = new EmployeeDocumentDto
        {
            Id = employeeDocDto.Id,
            EmployeeId = employee.Id,
            FileName = employeeDocDto.FileName,
            FileCategory = employeeDocDto.FileCategory,
            Blob = employeeDocDto.Blob,
            Status = status,
            UploadDate = DateTime.Now,
            CounterSign = false,
            DocumentType = docType,
            LastUpdatedDate = DateTime.Now,
        };

        var newEmployeeDocument = await _db.EmployeeDocument.Add(new EmployeeDocument(employeeDocument));

        return newEmployeeDocument.ToDto();
    }

    public async Task<EmployeeDocumentDto> GetEmployeeDocument(int employeeId, string filename, DocumentType documentType)
    {
        var ifEmployeeExists = await CheckEmployee(employeeId);

        if (!ifEmployeeExists)
            throw new CustomException("Employee not found");

        var employeeDocument = await _db.EmployeeDocument
            .Get(employeeDocument =>
                employeeDocument.EmployeeId == employeeId &&
                employeeDocument.FileName!.Equals(filename, StringComparison.CurrentCultureIgnoreCase) &&
                employeeDocument.DocumentType == documentType)
            .AsNoTracking()
            .Include(employeeDocument => employeeDocument.Employee)
            .Select(employeeDocument => employeeDocument.ToDto())
            .Take(1)
            .FirstOrDefaultAsync();

        if (employeeDocument == null)
            throw new CustomException("Employee certification record not found");

        return employeeDocument;
    }

    public async Task<List<EmployeeDocumentDto>> GetEmployeeDocuments(int employeeId, DocumentType documentType)
    {
        var ifEmployeeExists = await CheckEmployee(employeeId);

        if (!ifEmployeeExists)
            throw new CustomException("Employee not found");

        var employeeDocuments = await _db.EmployeeDocument
               .Get(employeeDocument => true)
               .Where(employeeDocument =>
                                      (employeeId == 0 || employeeDocument.EmployeeId == employeeId)
                                      && (employeeDocument.DocumentType! == documentType))
               .AsNoTracking()
               .Include(employeeDocument => employeeDocument.Employee)
               .Select(employeeDocument => employeeDocument.ToDto())
               .ToListAsync();

        return employeeDocuments;
    }

    public async Task<EmployeeDocumentDto> UpdateEmployeeDocument(EmployeeDocumentDto employeeDocumentDto, string email)
    {
        var ifEmployeeExists = await CheckEmployee(employeeDocumentDto.EmployeeId);

        if (!ifEmployeeExists)
            throw new CustomException("Employee not found");

        var employeeEmail = await _db.Employee
            .Get(employee => employee.Id == employeeDocumentDto.EmployeeId)
            .AsNoTracking()
            .Select(employee => employee.Email)
            .FirstAsync();


        var sameEmail = email.Equals(employeeEmail);
        var isAdmin = await IsAdmin(email);
        if (isAdmin && !sameEmail) employeeDocumentDto.Status = DocumentStatus.ActionRequired;

        var employeeDocument = new EmployeeDocument(employeeDocumentDto);

        var updatedEmployeeDocument = await _db.EmployeeDocument.Update(employeeDocument);

        return updatedEmployeeDocument.ToDto();
    }

    public async Task<EmployeeDocumentDto> DeleteEmployeeDocument(int documentId)
    {
        var deletedEmployeeDocument = await _db.EmployeeDocument.Delete(documentId);

        return deletedEmployeeDocument.ToDto();
    }

    public async Task<List<EmployeeDocumentDto>> GetEmployeeDocumentsByStatus(int employeeId, DocumentStatus status)
    {
        var ifEmployeeExists = await CheckEmployee(employeeId);

        if (!ifEmployeeExists)
            throw new CustomException("Employee not found");

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

        return employee != null;
    }

    public async Task<bool> IsAdmin(string email)
    {
        var checkingEmployee = (await _employeeService.GetEmployeeByEmail(email))!;

        var empRole = (await _db.EmployeeRole
            .Get(role => role.EmployeeId == checkingEmployee!.Id)
            .FirstOrDefaultAsync())!;

        var role = (await _db.Role
            .Get(role => role.Id == empRole!.RoleId)
            .FirstOrDefaultAsync())!;

        return role.Description is "Admin" or "SuperAdmin";
    }

    public async Task<List<SimpleEmployeeDocumentGetAllDto>> GetAllDocuments()
    {
        return await _db.EmployeeDocument
            .Get(employeeDocument => true)
            .AsNoTracking()
            .Include(entry => entry.Employee)
            .Include(entry => entry.Employee.EmployeeType)
            .OrderBy(employeeDocument => employeeDocument.EmployeeId)
            .Select(employeeDocument => new SimpleEmployeeDocumentGetAllDto
            {
                EmployeeDocumentDto = employeeDocument.ToDto(),
                Name = employeeDocument.Employee.Name,
                Surname = employeeDocument.Employee.Surname
            })
            .ToListAsync();
    }
}
