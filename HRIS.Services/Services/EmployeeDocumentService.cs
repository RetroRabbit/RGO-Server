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
    private readonly IErrorLoggingService _errorLoggingService;

    public EmployeeDocumentService(IUnitOfWork db, IEmployeeService employeeService, IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _employeeService = employeeService;
        _errorLoggingService = errorLoggingService;
    }

    public async Task<EmployeeDocumentDto> SaveEmployeeDocument(SimpleEmployeeDocumentDto employeeDocDto, string email)
    {
        var employee = await _employeeService.GetById(employeeDocDto.EmployeeId);

        if (employee == null)
        {
            var exception = new Exception("employee not found");
            throw _errorLoggingService.LogException(exception);
        }

        bool sameEmail = email.Equals(employee.Email);
        var isAdmin = await IsAdmin(email);
        var status = isAdmin && !sameEmail ? DocumentStatus.ActionRequired : DocumentStatus.PendingApproval;

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
        };

        var newEmployeeDocument = await _db.EmployeeDocument.Add(new EmployeeDocument(employeeDocument));

        return newEmployeeDocument;
    }

    public async Task<EmployeeDocumentDto> GetEmployeeDocument(int employeeId, string filename)
    {
        var ifEmployeeExists = await CheckEmployee(employeeId);

        if (!ifEmployeeExists) 
        { 
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }

        var employeeDocument = await _db.EmployeeDocument
            .Get(employeeDocument =>
                employeeDocument.EmployeeId == employeeId &&
                employeeDocument.FileName!.Equals(filename, StringComparison.CurrentCultureIgnoreCase))
            .AsNoTracking()
            .Include(employeeDocument => employeeDocument.Employee)
            .Select(employeeDocument => employeeDocument.ToDto())
            .Take(1)
            .FirstOrDefaultAsync();

        if (employeeDocument == null) 
        { 
            var exception = new Exception("Employee certification record not found");
            throw _errorLoggingService.LogException(exception);
        }
        return employeeDocument;
    }

    public async Task<List<EmployeeDocumentDto>> GetAllEmployeeDocuments(int employeeId)
    {
        var ifEmployeeExists = await CheckEmployee(employeeId);

        if (!ifEmployeeExists) 
        {
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }

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

        if (!ifEmployeeExists) 
        { 
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }

        EmployeeDocument employeeDocument = new EmployeeDocument(employeeDocumentDto);
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

        if (!ifEmployeeExists) 
        {
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }

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

        if (employee == null) { return false; }
        else { return true; }
    }

    private async Task<bool> IsAdmin(string email)
    {
        EmployeeDto checkingEmployee = (await _employeeService.GetEmployee(email))!;

        EmployeeRole empRole = (await _db.EmployeeRole
            .Get(role => role.EmployeeId == checkingEmployee!.Id)
            .FirstOrDefaultAsync())!;

        Role role = (await _db.Role
            .Get(role => role.Id == empRole!.RoleId)
            .FirstOrDefaultAsync())!;

        return role.Description is "Admin" or "SuperAdmin";
    }
}
