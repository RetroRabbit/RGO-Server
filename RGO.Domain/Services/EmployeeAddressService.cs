using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RGO.Services.Services
{
    public class EmployeeAddressService : IEmployeeAddressService
    {
        private readonly IUnitOfWork _db;

        public EmployeeAddressService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task SaveEmployeeAddress(EmployeeAddressDto employeeAddressDto)
        {
            var ifEmployee = await CheckEmployee(employeeAddressDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found");}

            EmployeeAddress employeeAddress = new EmployeeAddress(employeeAddressDto);
            var existingAddress = await _db.EmployeeAddress
                .Get(employeeAddress => employeeAddress.EmployeeId == employeeAddressDto.Employee.Id)
                .AsNoTracking()
                .Include(employeeAddress => employeeAddress.Employee)
                .Select(employeeAddress => employeeAddress.ToDto())
                .Take(1)
                .FirstOrDefaultAsync();

            if (existingAddress != null) { throw new Exception("Existing employee address record found"); }
            await _db.EmployeeAddress.Add(employeeAddress);
        }

        public async Task<EmployeeAddressDto> GetEmployeeAddress(int employeeId)
        {
            var ifEmployee = await CheckEmployee(employeeId); 

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            var employeeAddress = await _db.EmployeeAddress
                .Get(employeeAddress => employeeAddress.EmployeeId == employeeId)
                .AsNoTracking()
                .Include(employeeAddress => employeeAddress.Employee)
                .Select(employeeAddress => employeeAddress.ToDto())
                .Take(1)
                .FirstOrDefaultAsync();

            if(employeeAddress == null) { throw new Exception("Employee address record not found"); }
            return employeeAddress;
        }

        public async Task UpdateEmployeeAddress(EmployeeAddressDto employeeAddressDto)
        {
            var ifEmployee = await CheckEmployee(employeeAddressDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found"); }
            EmployeeAddress employeeAddress = new EmployeeAddress(employeeAddressDto);
            await _db.EmployeeAddress.Update(employeeAddress);
        }

        public async Task DeleteEmployeeAddress(EmployeeAddressDto employeeAddressDto)
        {
            var ifEmployee = await CheckEmployee(employeeAddressDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            EmployeeAddress employeeAddress = new EmployeeAddress(employeeAddressDto);
            await _db.EmployeeAddress.Delete(employeeAddress.Id);
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
