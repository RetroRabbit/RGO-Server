using Microsoft.EntityFrameworkCore;
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
    public class EmployeeDataService : IEmployeeDataService
    {
        private readonly IUnitOfWork _db;

        public EmployeeDataService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task SaveEmployeeData(EmployeeDataDto employeeDataDto)
        {
            var ifEmployeeData = await CheckEmployee(employeeDataDto.Employee.Id);

            if (!ifEmployeeData) { throw new Exception("Employee not found"); }

            EmployeeData employeeData = new EmployeeData(employeeDataDto);
            var existingData = await _db.EmployeeData
                .Get(employeeData => employeeData.EmployeeId == employeeDataDto.Employee.Id)
                .AsNoTracking()
                .Include(employeeData => employeeData.Employee)
                .Select(employeeData => employeeData.ToDto())
                .Take(1)
                .FirstOrDefaultAsync();

            if (existingData != null) { throw new Exception("Existing employee certification record found"); }
            await _db.EmployeeData.Add(employeeData);
        }

        public async Task<EmployeeDataDto> GetEmployeeData(int employeeId)
        {
            var ifEmployee = await CheckEmployee(employeeId);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            var employeeData = await _db.EmployeeData
                .Get(employeeData => employeeData.EmployeeId == employeeId)
                .AsNoTracking()
                .Include(employeeData => employeeData.Employee)
                .Select(employeeData => employeeData.ToDto())
                .Take(1)
                .FirstOrDefaultAsync();

            if (employeeData == null) { throw new Exception("Employee certification record not found"); }
            return employeeData;
        }

        public async Task UpdateEmployeeData(EmployeeDataDto employeeDataDto)
        {
            var ifEmployee = await CheckEmployee(employeeDataDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found"); }
            EmployeeData employeeData = new EmployeeData(employeeDataDto);
            await _db.EmployeeData.Update(employeeData);
        }

        public async Task DeleteEmployeeData(EmployeeDataDto employeeDataDto)
        {
            var ifEmployee = await CheckEmployee(employeeDataDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            EmployeeData employeeData = new EmployeeData(employeeDataDto);
            await _db.EmployeeData.Delete(employeeData.Id);
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
