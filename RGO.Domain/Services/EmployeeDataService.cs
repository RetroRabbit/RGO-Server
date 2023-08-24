using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services
{
    public class EmployeeDataService : IEmployeeDataService
    {
        private readonly IUnitOfWork _db;

        public EmployeeDataService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<EmployeeDataDto> SaveEmployeeData(EmployeeDataDto employeeDataDto)
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
            var newEmployeeData = await _db.EmployeeData.Add(employeeData);

            return newEmployeeData;
        }

        public async Task<EmployeeDataDto> GetEmployeeData(int employeeId, string value)
        {
            var ifEmployee = await CheckEmployee(employeeId);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            var employeeData = await _db.EmployeeData
                .Get(employeeData =>
                    employeeData.EmployeeId == employeeId &&
                    employeeData.Value == value)
                .AsNoTracking()
                .Include(employeeData => employeeData.Employee)
                .Select(employeeData => employeeData.ToDto())
                .Take(1)
                .FirstOrDefaultAsync();

            if (employeeData == null) { throw new Exception("Employee data record not found"); }
            return employeeData;
        }

        public async Task<List<EmployeeDataDto>> GetAllEmployeeData(int employeeId)
        {
            var ifEmployee = await CheckEmployee(employeeId);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            var employeeData = await _db.EmployeeData
                .Get(employeeData => employeeData.EmployeeId == employeeId)
                .AsNoTracking()
                .Include(employeeData => employeeData.Employee)
                .Select(employeeData => employeeData.ToDto())
                .ToListAsync();

            if (employeeData == null) { throw new Exception("Employee data record not found"); }
            return employeeData;
        }

        public async Task<EmployeeDataDto> UpdateEmployeeData(EmployeeDataDto employeeDataDto)
        {
            var ifEmployee = await CheckEmployee(employeeDataDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found"); }
            EmployeeData employeeData = new EmployeeData(employeeDataDto);
            var updatedRmployeeData = await _db.EmployeeData.Update(employeeData);

            return updatedRmployeeData;
        }

        public async Task<EmployeeDataDto> DeleteEmployeeData(EmployeeDataDto employeeDataDto)
        {
            var ifEmployee = await CheckEmployee(employeeDataDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            EmployeeData employeeData = new EmployeeData(employeeDataDto);
            var deletedRmployeeData = await _db.EmployeeData.Delete(employeeData.Id);
            return deletedRmployeeData;
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
