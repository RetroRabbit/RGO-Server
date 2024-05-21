//TODO: Complete Unit Tests

//using System.Linq.Expressions;
//using HRIS.Models;
//using HRIS.Models.Enums;
//using HRIS.Services.Interfaces;
//using HRIS.Services.Services;
//using MockQueryable.Moq;
//using Moq;
//using RGO.Tests.Data.Models;
//using RR.Tests.Data.Models.HRIS;
//using RR.UnitOfWork;
//using RR.UnitOfWork.Entities.HRIS;
//using Xunit;

//namespace HRIS.Services.Tests.Services
//{
//    public class EmployeeSalaryDetailsServiceUnitTest
//    {
//        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
//        private readonly Mock<IEmployeeService> _employeeServiceMock;
//        private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
//        private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
//        private readonly Mock<IEmployeeSalarayDetailsService> _employeeSalaryDetailsServiceMock;
//        private readonly Mock<EmployeeSalaryDetails> _employeeSalaryDetailsMock;
//        private readonly EmployeeSalaryDetailsService _employeeSalaryDetailsService;
//        private readonly EmployeeSalaryDetailsDto _employeeSalaryDetailsDto;

//        public EmployeeSalaryDetailsServiceUnitTest()
//        {
//            _unitOfWorkMock = new Mock<IUnitOfWork>();
//            _employeeServiceMock = new Mock<IEmployeeService>();
//            _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
//            _employeeSalaryDetailsMock = new Mock<EmployeeSalaryDetails>();
//            _employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
//            _employeeSalaryDetailsService = new EmployeeSalaryDetailsService(_unitOfWorkMock.Object, _errorLoggingServiceMock.Object);
//            _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
//        }

//        int employeeId = 1;
//        static Employee testEmployee = new Employee(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType);

//        [Fact]
//        public async Task GetEmployeeSalaryPass()
//        {
//            double salary = 25000.00;

//            var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMockDbSet();
//            _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
//                        .Returns(mockEmployeeDbSet.Object);

//            var employeeSalaryDetails = new EmployeeSalaryDetails { EmployeeId = employeeId, Salary = salary };
//            var mockEmployeeSalaryDetailsDbSet = new List<EmployeeSalaryDetails> { employeeSalaryDetails }.AsQueryable().BuildMockDbSet();
//            _unitOfWorkMock.Setup(m => m.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()));

//            var service = new EmployeeSalaryDetailsService(_unitOfWorkMock.Object, _errorLoggingServiceMock.Object);

//            var result = await service.GetEmployeeSalary(employeeId);

//            Assert.NotNull(result);
//            Assert.Equal(salary, result.Salary);
//        }


//        [Fact]
//        public async Task GetAllEmployeeSalariesPass()
//        {
//            var salaryDto = new EmployeeSalaryDetailsDto
//            {
//                Id = 1,
//                EmployeeId = 1,
//                Salary = 2000,
//                MinSalary = 1500,
//                MaxSalary = 3000,
//                Remuneration = 2500,
//                Band = EmployeeSalaryBand.Level1,
//                Contribution = null

//            };

//            var obj = _unitOfWorkMock
//            .Setup(m => m.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
//            .Returns(new List<EmployeeSalaryDetails>
//            {
//                new EmployeeSalaryDetails(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1),
//                new EmployeeSalaryDetails(EmployeeSalaryDetailsTestData.EmployeeSalaryTest2)
//            }
//            .AsQueryable().BuildMock());

//            var result = await _employeeSalaryDetailsService.GetAllEmployeeSalaries();

//            Assert.Equal(1, result.Count);
//            Assert.NotNull(result);
//            Assert.IsType<List<EmployeeSalaryDetailsDto>>(result);
//        }

//        [Fact]
//        public async Task CheckEmployeeFail()
//        {
//            var mockEmployeeDbSet = new List<Employee>();
//            _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
//                           .Returns(mockEmployeeDbSet.AsQueryable().BuildMock());

//            var result = await _employeeSalaryDetailsService.CheckEmployee(employeeId);
//            Assert.False(result);

//        }

//        [Fact]
//        public async Task UpdateEmployeeDocumentPass()
//        {
//            var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMockDbSet();
//            _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
//                      .Returns(mockEmployeeDbSet.Object);

//            var employeeSalaryDetails = new EmployeeSalaryDetails(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1);

//            _unitOfWorkMock.Setup(m => m.EmployeeSalaryDetails.Update(It.IsAny<EmployeeSalaryDetails>()))
//                        .ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1);
//            var service = new EmployeeSalaryDetailsService(_unitOfWorkMock.Object, _errorLoggingServiceMock.Object);

//            var results = await service.UpdateEmployeeSalary(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1);

//            Assert.NotNull(results);
//        }

//        [Fact]
//        public async Task SaveEmployeeSalaryFail()
//        {
//            _employeeServiceMock.Setup(x => x.GetById(employeeId))
//                                .ReturnsAsync((EmployeeDto?)null);

//            _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception("employee not found"));

//            var exception = await Assert.ThrowsAnyAsync<Exception>(() => _employeeSalaryDetailsService
//                .SaveEmployeeSalary(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1));

//            Assert.Equal("employee not found", exception.Message);

//            _employeeServiceMock.Verify(x => x.GetById(employeeId), Times.Once);
//        }

//        [Fact]
//        public async Task SaveEmployeeSalaryPass()
//        {
//            _unitOfWorkMock.Setup(x => x.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
//                        .ReturnsAsync(false);

//            await _employeeSalaryDetailsService.SaveEmployeeSalary(new EmployeeSalaryDetailsDto { Id = 1, EmployeeId = 1, Band = EmployeeSalaryBand.Level1, Salary = 1000, MinSalary = 1000, MaxSalary = 25000, Remuneration = 1500, Contribution = null });

//            _unitOfWorkMock.Verify(x => x.EmployeeSalaryDetails.Add(It.IsAny<EmployeeSalaryDetails>()), Times.Once);
//        }

//        [Fact]
//        public async Task DeleteSalaryPass()
//        {
//            var salaries = new List<EmployeeSalaryDetailsDto> { _employeeSalaryDetailsDto };
//            _unitOfWorkMock.Setup(x => x.EmployeeSalaryDetails.GetAll(null)).Returns(Task.FromResult(salaries));

//            _unitOfWorkMock.Setup(x => x.EmployeeSalaryDetails.Delete(It.IsAny<int>()))
//                   .Returns(Task.FromResult(_employeeSalaryDetailsDto));

//            var result = await _employeeSalaryDetailsService.DeleteEmployeeSalary(employeeId);
//            Assert.NotNull(result);
//            Assert.Equal(_employeeSalaryDetailsDto, result);
//            _unitOfWorkMock.Verify(r => r.EmployeeSalaryDetails.Delete(It.IsAny<int>()), Times.Once);
//        }
//    }
//}