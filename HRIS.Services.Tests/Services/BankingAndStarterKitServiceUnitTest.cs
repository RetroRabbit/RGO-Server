using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RGO.Tests.Data.Models;
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

        private const int EmployeeId = 1;

        public BankingAndStarterKitServiceUnitTest()
        {
            _dbMock = new Mock<IUnitOfWork>();
            _bankingAndStarterKitService = new BankingAndStarterKitService(_dbMock.Object);
        }

        [Fact]
        public async Task GetBankingAndStarterKitByIdPass()
        {
            var fileName = "TestFile.pdf";

            var mockEmployeeDbSet = EmployeeTestData.EmployeeOne.EntityToList().AsQueryable().BuildMockDbSet();
            _dbMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(mockEmployeeDbSet.Object);

            var employeeDocument = new EmployeeDocument { EmployeeId = EmployeeId, FileName = fileName, Employee = EmployeeTestData.EmployeeOne };
            var mockEmployeeDocumentDbSet = new List<EmployeeDocument> { employeeDocument }.AsQueryable().BuildMockDbSet();
            _dbMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                          .Returns(mockEmployeeDocumentDbSet.Object);

            _dbMock.Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
                .Returns(EmployeeBankingTestData.EmployeeBankingOne.ToMockIQueryable());

            var result = await _bankingAndStarterKitService.GetBankingAndStarterKitByIdAsync(EmployeeId);
            Assert.NotNull(result);

            var employeeDocDto = employeeDocument.ToDto();
            Assert.Equivalent(result[0].EmployeeDocumentDto, employeeDocDto);

            var employeeBankingDto = EmployeeBankingTestData.EmployeeBankingOne.ToDto();
            Assert.Equivalent(result[1].EmployeeBankingDto, employeeBankingDto);
        }
    }
}
