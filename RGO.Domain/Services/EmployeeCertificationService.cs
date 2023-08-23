using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services
{
    public class EmployeeCertificationService : IEmployeeCertificationService
    {
        private readonly IUnitOfWork _db;

        public EmployeeCertificationService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task SaveEmployeeCertification(EmployeeCertificationDto employeeCertificationDto)
        {
            var ifEmployee = await CheckEmployee(employeeCertificationDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            EmployeeCertification employeeCertification = new EmployeeCertification(employeeCertificationDto);
            var existingCertification = await _db.EmployeeCertification
                .Get(employeeCertification => employeeCertification.EmployeeId == employeeCertificationDto.Employee.Id)
                .AsNoTracking()
                .Include(employeeCertification => employeeCertification.Employee)
                .Select(employeeCertification => employeeCertification.ToDto())
                .Take(1)
                .FirstOrDefaultAsync();

            if (existingCertification != null) { throw new Exception("Existing employee certification record found"); }
            await _db.EmployeeCertification.Add(employeeCertification);
        }

        public async Task<EmployeeCertificationDto> GetEmployeeCertification(int employeeId)
        {
            var ifEmployee = await CheckEmployee(employeeId);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            var employeeCertification = await _db.EmployeeCertification
                .Get(employeeCertification => employeeCertification.EmployeeId == employeeId)
                .AsNoTracking()
                .Include(employeeCertification => employeeCertification.Employee)
                .Select(employeeCertification => employeeCertification.ToDto())
                .Take(1)
                .FirstOrDefaultAsync();

            if (employeeCertification == null) { throw new Exception("Employee certification record not found"); }
            return employeeCertification;
        }

        public async Task UpdateEmployeeCertification(EmployeeCertificationDto employeeCertificationDto)
        {
            var ifEmployee = await CheckEmployee(employeeCertificationDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found"); }
            EmployeeCertification employeeCertification = new EmployeeCertification(employeeCertificationDto);
            await _db.EmployeeCertification.Update(employeeCertification);
        }

        public async Task DeleteEmployeeCertification(EmployeeCertificationDto employeeCertificationDto)
        {
            var ifEmployee = await CheckEmployee(employeeCertificationDto.Employee.Id);

            if (!ifEmployee) { throw new Exception("Employee not found"); }

            EmployeeCertification employeeCertification = new EmployeeCertification(employeeCertificationDto);
            await _db.EmployeeAddress.Delete(employeeCertification.Id);
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
