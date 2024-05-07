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

    private List<EmployeeDocument> GetEmployeeDocumentsByStatus(DocumentStatus status)
    {
        var documents = new List<EmployeeDocument>();

        for (int i = 0; i < 2; i++)
        {
            documents.Add(new EmployeeDocument()
            {
                Status = status
            });
        }

        return documents;
    }

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
    public async Task addNewAdditionalDocumentPass()
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

        var result = await _employeeDocumentService.addNewAdditionalDocument(EmployeeDocumentTestData.SimpleDocumentDto, "test@retrorabbit.co.za", 1);

        Assert.NotNull(result);
        Assert.Equal(EmployeeDocumentTestData.EmployeeDocumentPending, result);
        _employeeServiceMock.Verify(x => x.GetById(employeeId), Times.Once);
        _unitOfWorkMock.Verify(x => x.EmployeeDocument.Add(It.IsAny<EmployeeDocument>()), Times.Once);
    }

    [Fact]
    public async Task addNewAdditionalDocumentFail()
    {
        var employeeDocDto = EmployeeDocumentTestData.SimpleDocumentDto;
        var email = "test@retrorabbit.co.za";
        var documentType = 1;

        _employeeServiceMock.Setup(x => x.GetById(employeeDocDto.EmployeeId))
            .ReturnsAsync((EmployeeDto)null);

        _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
            .Throws(new Exception("employee not found"));

        var exception = await Assert.ThrowsAsync<Exception>(() => _employeeDocumentService
            .addNewAdditionalDocument(employeeDocDto, email, documentType));

        Assert.Equal("employee not found", exception.Message);

        _errorLoggingServiceMock.Verify(x => x.LogException(It.IsAny<Exception>()), Times.Once);
    }


    //[Theory]
    //[InlineData(true, true, 0, DocumentStatus.PendingApproval, DocumentType.starterKit, "test@retrorabbit.co.za", 1)]
    //[InlineData(true, false, 1, DocumentStatus.PendingApproval, DocumentType.additional, "test@retrorabbit.co.za", 1)]
    //[InlineData(false, true, 0, DocumentStatus.ActionRequired, DocumentType.starterKit, "test1@retrorabbit.co.za", 0)]
    //[InlineData(false, false, 1, DocumentStatus.PendingApproval, DocumentType.additional, "test1@retrorabbit.co.za", 1)]
    //public async Task addNewAdditionalDocumentSetsStatusAndDocType(
    //bool emailMatches, bool isAdmin, int documentType,
    //DocumentStatus expectedStatus, DocumentType expectedDocType, String email, int docStatus)
    //{
    //    var employeeDocDto = EmployeeDocumentTestData.SimpleDocumentDto;

    //    _employeeTypeServiceMock
    //       .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
    //       .ReturnsAsync(EmployeeTypeTestData.DeveloperType);

    //    _unitOfWorkMock
    //        .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
    //        .Returns(
    //            new List<Employee>
    //            {
    //                new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
    //                {
    //                    Email = email,
    //                    EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType),
    //                    PhysicalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto),
    //                    PostalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto)
    //                }
    //            }.AsQueryable().BuildMock());

    //    _unitOfWorkMock
    //        .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
    //        .Returns(
    //            new List<EmployeeBanking>
    //            {
    //                new(EmployeeBankingTestData.EmployeeBankingDto)
    //            }.AsQueryable().BuildMock());



    //    List<Role> roles = new List<Role> { new Role(EmployeeRoleTestData.RoleDtoEmployee) };
    //    _employeeServiceMock.Setup(x => x.GetById(employeeId))
    //        .ReturnsAsync(EmployeeTestData.EmployeeDto);

    //    _unitOfWorkMock
    //        .Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
    //        .Returns(EmployeeRoleTestData.EmployeeRolesList.AsQueryable().BuildMock());

    //    _unitOfWorkMock
    //        .Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
    //        .Returns(roles.AsQueryable().BuildMock());

    //    EmployeeDocumentDto employeeDocument = null;
    //    if (docStatus == 0)
    //    {
    //        employeeDocument = EmployeeDocumentTestData.EmployeeDocumentActionRequired;
    //    }
    //    else
    //    {
    //        employeeDocument = EmployeeDocumentTestData.EmployeeDocumentPending;
    //    }


    //    _unitOfWorkMock.Setup(x => x.EmployeeDocument.Add(It.IsAny<EmployeeDocument>()))
    //        .ReturnsAsync(employeeDocument);

    //    _employeeServiceMock.Setup(x => x.GetEmployee(email)).Returns(Task.FromResult(EmployeeTestData.EmployeeDto));

    //    _employeeDocumentServiceMock.Setup(x => x.IsAdmin(email)).ReturnsAsync(isAdmin);

    //    var result = await _employeeDocumentService.addNewAdditionalDocument(employeeDocDto, email, documentType);

    //    Assert.Equal(expectedStatus, result.Status);
    //    Assert.Equal(expectedDocType, result.DocumentType);
    //}

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

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>()))
            .Throws(new Exception("employee not found"));

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
    public async Task GetEmployeeDocumentEmployeeNotFoundFail()
    {
        var employeeId = 1;
        var filename = "sample_certification.pdf";
        var documentType = DocumentType.starterKit;

        _employeeServiceMock.Setup(x => x.GetById(employeeId))
                        .ReturnsAsync((EmployeeDto)null);

        _employeeDocumentServiceMock.Setup(x => x.CheckEmployee(employeeId)).ReturnsAsync(false);

        _unitOfWorkMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func <Employee, bool>>>()))
            .Returns(Enumerable.Empty<Employee>().AsQueryable().BuildMock());

        _unitOfWorkMock.Setup(x => x.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                       .Returns(Enumerable.Empty<EmployeeDocument>().AsQueryable().BuildMock());

        _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>())).Throws(new Exception("Employee not found")); 

        var exception = await Assert.ThrowsAsync<Exception>(() =>
                        _employeeDocumentService.GetEmployeeDocument(employeeId, filename, documentType));

        Assert.Equal("Employee not found", exception.Message);

        _errorLoggingServiceMock.Verify(x => x.LogException(It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeDocumentRecordNotFoundFail()
    {
        var employeeId = 1;
        var filename = "sample_certification.pdf";
        var documentType = DocumentType.starterKit;

        var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet.Object);

        _employeeDocumentServiceMock.Setup(x => x.CheckEmployee(employeeId)).ReturnsAsync(true);


        _unitOfWorkMock.Setup(x => x.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
            .Returns(new List<EmployeeDocument>().AsQueryable().BuildMock());

        _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
            .Throws(new Exception("Employee certification record not found"));

        var exception = await Assert.ThrowsAsync<Exception>(() =>
                    _employeeDocumentService.GetEmployeeDocument(employeeId, filename, documentType));

        Assert.Equal("Employee certification record not found", exception.Message);
        _errorLoggingServiceMock.Verify(x => x.LogException(It.IsAny<Exception>()), Times.Once);
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
    public async Task GetAllEmployeeDocumentsFail()
    {
        var employeeId = 1;
        var documentType = DocumentType.starterKit;

        _employeeServiceMock.Setup(x => x.GetById(employeeId))
                        .ReturnsAsync((EmployeeDto)null);

        _employeeDocumentServiceMock.Setup(x => x.CheckEmployee(employeeId)).ReturnsAsync(false);

        _unitOfWorkMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(Enumerable.Empty<Employee>().AsQueryable().BuildMock());

        _unitOfWorkMock.Setup(x => x.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                       .Returns(Enumerable.Empty<EmployeeDocument>().AsQueryable().BuildMock());

        _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>())).Throws(new Exception("Employee not found"));

        var exception = await Assert.ThrowsAsync<Exception>(() =>
                        _employeeDocumentService.GetAllEmployeeDocuments(employeeId, documentType));

        Assert.Equal("Employee not found", exception.Message);

        _errorLoggingServiceMock.Verify(x => x.LogException(It.IsAny<Exception>()), Times.Once);
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
    public async Task UpdateEmployeeDocumentFail()
    {
        var employeeId = 1;

        _employeeServiceMock.Setup(x => x.GetById(employeeId))
                        .ReturnsAsync((EmployeeDto)null);

        _employeeDocumentServiceMock.Setup(x => x.CheckEmployee(employeeId)).ReturnsAsync(false);

        _unitOfWorkMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(Enumerable.Empty<Employee>().AsQueryable().BuildMock());

        _unitOfWorkMock.Setup(x => x.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                       .Returns(Enumerable.Empty<EmployeeDocument>().AsQueryable().BuildMock());

        _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>())).Throws(new Exception("Employee not found"));

        var exception = await Assert.ThrowsAsync<Exception>(() =>
                        _employeeDocumentService.UpdateEmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentApproved));

        Assert.Equal("Employee not found", exception.Message);

        _errorLoggingServiceMock.Verify(x => x.LogException(It.IsAny<Exception>()), Times.Once);
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

    [Theory]
    [InlineData(DocumentStatus.PendingApproval)]
    [InlineData(DocumentStatus.Approved)]
    [InlineData(DocumentStatus.Rejected)]
    public async Task GetEmployeeDocumentsByStatusPass(DocumentStatus status)
    {
        var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMock();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                       .Returns(mockEmployeeDbSet);

        var employeeDocuments = GetEmployeeDocumentsByStatus(status);
        var mockEmployeeDocumentDbSet = employeeDocuments.AsQueryable().BuildMock();

        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                       .Returns(mockEmployeeDocumentDbSet);

        var result = await _employeeDocumentService.GetEmployeeDocumentsByStatus(employeeId, status);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetEmployeeDocumentsByStatusFail()
    {
        var employeeId = 1;

        _employeeServiceMock.Setup(x => x.GetById(employeeId))
                        .ReturnsAsync((EmployeeDto)null);

        _employeeDocumentServiceMock.Setup(x => x.CheckEmployee(employeeId)).ReturnsAsync(false);

        _unitOfWorkMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(Enumerable.Empty<Employee>().AsQueryable().BuildMock());

        _unitOfWorkMock.Setup(x => x.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                       .Returns(Enumerable.Empty<EmployeeDocument>().AsQueryable().BuildMock());

        _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>())).Throws(new Exception("Employee not found"));

        var exception = await Assert.ThrowsAsync<Exception>(() =>
                        _employeeDocumentService.GetEmployeeDocumentsByStatus(employeeId,DocumentStatus.Approved));

        Assert.Equal("Employee not found", exception.Message);

        _errorLoggingServiceMock.Verify(x => x.LogException(It.IsAny<Exception>()), Times.Once);
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