using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository) 
        {
            _employeeRepository = employeeRepository;
        }
        public async Task<EmployeeDto> GetEmployee(string email)
        {
            var employee = await _employeeRepository
                .Get(employee => employee.PersonalEmail == email)
                .Select(employee => employee.ToDto())
                .Take(1)
                .FirstOrDefaultAsync();
            if (employee == null) { throw new Exception("User not found"); }

            return employee;
        }
    }
}
