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

namespace HRIS.Services.Tests.Services;
public class EmployeeDocumentServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEmployeeService> _employeeServiceMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
    private readonly Mock<IEmployeeDocumentService> _employeeDocumentServiceMock;
    private readonly Mock<SimpleEmployeeDocumentGetAllDto> _simpleEmployeeDocumentGetAllDtoMock;
    private readonly Mock<EmployeeDocument> _employeeDocumentMock;
    private readonly EmployeeDocumentService _employeeDocumentService;

    public EmployeeDocumentServiceUnitTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _employeeServiceMock = new Mock<IEmployeeService>();
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>(); 
        _employeeDocumentMock = new Mock<EmployeeDocument>();
        _employeeDocumentServiceMock = new Mock<IEmployeeDocumentService>();
        _employeeDocumentService = new EmployeeDocumentService(_unitOfWorkMock.Object, _employeeServiceMock.Object, _errorLoggingServiceMock.Object);
        _simpleEmployeeDocumentGetAllDtoMock = new Mock<SimpleEmployeeDocumentGetAllDto>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
    }

    int employeeId = 1;
    static Employee testEmployee = new Employee(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType);

    [Fact]
    public async Task SaveEmployeeDocumentPass()
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType);

        _unitOfWorkMock
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(
                new List<Employee>
                {
                    new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
                    {
                        EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType),
                        PhysicalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto),
                        PostalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto)
                    }
                }.AsQueryable().BuildMock());

        _unitOfWorkMock
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(
                new List<EmployeeBanking>
                {
                    new(EmployeeBankingTestData.EmployeeBankingDto)
                }.AsQueryable().BuildMock());



        List<Role> roles = new List<Role> { new Role(EmployeeRoleTestData.RoleDtoEmployee) };
        _employeeServiceMock.Setup(x => x.GetById(employeeId))
            .ReturnsAsync(EmployeeTestData.EmployeeDto);

        _unitOfWorkMock
            .Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
            .Returns(EmployeeRoleTestData.EmployeeRolesList.AsQueryable().BuildMock());

        _unitOfWorkMock
            .Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roles.AsQueryable().BuildMock());

        _unitOfWorkMock.Setup(x => x.EmployeeDocument.Add(It.IsAny<EmployeeDocument>()))
            .ReturnsAsync(EmployeeDocumentTestData.EmployeeDocumentPending);

        var result = await _employeeDocumentService.SaveEmployeeDocument(EmployeeDocumentTestData.SimpleDocumentDto, "test@retrorabbit.co.za", 1);

        Assert.NotNull(result);
        Assert.Equal(EmployeeDocumentTestData.EmployeeDocumentPending, result);
        _employeeServiceMock.Verify(x => x.GetById(employeeId), Times.Once);
        _unitOfWorkMock.Verify(x => x.EmployeeDocument.Add(It.IsAny<EmployeeDocument>()), Times.Once);
    }

    [Fact]
    public async Task SaveEmployeeAdminDocumentPass()
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType);

        _unitOfWorkMock
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(
                new List<Employee>
                {
                    new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
                    {
                        EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType),
                        PhysicalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto),
                        PostalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto)
                    }
                }.AsQueryable().BuildMock());

        _unitOfWorkMock
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(
                new List<EmployeeBanking>
                {
                    new(EmployeeBankingTestData.EmployeeBankingDto)
                }.AsQueryable().BuildMock());

        List<Role> roles = new List<Role> { new Role(EmployeeRoleTestData.RoleDtoAdmin) };

        _unitOfWorkMock
            .Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
            .Returns(EmployeeRoleTestData.EmployeeRolesList.AsQueryable().BuildMock());

        _unitOfWorkMock
            .Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roles.AsQueryable().BuildMock());

        _employeeServiceMock.Setup(x => x.GetById(employeeId))
            .ReturnsAsync(EmployeeTestData.EmployeeDto);

        _unitOfWorkMock.Setup(x => x.EmployeeDocument.Add(It.IsAny<EmployeeDocument>()))
            .ReturnsAsync(EmployeeDocumentTestData.EmployeeDocumentActionRequired);

        var result = await _employeeDocumentService.SaveEmployeeDocument(EmployeeDocumentTestData.SimpleDocumentDto, "test@retrorabbit.co.za", 1);

        Assert.NotNull(result);
        Assert.Equal(EmployeeDocumentTestData.EmployeeDocumentActionRequired, result);
    }

    [Fact]
    public async Task SaveEmployeeDocumentFail()
    {
        _employeeServiceMock.Setup(x => x.GetById(employeeId))
                            .ReturnsAsync((EmployeeDto?)null);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception("employee not found"));

        var exception = await Assert.ThrowsAsync<Exception>(() => _employeeDocumentService
            .SaveEmployeeDocument(EmployeeDocumentTestData.SimpleDocumentDto, "test@retrorabbit.co.za", 1));

        Assert.Equal("employee not found", exception.Message);

        _employeeServiceMock.Verify(x => x.GetById(employeeId), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeDocumentPass()
    {
        string fileName = "TestFile.pdf";

        var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet.Object);

        var employeeDocument = new EmployeeDocument { EmployeeId = employeeId, FileName = fileName };
        var mockEmployeeDocumentDbSet = new List<EmployeeDocument> { employeeDocument }.AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                      .Returns(mockEmployeeDocumentDbSet.Object);

        var service = new EmployeeDocumentService(_unitOfWorkMock.Object, _employeeServiceMock.Object, _errorLoggingServiceMock.Object);

        var result = await service.GetEmployeeDocument(employeeId, fileName,DocumentType.starterKit);

        Assert.NotNull(result);
        Assert.Equal(fileName, result.FileName);
    }

    [Fact]
    public async Task GetAllEmployeeDocumentsPass()
    {
        var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet.Object);

        var employeeDocuments = new List<EmployeeDocument>
        {
            new EmployeeDocument { EmployeeId = employeeId, FileName = "TestFile1.pdf", DocumentType = DocumentType.starterKit },
            new EmployeeDocument { EmployeeId = employeeId, FileName = "TestFile2.pdf", DocumentType = DocumentType.starterKit }
        };

        var mockEmployeeDocumentDbSet = employeeDocuments.AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                      .Returns(mockEmployeeDocumentDbSet.Object);

        var service = new EmployeeDocumentService(_unitOfWorkMock.Object, _employeeServiceMock.Object, _errorLoggingServiceMock.Object);

        var result = await service.GetAllEmployeeDocuments(employeeId, DocumentType.starterKit);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, doc => doc.FileName == "TestFile1.pdf");
        Assert.Contains(result, doc => doc.FileName == "TestFile2.pdf");
    }

    [Fact]
    public async Task UpdateEmployeeDocumentPass()
    {
        var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                  .Returns(mockEmployeeDbSet.Object);

        var employeeDocument = new EmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentPending);

        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Update(It.IsAny<EmployeeDocument>()))
                      .ReturnsAsync(EmployeeDocumentTestData.EmployeeDocumentPending);
        var service = new EmployeeDocumentService(_unitOfWorkMock.Object, _employeeServiceMock.Object, _errorLoggingServiceMock.Object);

        var result = await service.UpdateEmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentPending);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteEmployeeDocumentPass()
    {
        var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet.Object);

        var employeeDocument = new EmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentPending);

        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Delete(It.IsAny<int>()))
                      .Returns(Task.FromResult(EmployeeDocumentTestData.EmployeeDocumentPending));

        var service = new EmployeeDocumentService(_unitOfWorkMock.Object, _employeeServiceMock.Object, _errorLoggingServiceMock.Object);

        var result = await service.DeleteEmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentPending.Id);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetEmployeeDocumentsByStatusPendingPass()
    {
        var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMock();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet);

        var employeeDocuments = new List<EmployeeDocument>
        {
            new EmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentPending),
            new EmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentPending)
        };

        var mockEmployeeDocumentDbSet = employeeDocuments.AsQueryable().BuildMock();
        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                      .Returns(mockEmployeeDocumentDbSet);

        var result = await _employeeDocumentService.GetEmployeeDocumentsByStatus(employeeId, DocumentStatus.PendingApproval);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetEmployeeDocumentsByStatusApprovedPass()
    {
        var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMock();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet);

        var employeeDocuments = new List<EmployeeDocument>
        {
            new EmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentApproved),
            new EmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentApproved)
        };

        var mockEmployeeDocumentDbSet = employeeDocuments.AsQueryable().BuildMock();
        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                      .Returns(mockEmployeeDocumentDbSet);

        var result = await _employeeDocumentService.GetEmployeeDocumentsByStatus(employeeId, DocumentStatus.Approved);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetEmployeeDocumentsByStatusRejectedPass()
    {
        var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMock();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet);

        var employeeDocuments = new List<EmployeeDocument>
        {
            new EmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentRejected),
            new EmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentRejected)
        };

        var mockEmployeeDocumentDbSet = employeeDocuments.AsQueryable().BuildMock();
        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                      .Returns(mockEmployeeDocumentDbSet);

        var result = await _employeeDocumentService.GetEmployeeDocumentsByStatus(employeeId, DocumentStatus.Rejected);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task CheckEmployeeFail()
    {
        var mockEmployeeDbSet = new List<Employee>();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                       .Returns(mockEmployeeDbSet.AsQueryable().BuildMock());

        var result = await _employeeDocumentService.CheckEmployee(employeeId);
        Assert.False(result);
        
    }

    [Fact]
    public async Task GetAllEmployeeDocumentsWithEmployee()
     {
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
}