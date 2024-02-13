using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var employeesData = await _db.EmployeeData.GetAll();
            var employeeData = employeesData
                .Where(employeeData => employeeData.EmployeeId == employeeDataDto.EmployeeId && employeeData.FieldCodeId == employeeDataDto.FieldCodeId)
                .Select(employeeData => employeeData)
                .FirstOrDefault();

            if (employeeData != null) { throw new Exception("Existing employee data record found"); }
            var newEmployeeData = await _db.EmployeeData.Add(new EmployeeData(employeeDataDto));

            return newEmployeeData;
        }

        public async Task<EmployeeDataDto> GetEmployeeData(int employeeId, string value)
        {

            var employeesData = await _db.EmployeeData.GetAll();
            var employeeData = employeesData
                .Where(employeeData => employeeData.EmployeeId == employeeId && employeeData.Value == value)
                .Select(employeeData => employeeData)
                .FirstOrDefault();
            if (employeeData == null) { throw new Exception("No employee data record found"); }
            return employeeData;
        }

        public async Task<List<EmployeeDataDto>> GetAllEmployeeData(int employeeId)
        {
            var employeesData = await _db.EmployeeData.GetAll();
            var employeeData = employeesData
                .Where(employeeData => employeeData.EmployeeId == employeeId)
                .ToList();
            return employeeData;
        }

        public async Task<EmployeeDataDto> UpdateEmployeeData(EmployeeDataDto employeeDataDto)
        {
            var employeesData = await _db.EmployeeData.GetAll();
            var employeeData = employeesData
                .Where(employeeData => employeeData.Id == employeeDataDto.Id)
                .Select(employeeData => employeeData)
                .FirstOrDefault();

            if (employeeData == null) { throw new Exception("No employee data record found"); }
            var updatedEmployeeData = await _db.EmployeeData.Update(new EmployeeData(employeeDataDto));

            return updatedEmployeeData;
        }

        public async Task<EmployeeDataDto> DeleteEmployeeData(int employeeDataId)
        {
            var deletedData = await _db.EmployeeData.Delete(employeeDataId);
            return deletedData;
        }
    }
}
