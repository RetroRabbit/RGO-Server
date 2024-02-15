using MockQueryable.Moq;
using Moq;
using Newtonsoft.Json;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.Tests.Data.Models;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using Xunit;
using Xunit.Sdk;

namespace RGO.Tests.Services
{
    public class EmployeeServiceUnitTests
    {
        private readonly Mock<IUnitOfWork> _dbMock;
        private readonly Mock<IEmployeeTypeService> employeeTypeServiceMock;
        private readonly Mock<IEmployeeAddressService> employeeAddressServiceMock;
        private readonly Mock<IRoleService> roleServiceMock;
        private EmployeeService employeeService;

        static private EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        static private EmployeeTypeDto employeeTypeDto2 = new EmployeeTypeDto(2, "Designer");
        static EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");
        static private RoleDto roleDto = new RoleDto(3, "Employee");
        static private RoleDto roleDto2 = new RoleDto(1, "Admin");

        EmployeeRoleDto employeeRoleDto = new EmployeeRoleDto(0, EmployeeTestData.EmployeeDto, roleDto);
        static EmployeeDto employeeDto = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty", "D",
            "Missile", new DateTime(), "South Africa", "South African", "5522522655", " ",
            new DateTime(), null, Models.Enums.Race.Black, Models.Enums.Gender.Female, null,
            "dm@retrorabbit.co.za", "test@gmail.com", "0123456789", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        public EmployeeServiceUnitTests()
        {
            _dbMock = new Mock<IUnitOfWork>();
            employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
            employeeAddressServiceMock = new Mock<IEmployeeAddressService>();
            roleServiceMock = new Mock<IRoleService>();
            employeeService = new EmployeeService(employeeTypeServiceMock.Object, _dbMock.Object, employeeAddressServiceMock.Object, roleServiceMock.Object);
        }

        [Fact]
        public async Task SaveEmployeeFailTest1()
        {

            _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(Task.FromResult(true));

            await Assert.ThrowsAsync<Exception>(() => employeeService.SaveEmployee(EmployeeTestData.EmployeeDto));
        }

        [Fact]
        public async Task SaveEmployeeFailTest2()
        {

            _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(Task.FromResult(false));

            employeeTypeServiceMock.Setup(r => r.GetEmployeeType(It.IsAny<string>())).Throws(new Exception());

            employeeTypeServiceMock.Setup(r => r.SaveEmployeeType(It.IsAny<EmployeeTypeDto>())).ReturnsAsync(employeeTypeDto);

            employeeAddressServiceMock.SetupSequence(r => r.CheckIfExists(It.IsAny<EmployeeAddressDto>()))
                .ReturnsAsync(false)
                .ReturnsAsync(false);

            employeeAddressServiceMock.SetupSequence(r => r.Save(It.IsAny<EmployeeAddressDto>()))
                .ReturnsAsync(employeeAddressDto)
                .ReturnsAsync(employeeAddressDto);

            _dbMock.Setup(r => r.Employee.Add(It.IsAny<Employee>())).Returns(Task.FromResult(EmployeeTestData.EmployeeDto));
            _dbMock.Setup(r => r.EmployeeRole.Add(It.IsAny<EmployeeRole>())).Returns(Task.FromResult(employeeRoleDto));

            roleServiceMock.Setup(r => r.GetRole("Employee")).Returns(Task.FromResult(roleDto));
            var result = await employeeService.SaveEmployee(EmployeeTestData.EmployeeDto);

            Assert.Equal(EmployeeTestData.EmployeeDto, result);
        }

        [Fact]
        public async Task SaveEmployeeFailTest3()
        {

            _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(Task.FromResult(false));

            employeeTypeServiceMock.Setup(r => r.GetEmployeeType(It.IsAny<string>())).Throws(new Exception());

            employeeTypeServiceMock.Setup(r => r.SaveEmployeeType(It.IsAny<EmployeeTypeDto>())).Returns(Task.FromResult(employeeTypeDto));

            employeeAddressServiceMock.SetupSequence(r => r.CheckIfExists(employeeAddressDto))
                .ReturnsAsync(true)
                .ReturnsAsync(true);

            employeeAddressServiceMock.Setup(x => x.Get(It.IsAny<EmployeeAddressDto>()))
                .ReturnsAsync(employeeAddressDto);

            employeeAddressServiceMock.SetupSequence(r => r.Save(employeeAddressDto))
                .ReturnsAsync(employeeAddressDto)
                .ReturnsAsync(employeeAddressDto);

            _dbMock.Setup(r => r.Employee.Add(It.IsAny<Employee>())).Returns(Task.FromResult(employeeDto));
            _dbMock.Setup(r => r.EmployeeRole.Add(It.IsAny<EmployeeRole>())).Returns(Task.FromResult(employeeRoleDto));

            roleServiceMock.Setup(r => r.GetRole("Employee")).Returns(Task.FromResult(roleDto));

            var result = await employeeService.SaveEmployee(employeeDto);

            Assert.Equal(employeeDto, result);
        }

        [Fact]
        public async Task SaveEmployeeTest()
        {

            employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name)).Returns(Task.FromResult(employeeTypeDto));
            EmployeeRoleDto employeeRoleDto = new EmployeeRoleDto(0, EmployeeTestData.EmployeeDto, roleDto);

            employeeAddressServiceMock.SetupSequence(r => r.CheckIfExists(It.IsAny<EmployeeAddressDto>()))
                .ReturnsAsync(false)
                .ReturnsAsync(true);

            employeeAddressServiceMock.Setup(r => r.Save(It.IsAny<EmployeeAddressDto>()))
                .ReturnsAsync(employeeAddressDto);

            employeeAddressServiceMock.Setup(r => r.Get(It.IsAny<EmployeeAddressDto>()))
                .ReturnsAsync(employeeAddressDto);

            roleServiceMock.Setup(r => r.GetRole("Employee")).Returns(Task.FromResult(roleDto));

            _dbMock.Setup(r => r.Employee.Add(It.IsAny<Employee>())).Returns(Task.FromResult(EmployeeTestData.EmployeeDto));
            _dbMock.Setup(r => r.EmployeeRole.Add(It.IsAny<EmployeeRole>())).Returns(Task.FromResult(employeeRoleDto));

            var result = await employeeService.SaveEmployee(EmployeeTestData.EmployeeDto);
            Assert.NotNull(result);
            Assert.Equal(EmployeeTestData.EmployeeDto, result);
        }

        //There is not PushToPreducerTestPass because we cannot mock the connection to RabbitMQ Docker Instance

        [Fact]
        public void PushToProducerTestFail()
        {
            var employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var _dbMock = new Mock<IUnitOfWork>();

            employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name)).Returns(Task.FromResult(employeeTypeDto));

            var employeeData = new Employee(EmployeeTestData.EmployeeDto, employeeTypeDto);

            employeeService.PushToProducer(employeeData);
        }


        [Fact]
        public async Task GetEmployeeTest()
        {
            var employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
            var _dbMock = new Mock<IUnitOfWork>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var employeeList = new List<Employee>
            {
               new Employee(EmployeeTestData.EmployeeDto, employeeTypeDto)
            };

            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                 .Returns(employeeList.AsQueryable().BuildMock());


            var result = employeeService.GetEmployee("dm@retrorabbit.co.za");

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteEmployeeTest()
        {

            employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name)).Returns(Task.FromResult(employeeTypeDto));

            var employeeList = new List<Employee>
            {
               new Employee(EmployeeTestData.EmployeeDto, employeeTypeDto)
            };

            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                 .Returns(employeeList.AsQueryable().BuildMock());

            _dbMock.Setup(r => r.Employee.Delete(EmployeeTestData.EmployeeDto.Id)).Returns(Task.FromResult(EmployeeTestData.EmployeeDto));

            var result = await employeeService.DeleteEmployee(EmployeeTestData.EmployeeDto.Email);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllTest()
        {
            EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
            EmployeeType employeeType = new EmployeeType(employeeTypeDto);

            employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name)).Returns(Task.FromResult(employeeTypeDto));
            EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

            var employees = new List<Employee>
            {
                new Employee(EmployeeTestData.EmployeeDto, employeeTypeDto),
                new Employee(EmployeeTestData.EmployeeDto2, employeeTypeDto),
                new Employee(EmployeeTestData.EmployeeDto3, employeeTypeDto)
            };

            _dbMock.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(employees.AsQueryable().BuildMock());

            var result = await employeeService.GetAll();

            Assert.NotNull(result);
            Assert.IsType<List<EmployeeDto>>(result);
            Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
            _dbMock.Verify(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAllIsJourney()
        {
            Employee emp = new Employee(EmployeeTestData.EmployeeDto2, employeeTypeDto2);
            emp.EmployeeType = new EmployeeType(employeeTypeDto2);

            RoleDto roleDto = new RoleDto(2, "Journey");
            EmployeeRoleDto empRoleDto = new EmployeeRoleDto(1, EmployeeTestData.EmployeeDto, roleDto);
            EmployeeRole empRole = new EmployeeRole(empRoleDto);

            List<Employee> employees = new List<Employee> { emp };
            List<EmployeeRole> empRoles = new List<EmployeeRole> { empRole };
            List<Role> roles = new List<Role> { new Role(roleDto) };

            var mockEmployees = employees;
            var expectedEmployees = new List<Employee> { new Employee(EmployeeTestData.EmployeeDto3, employeeTypeDto) };
            _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                          .Returns(mockEmployees.AsQueryable().BuildMock());

            _dbMock.Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
                           .Returns(empRoles.AsQueryable().BuildMock());

            _dbMock.Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
                           .Returns(roles.AsQueryable().BuildMock());

            _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                          .Returns(expectedEmployees.AsQueryable().BuildMock());

            var journeyEmployees = await employeeService.GetAll(EmployeeTestData.EmployeeDto2.Email);

            Assert.Equal(journeyEmployees.Count, 1);
        }

        [Fact]
        public async Task CheckEmployeeExistsTest()
        {
            var employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
            var _dbMock = new Mock<IUnitOfWork>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(Task.FromResult(true));

            var result = employeeService.CheckUserExist("dm@retrorabbit.co.za");

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateEmployeeTestOwnProfile()
        {
            employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name)).Returns(Task.FromResult(employeeTypeDto));

            _dbMock.Setup(r => r.Employee.Update(It.IsAny<Employee>())).Returns(Task.FromResult(EmployeeTestData.EmployeeDto));

            var result = await employeeService.UpdateEmployee(EmployeeTestData.EmployeeDto, EmployeeTestData.EmployeeDto.Email);

            Assert.Equal(EmployeeTestData.EmployeeDto, result);
        } 

        [Fact]
        public async Task UpdateEmployeeTestAdminPass()
        {
            Employee emp = new Employee(EmployeeTestData.EmployeeDto2, employeeTypeDto);
            emp.EmployeeType = new EmployeeType(employeeTypeDto2);
            
            RoleDto roleDto = new RoleDto(2, "Admin");
            EmployeeRoleDto empRoleDto = new EmployeeRoleDto(1, EmployeeTestData.EmployeeDto2, roleDto);
            EmployeeRole empRole = new EmployeeRole(empRoleDto);

            List<Employee> employees = new List<Employee> { emp };
            List<EmployeeRole> empRoles = new List<EmployeeRole> { empRole };
            List<Role> roles = new List<Role> { new Role(roleDto) };

            employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name)).Returns(Task.FromResult(employeeTypeDto));
            
            _dbMock.Setup(r => r.Employee.Update(It.IsAny<Employee>())).Returns(Task.FromResult(EmployeeTestData.EmployeeDto));
            _dbMock.Setup(r => r.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(employees.AsQueryable().BuildMock());
            _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
            _dbMock.Setup(r => r.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>())).Returns(empRoles.AsQueryable().BuildMock());
            _dbMock.Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>())).Returns(roles.AsQueryable().BuildMock());
            
            var result = await employeeService.UpdateEmployee(EmployeeTestData.EmployeeDto, "admin@retrorabbit.co.za");
            
            Assert.NotNull(result);
            Assert.Equal(EmployeeTestData.EmployeeDto, result);
        }

        [Fact]
        public async Task UpdateEmployeeTestAdminFail()
        {
            Employee emp = new Employee(EmployeeTestData.EmployeeDto2, employeeTypeDto);
            emp.EmployeeType = new EmployeeType(employeeTypeDto2);
            List<Employee> employees = new List<Employee> { emp };
            
            RoleDto roleDto = new RoleDto(2, "Employee");
            EmployeeRoleDto empRoleDto = new EmployeeRoleDto(1, EmployeeTestData.EmployeeDto2, roleDto);
            EmployeeRole empRole = new EmployeeRole(empRoleDto);
            
            List<EmployeeRole> empRoles = new List<EmployeeRole> { empRole };
            List<Role> roles = new List<Role> { new Role(roleDto) };

            employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name)).Returns(Task.FromResult(employeeTypeDto));
            
            _dbMock.Setup(r => r.Employee.Update(It.IsAny<Employee>())).Returns(Task.FromResult(EmployeeTestData.EmployeeDto));
            _dbMock.Setup(r => r.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(employees.AsQueryable().BuildMock());
            _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
            _dbMock.Setup(r => r.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>())).Returns(empRoles.AsQueryable().BuildMock());
            _dbMock.Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>())).Returns(roles.AsQueryable().BuildMock());

            var exception = await Assert.ThrowsAsync<Exception>(
            async () => await employeeService.UpdateEmployee(EmployeeTestData.EmployeeDto, "unauthorized.email@retrorabbit.co.za"));

            Assert.Equal("Unauthorized action", exception.Message);
        }

        [Fact]
        public async Task UpdateEmployeeTestUserDoesNotExist()
        {
            Employee emp = new Employee(EmployeeTestData.EmployeeDto2, employeeTypeDto);
            emp.EmployeeType = new EmployeeType(employeeTypeDto2);
            
            List<Employee> employees = new List<Employee> { emp };
            RoleDto roleDto = new RoleDto(2, "Employee");
            EmployeeRoleDto empRoleDto = new EmployeeRoleDto(1, EmployeeTestData.EmployeeDto2, roleDto);
            
            EmployeeRole empRole = new EmployeeRole(empRoleDto);
            List<EmployeeRole> empRoles = new List<EmployeeRole> { empRole };
            List<Role> roles = new List<Role> { new Role(roleDto) };

            employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name)).Returns(Task.FromResult(employeeTypeDto));
            _dbMock.Setup(r => r.Employee.Update(It.IsAny<Employee>())).Returns(Task.FromResult(EmployeeTestData.EmployeeDto));
            _dbMock.Setup(r => r.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(employees.AsQueryable().BuildMock());
            _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<Exception>(
            async () => await employeeService.UpdateEmployee(EmployeeTestData.EmployeeDto, "unauthorized.email@retrorabbit.co.za"));

            Assert.Equal("Unauthorized action", exception.Message);
        }

        [Fact]
        public async Task GetByIdPass()
        {
            _dbMock.Setup(x => x.Employee.GetById(EmployeeTestData.EmployeeDto.Id))
                .ReturnsAsync(EmployeeTestData.EmployeeDto);
            
            var result = await employeeService.GetById(EmployeeTestData.EmployeeDto.Id);
            
            Assert.NotNull(result);
            Assert.Equal(EmployeeTestData.EmployeeDto, result);
        }

        [Fact]
        public async Task GetEmployeeByType()
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee(EmployeeTestData.EmployeeDto, employeeTypeDto)
            };

            List<EmployeeDto> expectedList = new List<EmployeeDto> { EmployeeTestData.EmployeeDto };
            
            _dbMock.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(employees.AsQueryable().BuildMock());
            
            var result = await employeeService.GetEmployeesByType(employeeTypeDto.Name);
            
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public async Task GetSimpleProfileWithPCAndTeamLeadAndClient()
        {
            ClientDto allocatedClient = new ClientDto(1, "FNB");
            List<Client> clients = new List<Client>() { new Client(allocatedClient) };

            List<Employee> employeeList = new List<Employee> { new Employee(EmployeeTestData.EmployeeDto4, employeeTypeDto) };
            employeeList.First().EmployeeType = new EmployeeType(employeeTypeDto);

            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(employeeList.AsQueryable().BuildMock());

            _dbMock.Setup(e => e.Employee.GetById(It.IsAny<int>()))
                .ReturnsAsync((int id) => id == EmployeeTestData.EmployeeDto4.TeamLead ? EmployeeTestData.EmployeeDto3 : EmployeeTestData.EmployeeDto2);

            _dbMock.Setup(db => db.Client.Get(It.IsAny<Expression<Func<Client, bool>>>()))
                .Returns(clients.AsQueryable().BuildMock());

            var result = await employeeService.GetSimpleProfile(EmployeeTestData.EmployeeDto4.Email);
            
            Assert.NotNull(result);
            Assert.Equal(EmployeeTestData.EmployeeDto4.TeamLead, result.TeamLeadId);
            Assert.Equal(EmployeeTestData.EmployeeDto4.PeopleChampion, result.PeopleChampionId);
            Assert.Equal(allocatedClient.Name, result.ClientAllocatedName);
        }

        [Fact]
        public async Task GetEmployeeByIdPass()
        {
            List<Employee> employees = new List<Employee>() { new Employee(EmployeeTestData.EmployeeDto, employeeTypeDto) };
            
            employees.First().EmployeeType = new EmployeeType(employeeTypeDto);
            employees.First().PhysicalAddress = new EmployeeAddress(employeeAddressDto);
            employees.First().PostalAddress = new EmployeeAddress(employeeAddressDto);

            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(employees.AsQueryable().BuildMock());

            var result = await employeeService.GetEmployeeById(EmployeeTestData.EmployeeDto.Id);
            
            Assert.NotNull(result);
            Assert.Equivalent(EmployeeTestData.EmployeeDto, result);
        }

        [Fact]
        public async Task GetEmployeeByIdFail()
        {
            List<Employee> mockEmployees = new List<Employee>();

            _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(mockEmployees.AsQueryable().BuildMock());

            await Assert.ThrowsAsync<Exception>(() => employeeService.GetEmployeeById(2));
        }

        [Fact]
        public async Task GetCurrentMonthTotalReturnsExistingTotalTest()
        {
            EmployeeTypeDto employeeTypeDto = new(2, "Developer");
            EmployeeType employeeType = new(employeeTypeDto);
            EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

            EmployeeDto employeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
               null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty", "D",
               "Missile", new DateTime(), "South Africa", "South African", "0000000000000", " ",
               new DateTime(), null, Models.Enums.Race.Black, Models.Enums.Gender.Female, null,
               "dm@.co.za", "test@gmail.com", "0123456789", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

            var employeeList = new List<EmployeeDto>
            {
                employeeDto
            };

            var employee = new List<Employee>
            {
                new Employee(employeeDto,employeeTypeDto)
            };

            _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                  .Returns(employee.AsQueryable().BuildMock());

            var currentMonth = DateTime.Now.ToString("MMMM");

            var currentYear = DateTime.Now.Year;

            MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = new(1, 1, 1, 0, 0, 0, currentMonth, currentYear);

            var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
            {
              new MonthlyEmployeeTotal (monthlyEmployeeTotalDto),
            };

            _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
                   .Returns(montlhyEmployeeTotalList.AsQueryable().BuildMock());

            var result = await employeeService.GetEmployeeCurrentMonthTotal();

            Assert.NotNull(result);
            Assert.Equal(monthlyEmployeeTotalDto.Month, result.Month);
        }

        [Fact]
        public async Task GetCurrentMonthTotalCreateNewTotalTest()
        {
            EmployeeTypeDto employeeTypeDto = new(2, "Developer");
            EmployeeType employeeType = new(employeeTypeDto);
            EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

            EmployeeDto employeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
               null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty", "D",
               "Missile", new DateTime(), "South Africa", "South African", "0000000000000", " ",
               new DateTime(), null, Models.Enums.Race.Black, Models.Enums.Gender.Female, null,
               "dm@.co.za", "test@gmail.com", "0123456789", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

            var employeeList = new List<EmployeeDto>
            {
                employeeDto
            };

            var employee = new List<Employee>
            {
                new Employee(employeeDto,employeeTypeDto)
            };

            _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                  .Returns(employee.AsQueryable().BuildMock());

            var previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");

            var currentYear = DateTime.Now.Year;
            MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = new(1, 1, 1, 0, 0, 0,previousMonth, currentYear);

            var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
            {
              new MonthlyEmployeeTotal (monthlyEmployeeTotalDto),
            };

            _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
                 .Returns(montlhyEmployeeTotalList.AsQueryable().BuildMock());

            _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

            var result = await employeeService.GetEmployeeCurrentMonthTotal();

            _dbMock.Verify(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(1, result.EmployeeTotal);
            Assert.Equal(1, result.DeveloperTotal);
        }


        [Fact]
        public async Task GetPreviousMonthTotalReturnsExistingTotalTest()
        {

            var previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");

            MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = new(1, 1, 1, 1, 1, 1, previousMonth, 2024);

            var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
            {
              new MonthlyEmployeeTotal (monthlyEmployeeTotalDto),
            };

            _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
                   .Returns(montlhyEmployeeTotalList.AsQueryable().BuildMock());

            var result = await employeeService.GetEmployeePreviousMonthTotal();

            Assert.NotNull(result);
            Assert.Equal(monthlyEmployeeTotalDto.EmployeeTotal, result.EmployeeTotal);
        }


        [Fact]
        public async Task GetPreviousMonthTotalreateNewTotalTest()
        {

            var previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");

            EmployeeTypeDto employeeTypeDto = new(2, "Developer");
            EmployeeType employeeType = new(employeeTypeDto);
            EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

            EmployeeDto employeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
               null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty", "D",
               "Missile", new DateTime(), "South Africa", "South African", "0000000000000", " ",
               new DateTime(), null, Models.Enums.Race.Black, Models.Enums.Gender.Female, null,
               "dm@.co.za", "test@gmail.com", "0123456789", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

            var employeeList = new List<EmployeeDto>
            {
                employeeDto
            };

            var employee = new List<Employee>
            {
                new Employee(employeeDto,employeeTypeDto)
            };

            _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                  .Returns(employee.AsQueryable().BuildMock());

            MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = new(1, 1, 1, 1, 1, 1, "November", 2023);

            var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
            {
              new MonthlyEmployeeTotal (monthlyEmployeeTotalDto),
            };

            _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
                 .Returns(montlhyEmployeeTotalList.AsQueryable().BuildMock());

            _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

            var result = await employeeService.GetEmployeePreviousMonthTotal();

            Assert.NotNull(result);
            Assert.Equal(1, result.EmployeeTotal);
        }

        [Fact]
        public async Task CalculateChurnRateTest()
        {
            var previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");

            EmployeeTypeDto employeeTypeDto = new(2, "Developer");
            EmployeeType employeeType = new(employeeTypeDto);
            EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

            EmployeeDto employeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
               null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty", "D",
               "Missile", new DateTime(), "South Africa", "South African", "0000000000000", " ",
               new DateTime(), null, Models.Enums.Race.Black, Models.Enums.Gender.Female, null,
               "dm@.co.za", "test@gmail.com", "0123456789", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

            var employeeList = new List<EmployeeDto>
            {
                employeeDto
            };

            var employee = new List<Employee>
            {
                new Employee(employeeDto,employeeTypeDto)
            };

            _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                  .Returns(employee.AsQueryable().BuildMock());

            MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = new(1, 1, 1, 1, 1, 1, previousMonth, 2023);

            var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
            {
              new MonthlyEmployeeTotal (monthlyEmployeeTotalDto),
            };

            ChurnRateDataCard churnRateDto = new ChurnRateDataCard
            {
                ChurnRate = Math.Round(0.0, 0),
                DeveloperChurnRate = Math.Round(0.0, 0),
                DesignerChurnRate = Math.Round(0.0, 0),
                ScrumMasterChurnRate = Math.Round(0.0, 0),
                BusinessSupportChurnRate = Math.Round(0.0, 0),
                Month = "January",
                Year = 2024,
            };

            _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
                 .Returns(montlhyEmployeeTotalList.AsQueryable().BuildMock());

            _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

            var result = await employeeService.CalculateEmployeeChurnRate();

            Assert.NotNull(result);
            Assert.Equal(churnRateDto.ChurnRate, result.ChurnRate);
        }

        [Fact]
        public async Task CalculateChurnRateIfStatementTest()
        {
            var previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");

            EmployeeTypeDto employeeTypeDto = new(2, "Developer");
            EmployeeType employeeType = new(employeeTypeDto);
            EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

            EmployeeDto employeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
               null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty", "D",
               "Missile", new DateTime(), "South Africa", "South African", "0000000000000", " ",
               new DateTime(), null, Models.Enums.Race.Black, Models.Enums.Gender.Female, null,
               "dm@.co.za", "test@gmail.com", "0123456789", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

            var employeeList = new List<EmployeeDto>
            {
                employeeDto
            };

            var employee = new List<Employee>
            {
                new Employee(employeeDto,employeeTypeDto)
            };

            _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                  .Returns(employee.AsQueryable().BuildMock());

            MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = new(1, 0, 0, 0, 0, 0, previousMonth, 2023);

            var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
            {
              new MonthlyEmployeeTotal (monthlyEmployeeTotalDto),
            };

            ChurnRateDataCard churnRateDto = new ChurnRateDataCard
            {
                ChurnRate = Math.Round(0.0, 0),
                DeveloperChurnRate = Math.Round(0.0, 0),
                DesignerChurnRate = Math.Round(0.0, 0),
                ScrumMasterChurnRate = Math.Round(0.0, 0),
                BusinessSupportChurnRate = Math.Round(0.0, 0),
                Month = "January",
                Year = 2024,
            };

            _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
                 .Returns(montlhyEmployeeTotalList.AsQueryable().BuildMock());

            _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

            var result = await employeeService.CalculateEmployeeChurnRate();

            Assert.NotNull(result);
            Assert.Equal(churnRateDto.ChurnRate, result.ChurnRate);
        }

        [Fact]
        public async Task GenerateEmployeeDataCardInfomrationTest()
        {
            EmployeeTypeDto employeeTypeDto = new(2, "Developer");
            EmployeeType employeeType = new(employeeTypeDto);
            EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

            EmployeeDto employeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
               null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty", "D",
               "Missile", new DateTime(), "South Africa", "South African", "0000000000000", " ",
               new DateTime(), null, Models.Enums.Race.Black, Models.Enums.Gender.Female, null,
               "dm@.co.za", "test@gmail.com", "0123456789", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

            var employeeList = new List<EmployeeDto>
            {
                employeeDto
            };

            var employee = new List<Employee>
            {
                new Employee(employeeDto,employeeTypeDto)
            };

            _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                  .Returns(employee.AsQueryable().BuildMock());

            MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = new(1, 1, 1, 1, 1, 1, "February", 2024);

            var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
            {
              new MonthlyEmployeeTotal (monthlyEmployeeTotalDto),
            };

            _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
                 .Returns(montlhyEmployeeTotalList.AsQueryable().BuildMock());

            _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

            var result = await employeeService.GenerateDataCardInformation();

            Assert.NotNull(result);
            Assert.Equal(1, result.DevsCount);
        }

    }
}
