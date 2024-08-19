using System;
using System.Linq.Expressions;
using HRIS.Models.Employee.Commons;
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
        private readonly Mock<IUnitOfWork> _dbMock;
        private readonly Mock<IEmployeeSalaryDetailsService> _employeeSalaryDetailsServiceMock;
        private readonly EmployeeSalaryDetailsService _employeeSalaryDetailsService;

        private const int EmployeeId = 1;
        private readonly Employee _testEmployee = EmployeeTestData.EmployeeOne;
        private readonly Mock<AuthorizeIdentityMock> _identity;
        private readonly EmployeeSalaryDetails _employeeSalaryDetails;

        public EmployeeSalaryDetailsServiceUnitTest()
        {
            _identity = new Mock<AuthorizeIdentityMock>();
            _dbMock = new Mock<IUnitOfWork>();
            _employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalaryDetailsService>();
            _employeeSalaryDetailsService = new EmployeeSalaryDetailsService(_dbMock.Object, _identity.Object);

            _employeeSalaryDetails = new EmployeeSalaryDetails
            {
                EmployeeId = EmployeeId,
                Salary = 25000.00d
            };
        }

        [Fact]
        public async Task GetEmployeeSalaryDetailsById_Success()
        {
            _identity.Setup(i => i.Role).Returns("Admin");
            _identity.Setup(x => x.EmployeeId).Returns(1);

            _dbMock.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                .ReturnsAsync(true);

            var mockEmployeeSalaryDetailsDbSet = new List<EmployeeSalaryDetails> { _employeeSalaryDetails }.AsQueryable().BuildMockDbSet();

            _dbMock.Setup(m => m.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                           .Returns(mockEmployeeSalaryDetailsDbSet.Object);

            var result = await _employeeSalaryDetailsService.GetEmployeeSalaryById(EmployeeId);
            Assert.NotNull(result);
            Assert.Equal(_employeeSalaryDetails.Salary, result.Salary);
            Assert.Equal(_employeeSalaryDetails.Salary, result.Salary);
            Assert.IsType<BankingSalaryDetailsDto>(result);
        }

        [Fact]
        public async Task GetEmployeeSalaryDetailsById_Unauthorized()
        {
            _identity.Setup(i => i.Role).Returns("Employee");
            _identity.SetupGet(i => i.EmployeeId).Returns(2);

            _dbMock.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(true);
            _dbMock.Setup(x => x.EmployeeSalaryDetails.GetById(It.IsAny<int>()))
            .ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne);

            var exception = await Assert.ThrowsAsync<CustomException>(() =>
               _employeeSalaryDetailsService.GetEmployeeSalaryById(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.EmployeeId));

            Assert.Equivalent("Unauthorized Access.", exception.Message);
        }

        [Fact]
        public async Task GetEmployeeSalaryDetailsById_DoesNotExist()
        {
            _dbMock.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                .ReturnsAsync(false);

            var mockEmployeeSalaryDetailsDbSet = new List<EmployeeSalaryDetails> { _employeeSalaryDetails }.AsQueryable().BuildMockDbSet();

            _dbMock.Setup(m => m.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                           .Returns(mockEmployeeSalaryDetailsDbSet.Object);

            await Assert.ThrowsAsync<CustomException>(() => _employeeSalaryDetailsService.GetEmployeeSalaryById(EmployeeId));

            _dbMock.Verify(x => x.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()), Times.Never);
            _dbMock.Verify(x => x.EmployeeSalaryDetails.FirstOrDefault(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()), Times.Never);
        }

        [Fact]
        public async Task GetAllEmployeeSalaryDetails_Success()
        {
            var employeeSalaries = new List<EmployeeSalaryDetails>
            {
                EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne,
                EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsTwo
            };

            _dbMock
                .Setup(m => m.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                .Returns(employeeSalaries.ToMockIQueryable());

            _employeeSalaryDetailsServiceMock.Setup(r => r.GetAllEmployeeSalaries());

            _dbMock.Setup(x => x.EmployeeSalaryDetails.GetAll(null)).ReturnsAsync(employeeSalaries);

            var result = await _employeeSalaryDetailsService.GetAllEmployeeSalaries();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equivalent(employeeSalaries.Select(x => x.ToDto()).ToList(), result);
            Assert.IsType<List<BankingSalaryDetailsDto>>(result);
        }

        [Fact]
        public async Task CheckIfEmployeeSalaryDetailsReturnsTrue()
        {
            var testId = 1;
            _dbMock.Setup(r => r.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                   .ReturnsAsync(true);

            var result = await _employeeSalaryDetailsService.EmployeeSalaryDetailsExists(testId);

            Assert.True(result);
            _dbMock.Verify(x => x.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task CheckIfEmployeeSalaryDetailsReturnsFalse()
        {
            var testId = 1;
            _dbMock.Setup(x => x.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                   .ReturnsAsync(false);

            var result = await _employeeSalaryDetailsService.EmployeeSalaryDetailsExists(testId);

            Assert.False(result);
            _dbMock.Verify(x => x.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeSalaryDetails_Success()
        {
            _identity.Setup(i => i.Role).Returns("Admin");
            _identity.Setup(x => x.EmployeeId).Returns(1);

            _dbMock.Setup(x => x.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
               .ReturnsAsync(true);
            _dbMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(_testEmployee.EntityToList().AsQueryable().BuildMockDbSet().Object);
            _dbMock.Setup(m => m.EmployeeSalaryDetails.Update(It.IsAny<EmployeeSalaryDetails>()))
                        .ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne);

            var service = new EmployeeSalaryDetailsService(_dbMock.Object, _identity.Object);
            var results = await service.UpdateEmployeeSalary(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto());
            Assert.NotNull(results);
            Assert.Equivalent(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto(), results);
        }

        [Fact]
        public async Task UpdateEmployeeSalaryDetails_DoesNotExist()
        {
            _dbMock.Setup(x => x.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
               .ReturnsAsync(false);
            _dbMock.Setup(m => m.EmployeeSalaryDetails.Update(It.IsAny<EmployeeSalaryDetails>()))
                        .ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne);

            await Assert.ThrowsAsync<CustomException>(() => _employeeSalaryDetailsService.UpdateEmployeeSalary(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto()));

            _dbMock.Verify(x => x.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()), Times.Never);
            _dbMock.Verify(x => x.EmployeeSalaryDetails.FirstOrDefault(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()), Times.Never);
        }

        [Fact]
        public async Task UpdateEmployeeSalaryDetails_Unauthorized()
        {
            _identity.Setup(i => i.Role).Returns("Employee");
            _identity.Setup(x => x.EmployeeId).Returns(5);

            _dbMock.Setup(x => x.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
               .ReturnsAsync(true);
            _dbMock.Setup(m => m.EmployeeSalaryDetails.Update(It.IsAny<EmployeeSalaryDetails>()))
                        .ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne);

            var exception = await Assert.ThrowsAsync<CustomException>(() => _employeeSalaryDetailsService.UpdateEmployeeSalary(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto()));

            Assert.Equivalent("Unauthorized Access.", exception.Message);
        }

        [Fact]
        public async Task CreateEmployeeSalaryDetails_Success()
        {
            _identity.Setup(i => i.Role).Returns("Admin");
            _identity.Setup(x => x.EmployeeId).Returns(1);
            _employeeSalaryDetailsServiceMock.Setup(x => x.EmployeeSalaryDetailsExists(0)).ReturnsAsync(true);

            _dbMock
                .Setup(m => m.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                .Returns(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToMockIQueryable());

            _employeeSalaryDetailsServiceMock.Setup(r => r.CreateEmployeeSalary(It.IsAny<BankingSalaryDetailsDto>())).ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto());

            _dbMock.Setup(r => r.EmployeeSalaryDetails.Add(It.IsAny<EmployeeSalaryDetails>())).ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne);

            var result = await _employeeSalaryDetailsService.CreateEmployeeSalary(new BankingSalaryDetailsDto { Id = 0, EmployeeId = 7, Band = EmployeeSalaryBand.Level1, Salary = 1090, MinSalary = 1900, MaxSalary = 25990, Remuneration = 1599, Contribution = null, SalaryUpdateDate = new DateTime() });

            _dbMock.Verify(x => x.EmployeeSalaryDetails.Add(It.IsAny<EmployeeSalaryDetails>()), Times.Once);

            Assert.NotNull(result);
            Assert.Equivalent(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto(), result);
            _dbMock.Verify(x => x.EmployeeSalaryDetails.Add(It.IsAny<EmployeeSalaryDetails>()), Times.Once);
        }

        [Fact]
        public async Task CreateEmployeeSalaryDetails_DoesNotExist()
        {
            _employeeSalaryDetailsServiceMock.Setup(r => r.CreateEmployeeSalary(It.IsAny<BankingSalaryDetailsDto>())).ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto());

            _dbMock.Setup(r => r.EmployeeSalaryDetails.Add(It.IsAny<EmployeeSalaryDetails>())).ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne);
            _dbMock.Setup(x => x.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                           .ReturnsAsync(true);

            await Assert.ThrowsAnyAsync<CustomException>(() => _employeeSalaryDetailsService
                .CreateEmployeeSalary(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto()));

            _dbMock.Verify(x => x.EmployeeSalaryDetails.Update(It.IsAny<EmployeeSalaryDetails>()), Times.Never);
            _dbMock.Verify(x => x.EmployeeSalaryDetails.FirstOrDefault(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()), Times.Never);
        }

        [Fact]
        public async Task CreateEmployeeSalaryDetails_Unauthorized()
        {
            _identity.Setup(i => i.Role).Returns("Employee");
            _identity.Setup(x => x.EmployeeId).Returns(5);

            _dbMock.Setup(x => x.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                          .ReturnsAsync(false);
            _dbMock.Setup(r => r.EmployeeSalaryDetails.Add(It.IsAny<EmployeeSalaryDetails>())).ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne);

            _employeeSalaryDetailsServiceMock.Setup(r => r.CreateEmployeeSalary(It.IsAny<BankingSalaryDetailsDto>())).ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto());

            var exception = await Assert.ThrowsAnyAsync<CustomException>(() => _employeeSalaryDetailsService
                .CreateEmployeeSalary(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto()));

            Assert.Equivalent("Unauthorized Access.", exception.Message);
        }

        [Fact]
        public async Task DeleteEmployeeSalaryDetails_Success()
        {
            _identity.Setup(i => i.Role).Returns("Admin");
            _identity.Setup(x => x.EmployeeId).Returns(1);

            _dbMock.Setup(x => x.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                           .ReturnsAsync(true);
            _dbMock.Setup(x => x.EmployeeSalaryDetails.GetAll(null))
                           .ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToMockDbSet().Object.ToList());
            _dbMock.Setup(x => x.EmployeeSalaryDetails.Delete(It.IsAny<int>()))
                           .ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne);

            var result = await _employeeSalaryDetailsService.DeleteEmployeeSalary(EmployeeId);
            Assert.NotNull(result);
            Assert.Equivalent(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto(), result);
            _dbMock.Verify(r => r.EmployeeSalaryDetails.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeSalaryDetails_DoesNotExist()
        {
            _dbMock.Setup(x => x.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                           .ReturnsAsync(false);
            _dbMock.Setup(x => x.EmployeeSalaryDetails.GetAll(null))
                           .ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToMockDbSet().Object.ToList());
            _dbMock.Setup(x => x.EmployeeSalaryDetails.Delete(It.IsAny<int>()))
                           .ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne);

            await Assert.ThrowsAnyAsync<CustomException>(() => _employeeSalaryDetailsService
               .DeleteEmployeeSalary(EmployeeId));

            _dbMock.Verify(x => x.EmployeeSalaryDetails.Update(It.IsAny<EmployeeSalaryDetails>()), Times.Never);
            _dbMock.Verify(x => x.EmployeeSalaryDetails.FirstOrDefault(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()), Times.Never);
        }

        [Fact]
        public async Task DeleteEmployeeSalaryDetails_Unauthorized()
        {
            _identity.Setup(i => i.Role).Returns("Employee");
            _identity.Setup(x => x.EmployeeId).Returns(5);

            _dbMock.Setup(x => x.EmployeeSalaryDetails.Any(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
                           .ReturnsAsync(true);
            _dbMock.Setup(x => x.EmployeeSalaryDetails.GetAll(null))
                           .ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToMockDbSet().Object.ToList());
            _dbMock.Setup(x => x.EmployeeSalaryDetails.Delete(It.IsAny<int>()))
                           .ReturnsAsync(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne);

            var exception = await Assert.ThrowsAnyAsync<CustomException>(() => _employeeSalaryDetailsService
              .DeleteEmployeeSalary(EmployeeId));

            Assert.Equivalent("Unauthorized Access.", exception.Message);
        }
    }
}