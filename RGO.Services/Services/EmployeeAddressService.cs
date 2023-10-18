using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services
{
    public class EmployeeAddressService : IEmployeeAddressService
    {
        private readonly IUnitOfWork _db;

        public EmployeeAddressService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<EmployeeAddressDto> SaveEmployeeAddress(EmployeeAddressDto employeeAddressDto)
        {
            var ifEmployee = await CheckEmployee(employeeAddressDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found");}

            EmployeeAddress employeeAddress = new EmployeeAddress(employeeAddressDto);
            var existingAddress = await _db.EmployeeAddress
                .Get(employeeAddress => employeeAddress.EmployeeId == employeeAddressDto.Employee.Id)
                .AsNoTracking()
                .Include(employeeAddress => employeeAddress.Employee)
                .Select(employeeAddress => employeeAddress.ToDto())
                .FirstOrDefaultAsync();

            if (existingAddress != null) { throw new Exception("Existing employee address record found"); }
            var newEmployeeAddress = await _db.EmployeeAddress.Add(employeeAddress);

            return newEmployeeAddress;
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

        public async Task<EmployeeAddressDto> UpdateEmployeeAddress(EmployeeAddressDto employeeAddressDto)
        {
            var ifEmployee = await CheckEmployee(employeeAddressDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found"); }
            EmployeeAddress employeeAddress = new EmployeeAddress(employeeAddressDto);
            var updatedEmployeeAddress = await _db.EmployeeAddress.Update(employeeAddress);

            return updatedEmployeeAddress;
        }

        public async Task<EmployeeAddressDto> DeleteEmployeeAddress(EmployeeAddressDto employeeAddressDto)
        {
            var ifEmployee = await CheckEmployee(employeeAddressDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            EmployeeAddress employeeAddress = new EmployeeAddress(employeeAddressDto);
            var deletedEmployeeAddress = await _db.EmployeeAddress.Delete(employeeAddress.Id);

            return deletedEmployeeAddress;
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
