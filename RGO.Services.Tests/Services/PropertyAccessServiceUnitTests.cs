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
        private readonly EmployeeRoleService _employeeRoleService;
        private readonly EmployeeDataService _employeeDataService;

        public PropertyAccessServiceUnitTests()
        {
            _dbMock = new Mock<IUnitOfWork>();
            _employeeDataService = new EmployeeDataService(_dbMock.Object);
            _employeeRoleService = new EmployeeRoleService(_dbMock.Object);
        }

        [Fact]
        public async Task GetPropertiesWithAccess()
        {
            EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
            /*      EmployeeDto { Id = 1, EmployeeNumber = 8464, TaxNumber = 8465468, EngagementDate = 9 / 6 / 2023, TerminationDate = , ReportingLine = , Disability = False, DisabilityNotes = na, Level = 4, EmployeeType = , Notes = asdsd asdsad sadsad, LeaveInterval = 1, SalaryDays = 1, PayRate = 1, Salary = 10, Title = Mr, Name = Matteuns, Initials = ads, Surname = Wehl, DateOfBirth = 9 / 6 / 2023, CountryOfBirth = SA, Nationality = asd, IdNumber = 0231646, PassportNumber = , PassportExpirationDate = , PassportCountryIssue = , Race = White, Gender = Male, Photo = asfsadf / asdfsad, Email = cwehl@retrorabbit.co.za, PersonalEmail = asdasd@gmail.com, CellphoneNo = 0842240324 }*/
            
            var employee = new List<EmployeeDto>(){new Employee
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
            }.ToDto() }
            .Select(e => new Employee(e, e.EmployeeType))
            .ToList()
            .AsQueryable()
            .BuildMock();

            _dbMock
                .Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(employee.Where(e => e.Email == employee.First().Email));

            var employeeRoles = new List<EmployeeRoleDto>()
            {
                new EmployeeRole{Id = 1, EmployeeId = 1, RoleId =1}.ToDto(),
                new EmployeeRole{Id = 2, EmployeeId = 1, RoleId =2}.ToDto(),
                new EmployeeRole{Id = 3, EmployeeId = 1, RoleId =3}.ToDto(),
                new EmployeeRole{Id = 4, EmployeeId = 1, RoleId =4}.ToDto(),
            };

            Assert.True(true);
            /*_employeeRoleService =*/

        
            /*_dbMock
                .Setup(e => e.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
                .Returns()*/
        }
    }
}
