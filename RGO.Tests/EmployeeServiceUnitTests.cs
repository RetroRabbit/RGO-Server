using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RGO.Services;
using Xunit;
using RGO.Services.Services;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using RGO.UnitOfWork.Entities;

namespace RGO.Tests
{
    public class EmployeeServiceUnitTests
    {
        [Fact]
        public async Task SaveEmployeeTest()
        {
            var employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
            var _dbMock = new Mock<IUnitOfWork>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            EmployeeDto employeeDto = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                null, false, "None", 4, null, "Notes", 1, 28, 128, 100000, "Ms", "Dorothy", "D",
                "Mahoko", new DateTime(), "South Africa", "South African", "9708180344086", " ",
                new DateTime(), null, Models.Enums.Race.Black, Models.Enums.Gender.Female, null, 
                "dmahoko@retrorabbit.co.za", "dimphomahoko@gmail.com", "0848645127");

            EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");

            Employee employee = new Employee(employeeDto, employeeTypeDto);

            _dbMock.Setup(r => r.Employee.Add(employee)).Returns(Task.FromResult(employeeDto));

            var employeeService = new EmployeeService(employeeTypeServiceMock.Object, _dbMock.Object);

            var result = await employeeService.SaveEmployee(employeeDto);
            Assert.NotNull(result);
        }


    }
}
