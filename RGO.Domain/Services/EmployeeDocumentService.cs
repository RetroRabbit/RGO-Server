﻿using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Services
{
    public class EmployeeDocumentService : IEmployeeDocumentService
    {
        private readonly IUnitOfWork _db;

        public EmployeeDocumentService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task SaveEmployeeDocument(EmployeeDocumentDto employeeDocumentDto) 
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
            await _db.EmployeeDocument.Add(employeeDocument);
        }

        public async Task<EmployeeDocumentDto> GetEmployeeDocument(int employeeId)
        {
            var ifEmployee = await CheckEmployee(employeeId);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            var employeeDocument = await _db.EmployeeDocument
                .Get(employeeDocument => employeeDocument.EmployeeId == employeeId)
                .AsNoTracking()
                .Include(employeeDocument => employeeDocument.Employee)
                .Select(employeeDocument => employeeDocument.ToDto())
                .Take(1)
                .FirstOrDefaultAsync();

            if (employeeDocument == null) { throw new Exception("Employee certification record not found"); }
            return employeeDocument;
        }

        public async Task UpdateEmployeeDocument(EmployeeDocumentDto employeeDocumentDto)
        {
            var ifEmployee = await CheckEmployee(employeeDocumentDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found"); }
            EmployeeDocument employeeDocument = new EmployeeDocument(employeeDocumentDto);
            await _db.EmployeeDocument.Update(employeeDocument);
        }

        public async Task DeleteEmployeeDocument(EmployeeDocumentDto employeeDocumentDto)
        {
            var ifEmployee = await CheckEmployee(employeeDocumentDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            EmployeeDocument employeeDocument = new EmployeeDocument(employeeDocumentDto);
            await _db.EmployeeAddress.Delete(employeeDocument.Id);
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
