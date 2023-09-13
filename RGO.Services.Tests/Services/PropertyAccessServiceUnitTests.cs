using Castle.Components.DictionaryAdapter.Xml;
using MockQueryable.Moq;
using Moq;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RGO.Services.Tests.Services
{
    public class PropertyAccessServiceUnitTests
    {
        private readonly Mock<IUnitOfWork> _dbMock;
        private readonly Mock<IEmployeeRoleService> _employeeRoleService;
        private readonly Mock<IEmployeeDataService> _employeeDataService;
        private readonly Mock<IEmployeeService> _employeeService;

        public PropertyAccessServiceUnitTests()
        {
            _dbMock = new Mock<IUnitOfWork>();
            _employeeDataService = new Mock<IEmployeeDataService>();
            _employeeRoleService = new Mock<IEmployeeRoleService>();
            _employeeService = new Mock<IEmployeeService>();
        }

        [Fact]
        public async Task GetPropertiesWithAccess()
        {
            var employee = new Employee
            {
                Id = 1,
                EmployeeNumber = "8464",
                TaxNumber = "8465468",
                EngagementDate = DateOnly.FromDateTime(DateTime.Now),
                TerminationDate = null,
                ReportingLine = null,
                Disability = false,
                DisabilityNotes = "na",
                Level = 4,
                EmployeeTypeId = 2,
                Notes = "asdsd asdsad sadsad",
                LeaveInterval = 1,
                SalaryDays = 1,
                PayRate = 1,
                Salary = 10,
                Title = "Mr",
                Initials = "ads",
                Name = "Carl",
                Surname = "Wehl",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
                CountryOfBirth = "SA",
                Nationality = "asd",
                IdNumber = "0231646",
                PassportNumber = null,
                PassportExpirationDate = null,
                PassportCountryIssue = null,
                Race = (Race)1,
                Gender = (Gender)1,
                Photo = "asfsadf/asdfsad",
                Email = "cwehl@retrorabbit.co.za",
                PersonalEmail = "asdasd@gmail.com",
                CellphoneNo = "085456565656"
            };

            var role = new Role { Id = 1, Description = "Employee" };

            var employeeRoles = new List<EmployeeRoleDto>()
            {
                new EmployeeRole{Id = 1, EmployeeId = 1, RoleId =1, Employee = employee, Role = role}.ToDto(),
                new EmployeeRole{Id = 2, EmployeeId = 1, RoleId =2, Employee = employee, Role = role}.ToDto(),
                new EmployeeRole{Id = 3, EmployeeId = 1, RoleId =3, Employee = employee, Role = role}.ToDto(),
                new EmployeeRole{Id = 4, EmployeeId = 1, RoleId =4, Employee = employee, Role = role}.ToDto(),
            };

            var access = new List<PropertyAccess>()
            {
                new PropertyAccess{Id = 2, RoleId = 1, Condition = 1, FieldCodeId= 1, Role = role, FieldCode = new FieldCode { Id = 1, Name = "Degree", Code = "degree", Status = ItemStatus.Active, Type = FieldCodeType.String } },
                new PropertyAccess{Id = 1, RoleId = 1, Condition = 0, FieldCodeId= 9, Role = role, FieldCode = new FieldCode { Id = 9, Name = "Engagement", Code = "engagement", Status = ItemStatus.Active, Type = FieldCodeType.String } },
                new PropertyAccess{Id = 3, RoleId = 1, Condition = 1, FieldCodeId= 7, Role = role, FieldCode = new FieldCode { Id = 7, Name = "Client", Code = "client", Status = ItemStatus.Active, Type = FieldCodeType.String }},
                new PropertyAccess{Id = 4, RoleId = 1, Condition = 1, FieldCodeId= 13, Role = role, FieldCode = new FieldCode { Id = 13, Name = "People's Champion", Code = "champion", Status = ItemStatus.Active, Type = FieldCodeType.String }},
                new PropertyAccess{Id = 5, RoleId = 1, Condition = 2, FieldCodeId= 15, Role = role, FieldCode = new FieldCode { Id = 15, Name = "Employee Name", Code = "name", Status = ItemStatus.Active, Type = FieldCodeType.String, Internal = true, InternalTable = "Employee" }},
                new PropertyAccess{Id = 6, RoleId = 1, Condition = 2, FieldCodeId= 16, Role = role, FieldCode = new FieldCode { Id = 16, Name = "Gender", Code = "gender", Status = ItemStatus.Active, Type = FieldCodeType.Int, Internal = true, InternalTable = "Employee" } },
            };

            var service = new PropertyAccessService(_dbMock.Object, _employeeRoleService.Object, _employeeDataService.Object, _employeeService.Object);

            _employeeService.Setup(x => x.GetEmployee(It.IsAny<string>())).Returns(Task.FromResult(employee.ToDto()));
            
            _dbMock
                .Setup(r => r.PropertyAccess.GetForEmployee(It.IsAny<string>()))
                .Returns(Task.FromResult(access.Select(x => x.ToDto()).ToList()));

            var result = await service.GetPropertiesWithAccess("");

            Assert.Equal(5, result.Count);
            Assert.True(result.Any(x => x.Id == 1), "Result.Id = 1");
            Assert.True(result.Any(x => x.Id == 7), "Result.Id = 7");
            Assert.True(result.Any(x => x.Id == 13), "Result.Id = 13");
            Assert.True(result.Any(x => x.Id == 15), "Result.Id = 15");
            Assert.True(result.Any(x => x.Id == 16), "Result.Id = 16");

        }
    }
}
