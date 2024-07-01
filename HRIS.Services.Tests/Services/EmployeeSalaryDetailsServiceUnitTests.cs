using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.Tests.Data;
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

        private const int EmployeeId = 1;
        private readonly Employee _testEmployee = EmployeeTestData.EmployeeOne;

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
            var salary = 25000.00d;
            _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                        .Returns(_testEmployee.EntityToList().AsQueryable().BuildMockDbSet().Object);

            var employeeSalaryDetails = new EmployeeSalaryDetails
            {
                EmployeeId = EmployeeId,
                Salary = salary
            };

            var mockEmployeeSalaryDetailsDbSet = new List<EmployeeSalaryDetails> { employeeSalaryDetails }.AsQueryable().BuildMockDbSet();

            _unitOfWorkMock.Setup(m => m.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                           .Returns(mockEmployeeSalaryDetailsDbSet.Object);

            var result = await _employeeSalaryDetailsService.GetEmployeeSalary(EmployeeId);
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
                EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne,
                EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsTwo
            };

            _unitOfWorkMock
                .Setup(m => m.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                .Returns(employeeSalaries.ToMockIQueryable());

            _employeeSalaryDetailsServiceMock.Setup(r => r.GetAllEmployeeSalaries());

            _unitOfWorkMock.Setup(x => x.EmployeeSalaryDetails.GetAll(null)).ReturnsAsync(employeeSalaries);

            var result = await _employeeSalaryDetailsService.GetAllEmployeeSalaries();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equivalent(employeeSalaries.Select(x => x.ToDto()).ToList(), result);
            Assert.IsType<List<EmployeeSalaryDetailsDto>>(result);
        }

        [Fact]
        public async Task CheckEmployeeFail()
        {
            _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                           .Returns(new List<Employee>().ToMockIQueryable());

            var result = await _employeeSalaryDetailsService.CheckEmployee(EmployeeId);
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateEmployeeDocumentPass()
        {
            _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(_testEmployee.EntityToList().AsQueryable().BuildMockDbSet().Object);

            _unitOfWorkMock.Setup(m => m.EmployeeSalaryDetails.Update(It.IsAny<EmployeeSalaryDetails>()))
                        .ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne);

            var service = new EmployeeSalaryDetailsService(_unitOfWorkMock.Object, _errorLoggingServiceMock.Object);
            var results = await service.UpdateEmployeeSalary(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto());
            Assert.NotNull(results);
            Assert.Equivalent(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto(), results);
        }

        [Fact]
        public async Task SaveEmployeeSalaryFail()
        {
            _unitOfWorkMock
                .Setup(m => m.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                .Returns(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToMockIQueryable());

            _employeeSalaryDetailsServiceMock.Setup(r => r.SaveEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>())).ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto());

            _unitOfWorkMock.Setup(r => r.EmployeeSalaryDetails.Add(It.IsAny<EmployeeSalaryDetails>())).ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne);

            _unitOfWorkMock.Setup(x => x.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                           .ReturnsAsync(true);

            _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception("Employee salary already exists"));

            var exception = await Assert.ThrowsAnyAsync<Exception>(() => _employeeSalaryDetailsService
                .SaveEmployeeSalary(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto()));

            Assert.Equal("Employee salary already exists", exception.Message);
        }

        [Fact]
        public async Task SaveEmployeeSalaryPass()
        {
            _unitOfWorkMock
                .Setup(m => m.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                .Returns(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToMockIQueryable());

            _employeeSalaryDetailsServiceMock.Setup(r => r.SaveEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>())).ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto());

            _unitOfWorkMock.Setup(r => r.EmployeeSalaryDetails.Add(It.IsAny<EmployeeSalaryDetails>())).ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne);

            _unitOfWorkMock.Setup(x => x.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                           .ReturnsAsync(true);

            var result = await _employeeSalaryDetailsService.SaveEmployeeSalary(new EmployeeSalaryDetailsDto { Id = 0, EmployeeId = 7, Band = EmployeeSalaryBand.Level1, Salary = 1090, MinSalary = 1900, MaxSalary = 25990, Remuneration = 1599, Contribution = null, SalaryUpdateDate = new DateTime() });

            _unitOfWorkMock.Verify(x => x.EmployeeSalaryDetails.Add(It.IsAny<EmployeeSalaryDetails>()), Times.Once);

            Assert.NotNull(result);
            Assert.Equivalent(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto(), result);
            _unitOfWorkMock.Verify(x => x.EmployeeSalaryDetails.Add(It.IsAny<EmployeeSalaryDetails>()), Times.Once);
        }

        [Fact]
        public async Task DeleteSalaryPass()
        {
            _unitOfWorkMock.Setup(x => x.EmployeeSalaryDetails.GetAll(null))
                           .ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToMockDbSet().Object.ToList());

            _unitOfWorkMock.Setup(x => x.EmployeeSalaryDetails.Delete(It.IsAny<int>()))
                           .ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne);

            var result = await _employeeSalaryDetailsService.DeleteEmployeeSalary(EmployeeId);
            Assert.NotNull(result);
            Assert.Equivalent(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto(), result);
            _unitOfWorkMock.Verify(r => r.EmployeeSalaryDetails.Delete(It.IsAny<int>()), Times.Once);
        }
    }
}