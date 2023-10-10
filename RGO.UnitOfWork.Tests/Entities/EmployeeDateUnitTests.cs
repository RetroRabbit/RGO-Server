using RGO.Models;
using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities
{
    public class EmployeeDateUnitTests
    {
        private EmployeeDto _employee;

        public EmployeeDateUnitTests() 
        {
            EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");

            _employee = new EmployeeDto(1, "001", "34434434", new DateOnly(), new DateOnly(),
                null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Ms", "Dorothy", "D",
                "Mahoko", new DateOnly(), "South Africa", "South African", "0000080000000", " ",
                new DateOnly(), null, Models.Enums.Race.Black, Models.Enums.Gender.Male, null,
                "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000");
        }

        public EmployeeDate CreateEmployeeDate(EmployeeDto employee)
        {
            EmployeeDate employeeDate = new EmployeeDate
            {
                Id = 1,
                EmployeeId = 1,
                Code = "",
                Date = DateTime.Now
            };

            employeeDate.Employee = new Employee(employee, employee.EmployeeType);

            return employeeDate;
        }

        [Fact]
        public void EmployeeDateTest()
        {
            var employeeDate = new EmployeeDate();
            Assert.IsType<EmployeeDate>(employeeDate);
            Assert.NotNull(employeeDate);
        }

        [Fact]
        public void EmployeeDateToDTO()
        {
            var employeeDate = CreateEmployeeDate(employee: _employee);
            var employeeDateDto = employeeDate.ToDto();

            Assert.NotNull(employeeDateDto.Employee);
            Assert.Equal(employeeDate.EmployeeId, employeeDateDto.Employee!.Id);
            Assert.IsType<EmployeeDateDto>(employeeDateDto);

            var initializedEmployeeDate = new EmployeeDate(employeeDateDto);
            Assert.Null(initializedEmployeeDate.Employee);
        }
    }
}
