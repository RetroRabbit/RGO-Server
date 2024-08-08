using System.Linq.Expressions;
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
    public class BankingAndStarterKitServiceUnitTest
    {
        private readonly Mock<IUnitOfWork> _dbMock;
        private readonly BankingAndStarterKitService _bankingAndStarterKitService;
        private readonly BankingAndStarterKitService _bankingAndStarterKitService2;

        private const int EmployeeId = 1;

        public BankingAndStarterKitServiceUnitTest()
        {
            _dbMock = new Mock<IUnitOfWork>();
            _bankingAndStarterKitService = new BankingAndStarterKitService(_dbMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", 1));
            _bankingAndStarterKitService2 = new BankingAndStarterKitService(_dbMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "Employee", 2));
        }

        [Fact]
        public async Task GetBankingAndStarterKitByEmployeePass()
        {
            var fileName = "TestFile.pdf";

            _dbMock.Setup(m => m.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(true);

            var employeeDocument = new EmployeeDocument { EmployeeId = EmployeeId, FileName = fileName, Employee = EmployeeTestData.EmployeeOne };
            var mockEmployeeDocumentDbSet = new List<EmployeeDocument> { employeeDocument }.AsQueryable().BuildMockDbSet();
            _dbMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                          .Returns(mockEmployeeDocumentDbSet.Object);

            _dbMock.Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
                .Returns(EmployeeBankingTestData.EmployeeBankingOne.ToMockIQueryable());

            _dbMock.Setup(x => x.Employee.FirstOrDefault(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(EmployeeTestData.EmployeeOne);

            var result = await _bankingAndStarterKitService.GetBankingAndStarterKitByEmployeeAsync(EmployeeTestData.EmployeeOne.Email!);
            Assert.NotNull(result);

            var employeeDocDto = employeeDocument.ToDto();
            Assert.Equivalent(result[0].EmployeeDocumentDto, employeeDocDto);

            var employeeBankingDto = EmployeeBankingTestData.EmployeeBankingOne.ToDto();
            Assert.Equivalent(result[1].EmployeeBankingDto, employeeBankingDto);
        }

        [Fact]
        public async Task GetBankingAndStarterKitByEmployeeUnauthorised()
        {
            _dbMock.Setup(m => m.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(true);

            _dbMock.Setup(x => x.Employee.FirstOrDefault(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(EmployeeTestData.EmployeeOne);

            await Assert.ThrowsAsync<CustomException>(() => _bankingAndStarterKitService2.GetBankingAndStarterKitByEmployeeAsync(EmployeeTestData.EmployeeOne.Email!));
        }

        [Fact]
        public async Task GetBankingAndStarterKitByEmployeeFail()
        {
            _dbMock.Setup(m => m.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(false);

            _dbMock.Setup(x => x.Employee.FirstOrDefault(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(EmployeeTestData.EmployeeTwo);

            await Assert.ThrowsAsync<CustomException>(() => _bankingAndStarterKitService.GetBankingAndStarterKitByEmployeeAsync(EmployeeTestData.EmployeeOne.Email!));
        }

        [Fact]
        public async Task GetBankingAndStarterKitPass()
        {
            var fileName = "TestFile.pdf";

            _dbMock.Setup(m => m.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(true);

            var employeeDocument = new EmployeeDocument { EmployeeId = EmployeeId, FileName = fileName, Employee = EmployeeTestData.EmployeeOne };
            var mockEmployeeDocumentDbSet = new List<EmployeeDocument> { employeeDocument }.AsQueryable().BuildMockDbSet();
            _dbMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                          .Returns(mockEmployeeDocumentDbSet.Object);

            _dbMock.Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
                .Returns(EmployeeBankingTestData.EmployeeBankingOne.ToMockIQueryable());

            var result = await _bankingAndStarterKitService.GetBankingAndStarterKitAsync();
            Assert.NotNull(result);

            var employeeDocDto = employeeDocument.ToDto();
            Assert.Equivalent(result[0].EmployeeDocumentDto, employeeDocDto);

            var employeeBankingDto = EmployeeBankingTestData.EmployeeBankingOne.ToDto();
            Assert.Equivalent(result[1].EmployeeBankingDto, employeeBankingDto);
        }

        [Fact]
        public async Task GetBankingAndStarterKitUnauthorised()
        {
            _dbMock.Setup(m => m.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(true);

            await Assert.ThrowsAsync<CustomException>(() => _bankingAndStarterKitService2.GetBankingAndStarterKitAsync());
        }
    }
}
