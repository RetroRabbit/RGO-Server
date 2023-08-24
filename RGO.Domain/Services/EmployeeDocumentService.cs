﻿using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services
{
    public class EmployeeDocumentService : IEmployeeDocumentService
    {
        private readonly IUnitOfWork _db;

        public EmployeeDocumentService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<EmployeeDocumentDto> SaveEmployeeDocument(EmployeeDocumentDto employeeDocumentDto)
        {
            var ifEmployee = await CheckEmployee(employeeDocumentDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            EmployeeDocument employeeDocument = new EmployeeDocument(employeeDocumentDto);
            var existingDocument = await _db.EmployeeDocument
                .Get(employeeDocument => employeeDocument.EmployeeId == employeeDocumentDto.Employee.Id)
                .AsNoTracking()
                .Include(employeeDocument => employeeDocument.Employee)
                .Select(employeeDocument => employeeDocument.ToDto())
                .Take(1)
                .FirstOrDefaultAsync();

            if (existingDocument != null) { throw new Exception("Existing employee certification record found"); }
            var newRmployeeDocument = await _db.EmployeeDocument.Add(employeeDocument);

            return newRmployeeDocument;
        }

        public async Task<EmployeeDocumentDto> GetEmployeeDocument(int employeeId, string filename)
        {
            var ifEmployee = await CheckEmployee(employeeId);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            var employeeDocument = await _db.EmployeeDocument
                .Get(employeeDocument =>
                    employeeDocument.EmployeeId == employeeId &&
                    employeeDocument.FileName.Equals(filename, StringComparison.CurrentCultureIgnoreCase))
                .AsNoTracking()
                .Include(employeeDocument => employeeDocument.Employee)
                .Select(employeeDocument => employeeDocument.ToDto())
                .Take(1)
                .FirstOrDefaultAsync();

            if (employeeDocument == null) { throw new Exception("Employee certification record not found"); }
            return employeeDocument;
        }

        public async Task<List<EmployeeDocumentDto>> GetAllEmployeeDocuments(int employeeId)
        {
            var ifEmployee = await CheckEmployee(employeeId);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

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
            var ifEmployee = await CheckEmployee(employeeDocumentDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            EmployeeDocument employeeDocument = new EmployeeDocument(employeeDocumentDto);
            var updatedEmployeeDocument = await _db.EmployeeDocument.Update(employeeDocument);

            return updatedEmployeeDocument;
        }

        public async Task<EmployeeDocumentDto> DeleteEmployeeDocument(EmployeeDocumentDto employeeDocumentDto)
        {
            var ifEmployee = await CheckEmployee(employeeDocumentDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            EmployeeDocument employeeDocument = new EmployeeDocument(employeeDocumentDto);
            var deletedEmployeeDocument = await _db.EmployeeDocument.Delete(employeeDocument.Id);

            return deletedEmployeeDocument;
        }

        private async Task<bool> CheckEmployee(int employeeId)
        {
            var employee = await _db.Employee
            .Get(employee => employee.Id == employeeId)
            .FirstOrDefaultAsync();

            if (employee == null) { return false; }
            else { return true; }
        }
    }
}