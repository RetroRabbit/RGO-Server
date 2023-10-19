using MockQueryable.Moq;
using Moq;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Models.Update;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Data;
using System.Linq.Expressions;
using Xunit;

namespace RGO.Tests.Services
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
                PeopleChampion = null,
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

            _employeeService.Setup(x => x.GetEmployee(It.IsAny<string>()))
                .Returns(Task.FromResult(employee.ToDto()));

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

        [Fact]
        public async Task UpdatesInternalField()
        {
            var employee = new Employee
            {
                Id = 1,
                EmployeeNumber = "8464",
                TaxNumber = "8465468",
                EngagementDate = DateOnly.FromDateTime(DateTime.Now),
                TerminationDate = null,
                PeopleChampion = null,
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

            var role = new List<Role>()
            {
                new Role { Id = 1, Description = "SuperAdmin" },
                new Role { Id = 2, Description = "Admin" },
                new Role { Id = 3, Description = "Employee" },
                new Role { Id = 4, Description = "Talent" }
            };

            var employeeRoles = new List<EmployeeRoleDto>()
            {
                new EmployeeRole{Id = 1, Employee = employee, Role = role[0]}.ToDto(),
                new EmployeeRole{Id = 2, Employee = employee, Role = role[1]}.ToDto(),
                new EmployeeRole{Id = 3, Employee = employee, Role = role[2]}.ToDto(),
                new EmployeeRole{Id = 4, Employee = employee, Role = role[3]}.ToDto(),
            };

            var accessList = new List<PropertyAccess>
            {
                new PropertyAccess{Id = 4, RoleId = 1, Condition = 2, FieldCodeId= 9},
                new PropertyAccess{Id = 4, RoleId = 1, Condition = 2, FieldCodeId= 10},
                new PropertyAccess{Id = 4, RoleId = 1, Condition = 2, FieldCodeId= 11},
                new PropertyAccess{Id = 4, RoleId = 1, Condition = 2, FieldCodeId= 12},
                new PropertyAccess{Id = 4, RoleId = 1, Condition = 2, FieldCodeId= 13},
                new PropertyAccess{Id = 5, RoleId = 1, Condition = 2, FieldCodeId= 15},
                new PropertyAccess{Id = 6, RoleId = 1, Condition = 2, FieldCodeId= 16},
            };

            var fieldCode = new List<FieldCode>()
            {
               new FieldCode { Id = 9, Name = "Bank Card Number", Code = "cardNo", Status = ItemStatus.Active, Type = FieldCodeType.String, Internal= true, InternalTable="BankDetails" },
               new FieldCode { Id = 10, Name = "Rating", Code = "rating", Status = ItemStatus.Active, Type = FieldCodeType.Float, Internal= true, InternalTable="Employee" },
               new FieldCode { Id = 11, Name = "Birth Date", Code = "dob", Status = ItemStatus.Active, Type = FieldCodeType.Date, Internal= true, InternalTable="Employee" },
               new FieldCode { Id = 12, Name = "Car Registration", Code = "car", Status = ItemStatus.Active, Type = FieldCodeType.String },
               new FieldCode { Id = 13, Name = "People's Champion", Code = "champion", Status = ItemStatus.Active, Type = FieldCodeType.String },
               new FieldCode { Id = 15, Name = "Employee Name", Code = "name", Status = ItemStatus.Active, Type = FieldCodeType.String, Internal = true, InternalTable = "Employee" },
               new FieldCode { Id = 16, Name = "Gender", Code = "gender", Status = ItemStatus.Active, Type = FieldCodeType.Int, Internal = true, InternalTable = "Employee" },
            };

            var employeeData = new List<EmployeeData>()
            {
                new EmployeeData {Id =1 , EmployeeId = 1, FieldCodeId = 13, Value = "Mandy" }
            };

            var service = new PropertyAccessService(_dbMock.Object, _employeeRoleService.Object, _employeeDataService.Object, _employeeService.Object);

            _employeeService.Setup(x => x.GetEmployee(It.IsAny<string>())).Returns(Task.FromResult(employee.ToDto()));

            var employeeList = new List<Employee> { employee }.AsQueryable().BuildMock();

            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(employeeList);

            var employeeRoleService = new Mock<EmployeeRoleService>();

            _employeeRoleService.Setup(er => er.GetEmployeeRoles(It.IsAny<string>()))
                .Returns(Task.FromResult(employeeRoles));

            var propertyAccess = new Mock<PropertyAccessService>();

            _dbMock.Setup(pa => pa.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
                .Returns(accessList.AsQueryable().BuildMock());

            var employeeDataService = new Mock<EmployeeData>();

            _dbMock.SetupSequence(ed => ed.EmployeeData.Get(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
                .Returns(new List<EmployeeData>() { }.AsQueryable().BuildMock())
                .Returns(employeeData.AsQueryable().BuildMock());

            var fieldCodeService = new Mock<FieldCodeService>();

            _dbMock.SetupSequence(f => f.FieldCode.GetById(It.IsAny<int>()))
                .Returns(Task.FromResult(fieldCode[0].ToDto()))
                .Returns(Task.FromResult(fieldCode[1].ToDto()))
                .Returns(Task.FromResult(fieldCode[2].ToDto()))
                .Returns(Task.FromResult(fieldCode[3].ToDto()))
                .Returns(Task.FromResult(fieldCode[4].ToDto()))
                .Returns(Task.FromResult(fieldCode[5].ToDto()))
                .Returns(Task.FromResult(fieldCode[6].ToDto()));


            _dbMock
                .Setup(r => r.RawSql(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            await service.UpdatePropertiesWithAccess(new List<UpdateFieldValueDto>()
            {
                new UpdateFieldValueDto(12, "0846 5132 5863 1588"),
                new UpdateFieldValueDto(10, 0.25),
                new UpdateFieldValueDto(11, DateOnly.FromDateTime(DateTime.Now)),
                new UpdateFieldValueDto(12, "zxy 098 gp"),
                new UpdateFieldValueDto(13, "James"),
                new UpdateFieldValueDto(15, "Pieter"),
                new UpdateFieldValueDto(16, 2),
            }, "cwehl@retrorabbit.co.za");
        }


        [Fact]
        public async Task UpdatesInternalFieldNoEmmployee()
        {
            var employee = new Employee
            {
            };

            var role = new List<Role>()
            {
                new Role { Id = 1, Description = "SuperAdmin" },
                new Role { Id = 2, Description = "Admin" },
                new Role { Id = 3, Description = "Employee" },
                new Role { Id = 4, Description = "Talent" }
            };

            var employeeRoles = new List<EmployeeRoleDto>()
            {
                new EmployeeRole{Id = 1, EmployeeId = 1, Role = role[0]}.ToDto(),
                new EmployeeRole{Id = 2, EmployeeId = 1, Role = role[1]}.ToDto(),
                new EmployeeRole{Id = 3, EmployeeId = 1, Role = role[2]}.ToDto(),
                new EmployeeRole{Id = 4, EmployeeId = 1, Role = role[3]}.ToDto(),
            };

            var accessList = new List<PropertyAccess>
            {
                new PropertyAccess{Id = 4, RoleId = 1, Condition = 2, FieldCodeId= 12},
                new PropertyAccess{Id = 4, RoleId = 1, Condition = 2, FieldCodeId= 13},
                new PropertyAccess{Id = 5, RoleId = 1, Condition = 2, FieldCodeId= 15},
                new PropertyAccess{Id = 6, RoleId = 1, Condition = 2, FieldCodeId= 16},
            };

            var fieldCode = new List<FieldCode>()
            {
               new FieldCode { Id = 12, Name = "Car Registration", Code = "car", Status = ItemStatus.Active, Type = FieldCodeType.String },
               new FieldCode { Id = 13, Name = "People's Champion", Code = "champion", Status = ItemStatus.Active, Type = FieldCodeType.String },
               new FieldCode { Id = 15, Name = "Employee Name", Code = "name", Status = ItemStatus.Active, Type = FieldCodeType.String, Internal = true, InternalTable = "Employee" },
               new FieldCode { Id = 16, Name = "Gender", Code = "gender", Status = ItemStatus.Active, Type = FieldCodeType.Int, Internal = true, InternalTable = "Employee" },
            };

            var employeeData = new List<EmployeeData>()
            {
                new EmployeeData {Id =1 , EmployeeId = 1, FieldCodeId = 13, Value = "Mandy" }
            };

            var service = new PropertyAccessService(_dbMock.Object, _employeeRoleService.Object, _employeeDataService.Object, _employeeService.Object);

            _employeeService.Setup(x => x.GetEmployee(It.IsAny<string>())).Returns(Task.FromResult(employee.ToDto()));

            var employeeList = new List<Employee> { }.AsQueryable().BuildMock();

            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(employeeList);

            var employeeRoleService = new Mock<EmployeeRoleService>();

            _employeeRoleService.Setup(er => er.GetEmployeeRoles(It.IsAny<string>()))
                .Returns(Task.FromResult(employeeRoles));

            var propertyAccess = new Mock<PropertyAccessService>();

            _dbMock.Setup(pa => pa.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
                .Returns(accessList.AsQueryable().BuildMock());

            var employeeDataService = new Mock<EmployeeData>();

            _dbMock.SetupSequence(ed => ed.EmployeeData.Get(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
                .Returns(new List<EmployeeData>() { }.AsQueryable().BuildMock())
                .Returns(employeeData.AsQueryable().BuildMock());

            var fieldCodeService = new Mock<FieldCodeService>();

            _dbMock.SetupSequence(f => f.FieldCode.GetById(It.IsAny<int>()))
                .Returns(Task.FromResult(fieldCode[0].ToDto()))
                .Returns(Task.FromResult(fieldCode[1].ToDto()))
                .Returns(Task.FromResult(fieldCode[2].ToDto()))
                .Returns(Task.FromResult(fieldCode[3].ToDto()));


            _dbMock
                .Setup(r => r.RawSql(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            Assert.ThrowsAsync<Exception>(() => service.UpdatePropertiesWithAccess(new List<UpdateFieldValueDto>()
            {
                new UpdateFieldValueDto(12, "zxy 035 gp"),
                new UpdateFieldValueDto(13, "James"),
                new UpdateFieldValueDto(15, "Pieter"),
                new UpdateFieldValueDto(16, 2),
            }, "cwehl@retrorabbit.co.za"));
        }

        [Fact]
        public async Task UpdatesInternalFieldNoEdit()
        {
            var employee = new Employee
            {
                Id = 1,
                EmployeeNumber = "8464",
                TaxNumber = "8465468",
                EngagementDate = DateOnly.FromDateTime(DateTime.Now),
                TerminationDate = null,
                PeopleChampion = null,
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

            var role = new List<Role>()
            {
                new Role { Id = 1, Description = "SuperAdmin" },
                new Role { Id = 2, Description = "Admin" },
                new Role { Id = 3, Description = "Employee" },
                new Role { Id = 4, Description = "Talent" }
            };

            var employeeRoles = new List<EmployeeRoleDto>()
            {

            };

            var accessList = new List<PropertyAccess>
            {
                new PropertyAccess{Id = 4, RoleId = 1, Condition = 1, FieldCodeId= 12},
                new PropertyAccess{Id = 4, RoleId = 1, Condition = 1, FieldCodeId= 13},
                new PropertyAccess{Id = 5, RoleId = 1, Condition = 1, FieldCodeId= 15},
                new PropertyAccess{Id = 6, RoleId = 1, Condition = 1, FieldCodeId= 16},
            };

            var fieldCode = new List<FieldCode>()
            {
               new FieldCode { Id = 12, Name = "Car Registration", Code = "car", Status = ItemStatus.Active, Type = FieldCodeType.String },
               new FieldCode { Id = 13, Name = "People's Champion", Code = "champion", Status = ItemStatus.Active, Type = FieldCodeType.String },
               new FieldCode { Id = 15, Name = "Employee Name", Code = "name", Status = ItemStatus.Active, Type = FieldCodeType.String, Internal = true, InternalTable = "Employee" },
               new FieldCode { Id = 16, Name = "Gender", Code = "gender", Status = ItemStatus.Active, Type = FieldCodeType.Int, Internal = true, InternalTable = "Employee" },
            };

            var employeeData = new List<EmployeeData>()
            {
                new EmployeeData {Id =1 , EmployeeId = 1, FieldCodeId = 13, Value = "Mandy" }
            };

            var service = new PropertyAccessService(_dbMock.Object, _employeeRoleService.Object, _employeeDataService.Object, _employeeService.Object);

            _employeeService.Setup(x => x.GetEmployee(It.IsAny<string>())).Returns(Task.FromResult(employee.ToDto()));

            var employeeList = new List<Employee> { employee }.AsQueryable().BuildMock();

            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(employeeList);

            var employeeRoleService = new Mock<EmployeeRoleService>();

            _employeeRoleService.Setup(er => er.GetEmployeeRoles(It.IsAny<string>()))
                .Returns(Task.FromResult(employeeRoles));

            var propertyAccess = new Mock<PropertyAccessService>();

            _dbMock.Setup(pa => pa.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
                .Returns(accessList.AsQueryable().BuildMock());

            var employeeDataService = new Mock<EmployeeData>();

            _dbMock.SetupSequence(ed => ed.EmployeeData.Get(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
                .Returns(new List<EmployeeData>() { }.AsQueryable().BuildMock())
                .Returns(employeeData.AsQueryable().BuildMock());

            var fieldCodeService = new Mock<FieldCodeService>();

            _dbMock.SetupSequence(f => f.FieldCode.GetById(It.IsAny<int>()))
                .Returns(Task.FromResult(fieldCode[0].ToDto()))
                .Returns(Task.FromResult(fieldCode[1].ToDto()))
                .Returns(Task.FromResult(fieldCode[2].ToDto()))
                .Returns(Task.FromResult(fieldCode[3].ToDto()));


            _dbMock
                .Setup(r => r.RawSql(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            Assert.ThrowsAsync<Exception>(() => service.UpdatePropertiesWithAccess(new List<UpdateFieldValueDto>()
            {
                new UpdateFieldValueDto(12, "zxy 035 gp"),
                new UpdateFieldValueDto(13, "James"),
                new UpdateFieldValueDto(15, "Pieter"),
                new UpdateFieldValueDto(16, 2),
            }, "cwehl@retrorabbit.co.za"));
        }

        [Fact]
        public async Task UpdatesInternalFieldWithInvalidTypeError()
        {
            var employee = new Employee
            {
                Id = 1,
                EmployeeNumber = "8464",
                TaxNumber = "8465468",
                EngagementDate = DateOnly.FromDateTime(DateTime.Now),
                TerminationDate = null,
                PeopleChampion = null,
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

            var role = new List<Role>()
            {
                new Role { Id = 1, Description = "SuperAdmin" },
                new Role { Id = 2, Description = "Admin" },
                new Role { Id = 3, Description = "Employee" },
                new Role { Id = 4, Description = "Talent" }
            };

            var employeeRoles = new List<EmployeeRoleDto>()
            {
                new EmployeeRole{Id = 1, Employee = employee, Role = role[0]}.ToDto(),
                new EmployeeRole{Id = 2, Employee = employee, Role = role[1]}.ToDto(),
                new EmployeeRole{Id = 3, Employee = employee, Role = role[2]}.ToDto(),
                new EmployeeRole{Id = 4, Employee = employee, Role = role[3]}.ToDto(),
            };

            var accessList = new List<PropertyAccess>
            {
                new PropertyAccess{Id = 4, RoleId = 1, Condition = 2, FieldCodeId= 10},
                new PropertyAccess{Id = 4, RoleId = 1, Condition = 2, FieldCodeId= 11},
                new PropertyAccess{Id = 4, RoleId = 1, Condition = 2, FieldCodeId= 12},
                new PropertyAccess{Id = 4, RoleId = 1, Condition = 2, FieldCodeId= 13},
                new PropertyAccess{Id = 5, RoleId = 1, Condition = 2, FieldCodeId= 15},
                new PropertyAccess{Id = 6, RoleId = 1, Condition = 2, FieldCodeId= 16},
            };

            var fieldCode = new List<FieldCode>()
            {
               new FieldCode { Id = 10, Name = "Rating", Code = "rating", Status = ItemStatus.Active, Type = (FieldCodeType)8, Internal= true, InternalTable="Employee" },
               new FieldCode { Id = 11, Name = "Birth Date", Code = "dob", Status = ItemStatus.Active, Type = FieldCodeType.Date, Internal= true, InternalTable="Employee" },
               new FieldCode { Id = 12, Name = "Car Registration", Code = "car", Status = ItemStatus.Active, Type = FieldCodeType.String },
               new FieldCode { Id = 13, Name = "People's Champion", Code = "champion", Status = ItemStatus.Active, Type = FieldCodeType.String },
               new FieldCode { Id = 15, Name = "Employee Name", Code = "name", Status = ItemStatus.Active, Type = FieldCodeType.String, Internal = true, InternalTable = "Employee" },
               new FieldCode { Id = 16, Name = "Gender", Code = "gender", Status = ItemStatus.Active, Type = FieldCodeType.Int, Internal = true, InternalTable = "Employee" },
            };

            var employeeData = new List<EmployeeData>()
            {
                new EmployeeData {Id =1 , EmployeeId = 1, FieldCodeId = 13, Value = "Mandy" }
            };

            var service = new PropertyAccessService(_dbMock.Object, _employeeRoleService.Object, _employeeDataService.Object, _employeeService.Object);

            _employeeService.Setup(x => x.GetEmployee(It.IsAny<string>())).Returns(Task.FromResult(employee.ToDto()));

            var employeeList = new List<Employee> { employee }.AsQueryable().BuildMock();

            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(employeeList);

            var employeeRoleService = new Mock<EmployeeRoleService>();

            _employeeRoleService.Setup(er => er.GetEmployeeRoles(It.IsAny<string>()))
                .Returns(Task.FromResult(employeeRoles));

            var propertyAccess = new Mock<PropertyAccessService>();

            _dbMock.Setup(pa => pa.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
                .Returns(accessList.AsQueryable().BuildMock());

            var employeeDataService = new Mock<EmployeeData>();

            _dbMock.SetupSequence(ed => ed.EmployeeData.Get(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
                .Returns(new List<EmployeeData>() { }.AsQueryable().BuildMock())
                .Returns(employeeData.AsQueryable().BuildMock());

            var fieldCodeService = new Mock<FieldCodeService>();

            _dbMock.SetupSequence(f => f.FieldCode.GetById(It.IsAny<int>()))
                .Returns(Task.FromResult(fieldCode[0].ToDto()))
                .Returns(Task.FromResult(fieldCode[1].ToDto()))
                .Returns(Task.FromResult(fieldCode[2].ToDto()))
                .Returns(Task.FromResult(fieldCode[3].ToDto()))
                .Returns(Task.FromResult(fieldCode[4].ToDto()))
                .Returns(Task.FromResult(fieldCode[5].ToDto()));


            _dbMock
                .Setup(r => r.RawSql(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            Assert.ThrowsAsync<Exception>(() => service.UpdatePropertiesWithAccess(new List<UpdateFieldValueDto>()
            {
                new UpdateFieldValueDto(12, "zxy 035 gp"),
                new UpdateFieldValueDto(13, "James"),
                new UpdateFieldValueDto(15, "Pieter"),
                new UpdateFieldValueDto(16, 2),
            }, "cwehl@retrorabbit.co.za"));
        }
    }
}
