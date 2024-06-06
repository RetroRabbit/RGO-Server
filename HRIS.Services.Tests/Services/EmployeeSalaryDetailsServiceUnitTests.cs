using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services
{
    public class EmployeeSalaryDetailsServiceUnitTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
        private readonly Mock<IEmployeeSalarayDetailsService> _employeeSalaryDetailsServiceMock;
        private readonly EmployeeSalaryDetailsService _employeeSalaryDetailsService;

        int employeeId = 1;
        static Employee testEmployee = new Employee(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType);

        public EmployeeSalaryDetailsServiceUnitTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
            _employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
            _employeeSalaryDetailsService = new EmployeeSalaryDetailsService(_unitOfWorkMock.Object, _errorLoggingServiceMock.Object);
        }

        [Fact]
        public async Task GetEmployeeSalaryPass()
        {
            double salary = 25000.00;
            var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMockDbSet();
            _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                        .Returns(mockEmployeeDbSet.Object);

            var employeeSalaryDetails = new EmployeeSalaryDetails
            {
                EmployeeId = employeeId,
                Salary = salary
            };

            var mockEmployeeSalaryDetailsDbSet = new List<EmployeeSalaryDetails> { employeeSalaryDetails }.AsQueryable().BuildMockDbSet();

            _unitOfWorkMock.Setup(m => m.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                           .Returns(mockEmployeeSalaryDetailsDbSet.Object);

            var result = await _employeeSalaryDetailsService.GetEmployeeSalary(employeeId);
            Assert.NotNull(result);
            Assert.Equal(salary, result.Salary);
            Assert.Equal(employeeSalaryDetails.Salary, result.Salary);
            Assert.IsType<EmployeeSalaryDetailsDto>(result);
        }

        [Fact]
        public async Task GetAllEmployeeSalariesPass()
        {
            var employeeSalaries = new List<EmployeeSalaryDetails>
            {
                new EmployeeSalaryDetails(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1),
                new EmployeeSalaryDetails(EmployeeSalaryDetailsTestData.EmployeeSalaryTest2)
            };

            var employeeSalariesDto = new List<EmployeeSalaryDetailsDto>
            {
                EmployeeSalaryDetailsTestData.EmployeeSalaryTest1,
                EmployeeSalaryDetailsTestData.EmployeeSalaryTest2
            };

            var obj = _unitOfWorkMock
            .Setup(m => m.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
            .Returns(employeeSalaries.AsQueryable().BuildMock());

            _employeeSalaryDetailsServiceMock.Setup(r => r.GetAllEmployeeSalaries());

            _unitOfWorkMock.Setup(x => x.EmployeeSalaryDetails.GetAll(null)).Returns(Task.FromResult(employeeSalariesDto));

            var result = await _employeeSalaryDetailsService.GetAllEmployeeSalaries();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(employeeSalariesDto, result);
            Assert.IsType<List<EmployeeSalaryDetailsDto>>(result);
        }

        [Fact]
        public async Task CheckEmployeeFail()
        {
            var mockEmployeeDbSet = new List<Employee>();

            _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                           .Returns(mockEmployeeDbSet.AsQueryable().BuildMock());

            var result = await _employeeSalaryDetailsService.CheckEmployee(employeeId);
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateEmployeeDocumentPass()
        {
            var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMockDbSet();
            _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet.Object);

            _unitOfWorkMock.Setup(m => m.EmployeeSalaryDetails.Update(It.IsAny<EmployeeSalaryDetails>()))
                        .ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1);

            var service = new EmployeeSalaryDetailsService(_unitOfWorkMock.Object, _errorLoggingServiceMock.Object);
            var results = await service.UpdateEmployeeSalary(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1);
            Assert.NotNull(results);
            Assert.Equal(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1, results);
        }

        [Fact]
        public async Task SaveEmployeeSalaryFail()
        {
            var obj = _unitOfWorkMock
            .Setup(m => m.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
            .Returns(new List<EmployeeSalaryDetails>
            {
                new EmployeeSalaryDetails(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1)
            }
            .AsQueryable().BuildMock());

            _employeeSalaryDetailsServiceMock.Setup(r => r.SaveEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>())).Returns(Task.FromResult(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1));

            _unitOfWorkMock.Setup(r => r.EmployeeSalaryDetails.Add(It.IsAny<EmployeeSalaryDetails>())).Returns(Task.FromResult(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1));

            _unitOfWorkMock.Setup(x => x.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                           .ReturnsAsync(true);

            _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception("Employee salary already exists"));

            var exception = await Assert.ThrowsAnyAsync<Exception>(() => _employeeSalaryDetailsService
                .SaveEmployeeSalary(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1));
            
            Assert.Equal("Employee salary already exists", exception.Message);
        }

        [Fact]
        public async Task SaveEmployeeSalaryPass()
        {
            var obj = _unitOfWorkMock
           .Setup(m => m.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
           .Returns(new List<EmployeeSalaryDetails>
           {
                new EmployeeSalaryDetails(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1)
           }
           .AsQueryable().BuildMock());

            _employeeSalaryDetailsServiceMock.Setup(r => r.SaveEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>())).Returns(Task.FromResult(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1));

            _unitOfWorkMock.Setup(r => r.EmployeeSalaryDetails.Add(It.IsAny<EmployeeSalaryDetails>())).Returns(Task.FromResult(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1));

            _unitOfWorkMock.Setup(x => x.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                           .ReturnsAsync(true);

            var result = await _employeeSalaryDetailsService.SaveEmployeeSalary(new EmployeeSalaryDetailsDto { Id = 0, EmployeeId = 7, Band = EmployeeSalaryBand.Level1, Salary = 1090, MinSalary = 1900, MaxSalary = 25990, Remuneration = 1599, Contribution = null, SalaryUpdateDate = new DateTime() });

            _unitOfWorkMock.Verify(x => x.EmployeeSalaryDetails.Add(It.IsAny<EmployeeSalaryDetails>()), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1, result);
            _unitOfWorkMock.Verify(x => x.EmployeeSalaryDetails.Add(It.IsAny<EmployeeSalaryDetails>()), Times.Once);
        }

        [Fact]
        public async Task DeleteSalaryPass()
        {
            var mockEmployeeSalaryDetailsDbSet = new List<EmployeeSalaryDetailsDto> { EmployeeSalaryDetailsTestData.EmployeeSalaryTest1 }.AsQueryable().BuildMockDbSet();
            _unitOfWorkMock.Setup(x => x.EmployeeSalaryDetails.GetAll(null))
                           .ReturnsAsync(mockEmployeeSalaryDetailsDbSet.Object.ToList());

            _unitOfWorkMock.Setup(x => x.EmployeeSalaryDetails.Delete(It.IsAny<int>()))
                           .ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1);

            var result = await _employeeSalaryDetailsService.DeleteEmployeeSalary(employeeId);
            Assert.NotNull(result);
            Assert.Equal(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1, result);
            _unitOfWorkMock.Verify(r => r.EmployeeSalaryDetails.Delete(It.IsAny<int>()), Times.Once);
        }
    }
}