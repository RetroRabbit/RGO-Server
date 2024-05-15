using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RGO.Tests.Data.Models;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services
{
    public class EmployeeSalaryDetailsServiceUnitTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
        private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
        private readonly Mock<IEmployeeSalarayDetailsService> _employeeSalaryDetailsServiceMock;
        //private readonly Mock<SimpleEmployeeDocumentGetAllDto> _simpleEmployeeDocumentGetAllDtoMock;
        private readonly Mock<EmployeeSalaryDetails> _employeeSalaryDetailsMock;
        private readonly EmployeeSalaryDetailsService _employeeSalaryDetailsService;

        public EmployeeSalaryDetailsServiceUnitTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _employeeServiceMock = new Mock<IEmployeeService>();
            _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
            _employeeSalaryDetailsMock = new Mock<EmployeeSalaryDetails>();
            _employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
            _employeeSalaryDetailsService = new EmployeeSalaryDetailsService(_unitOfWorkMock.Object, _errorLoggingServiceMock.Object);
            //_simpleEmployeeDocumentGetAllDtoMock = new Mock<SimpleEmployeeDocumentGetAllDto>();
            _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        }

        int employeeId = 1;
        static Employee testEmployee = new Employee(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType);

        [Fact]
        public async Task GetEmployeeSalaryPass()
        {
            double salary = 25000.00;

            var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMockDbSet();
            _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                        .Returns(mockEmployeeDbSet.Object);

            var employeeSalaryDetails = new EmployeeSalaryDetails { EmployeeId = employeeId, Salary = salary };
            var mockEmployeeSalaryDetailsDbSet = new List<EmployeeSalaryDetails> { employeeSalaryDetails }.AsQueryable().BuildMockDbSet();
            _unitOfWorkMock.Setup(m => m.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()));

            var service = new EmployeeSalaryDetailsService(_unitOfWorkMock.Object, _errorLoggingServiceMock.Object);

            var result = await service.GetEmployeeSalary(employeeId);

            Assert.NotNull(result);
            Assert.Equal(salary, result.Salary);
        }

        /*
        [Fact]
        public async Task GetAllEmployeeSalariesPass()
        {
            _unitOfWorkMock
            .Setup(m => m.EmployeeSalaryDetails.Get(It.IsAny<Expression<Func<EmployeeSalaryDetails, bool>>>()))
            .Returns(new List<EmployeeSalaryDetails>
            {
                new EmployeeSalaryDetails()
            })











            //////////////////////////////////////////////////////////////

            _unitOfWorkMock
            .Setup(x =>
                       x.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
            .Returns(new List<EmployeeDocument> {
                new EmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentApproved){Employee = new Employee(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)},
                new EmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentApproved){Employee = new Employee(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)}
            }
            .AsQueryable().BuildMock());

            var employeeDocumentGetAllList = new List<SimpleEmployeeDocumentGetAllDto>
        {
            new SimpleEmployeeDocumentGetAllDto { EmployeeDocumentDto = EmployeeDocumentTestData.EmployeeDocumentApproved, Name = "Pieter", Surname = "Pietersen" },
            new SimpleEmployeeDocumentGetAllDto { EmployeeDocumentDto = EmployeeDocumentTestData.EmployeeDocumentApproved, Name = "Pieter1", Surname = "Pietersen2"}
        };

            _employeeDocumentServiceMock.Setup(x => x.GetAllDocuments()).Returns(Task.FromResult(employeeDocumentGetAllList));

            var result = await _employeeDocumentService.GetAllDocuments();

            Assert.NotNull(result);
            Assert.IsType<List<SimpleEmployeeDocumentGetAllDto>>(result);
        }
        */
    }
}