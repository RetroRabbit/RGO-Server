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

namespace HRIS.Services.Tests.Services;

public class EmployeeDocumentServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEmployeeService> _employeeServiceMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly Mock<IEmployeeDocumentService> _employeeDocumentServiceMock;

    private readonly EmployeeDocumentService _employeeDocumentService;

    public EmployeeDocumentServiceUnitTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _employeeServiceMock = new Mock<IEmployeeService>();
        _employeeDocumentServiceMock = new Mock<IEmployeeDocumentService>();
        _employeeDocumentService = new EmployeeDocumentService(_unitOfWorkMock.Object, _employeeServiceMock.Object);
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
    }

    private const int EmployeeId = 1;
    
    private List<EmployeeDocument> GetEmployeeDocumentsByStatus(DocumentStatus status)
    {
        var documents = new List<EmployeeDocument>();

        for (var i = 0; i < 2; i++)
            documents.Add(new EmployeeDocument { Status = status });

        return documents;
    }

    private void SetupMockRoles()
    {
        var roles = EmployeeRoleTestData.RoleDtoEmployee.EntityToList();

        _employeeServiceMock.Setup(x => x.GetById(EmployeeId))
            .ReturnsAsync(EmployeeTestData.EmployeeOne.ToDto);

        _unitOfWorkMock
            .Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
            .Returns(EmployeeRoleTestData.EmployeeRolesList.ToMockIQueryable());

        _unitOfWorkMock
            .Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roles.ToMockIQueryable());

        _unitOfWorkMock.Setup(x => x.EmployeeDocument.Add(It.IsAny<EmployeeDocument>()))
            .ReturnsAsync(EmployeeDocumentTestData.EmployeeDocumentPending);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task SaveDocumentPass(int documentType)
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name!))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _unitOfWorkMock
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(EmployeeTestData.EmployeeOne.ToMockIQueryable());

        _unitOfWorkMock
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(EmployeeBankingTestData.EmployeeBankingOne.ToMockIQueryable());

        SetupMockRoles();

        var result = await _employeeDocumentService.SaveEmployeeDocument(EmployeeDocumentTestData.SimpleDocumentDto, "test@retrorabbit.co.za", documentType);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeDocumentTestData.EmployeeDocumentPending.ToDto(), result);
        _employeeServiceMock.Verify(x => x.GetById(EmployeeId), Times.Once);
        _unitOfWorkMock.Verify(x => x.EmployeeDocument.Add(It.IsAny<EmployeeDocument>()), Times.Once);
    }

    [Fact]
    public async Task AddNewAdditionalDocumentPass()
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name!))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _unitOfWorkMock
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(EmployeeTestData.EmployeeOne.ToMockIQueryable());

        _unitOfWorkMock
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(EmployeeBankingTestData.EmployeeBankingOne.ToMockIQueryable());

        SetupMockRoles();

        var result = await _employeeDocumentService.addNewAdditionalDocument(EmployeeDocumentTestData.SimpleDocumentDto, "test@retrorabbit.co.za", 1);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeDocumentTestData.EmployeeDocumentPending.ToDto(), result);
        _employeeServiceMock.Verify(x => x.GetById(EmployeeId), Times.Once);
        _unitOfWorkMock.Verify(x => x.EmployeeDocument.Add(It.IsAny<EmployeeDocument>()), Times.Once);
    }

    [Fact]
    public async Task AddNewAdditionalDocumentFail()
    {
        var employeeDocDto = EmployeeDocumentTestData.SimpleDocumentDto;

        _employeeServiceMock.Setup(x => x.GetById(employeeDocDto.EmployeeId))
            .ReturnsAsync((EmployeeDto)null!);

         await Assert.ThrowsAsync<CustomException>(() => _employeeDocumentService
            .addNewAdditionalDocument(employeeDocDto, "test@retrorabbit.co.za", 1));
    }

    [Fact]
    public async Task SaveEmployeeDocumentFail()
    {
        _employeeServiceMock.Setup(x => x.GetById(EmployeeId))
                            .ReturnsAsync((EmployeeDto?)null);
        
        await Assert.ThrowsAsync<CustomException>(() => _employeeDocumentService
            .SaveEmployeeDocument(EmployeeDocumentTestData.SimpleDocumentDto, "test@retrorabbit.co.za", 1));

        _employeeServiceMock.Verify(x => x.GetById(EmployeeId), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeDocumentPass()
    {
        var fileName = "TestFile.pdf";

        var mockEmployeeDbSet = EmployeeTestData.EmployeeOne.EntityToList().AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet.Object);

        var employeeDocument = new EmployeeDocument { EmployeeId = EmployeeId, FileName = fileName };
        var mockEmployeeDocumentDbSet = new List<EmployeeDocument> { employeeDocument }.AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                      .Returns(mockEmployeeDocumentDbSet.Object);

        var service = new EmployeeDocumentService(_unitOfWorkMock.Object, _employeeServiceMock.Object);

        var result = await service.GetEmployeeDocument(EmployeeId, fileName, DocumentType.StarterKit);

        Assert.NotNull(result);
        Assert.Equal(fileName, result.FileName);
    }

    [Fact]
    public async Task GetEmployeeDocumentEmployeeNotFoundFail()
    {
        var employeeId = 1;
        var filename = "sample_certification.pdf";
        var documentType = DocumentType.StarterKit;

        _employeeServiceMock.Setup(x => x.GetById(employeeId))
                        .ReturnsAsync((EmployeeDto)null!);

        _employeeDocumentServiceMock.Setup(x => x.CheckEmployee(employeeId)).ReturnsAsync(false);

        _unitOfWorkMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(Enumerable.Empty<Employee>().ToMockIQueryable());

        _unitOfWorkMock.Setup(x => x.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                       .Returns(Enumerable.Empty<EmployeeDocument>().ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() =>
                        _employeeDocumentService.GetEmployeeDocument(employeeId, filename, documentType));
    }

    [Fact]
    public async Task GetEmployeeDocumentRecordNotFoundFail()
    {
        var employeeId = 1;
        var filename = "sample_certification.pdf";
        var documentType = DocumentType.StarterKit;

        var mockEmployeeDbSet = EmployeeTestData.EmployeeOne.EntityToList().AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet.Object);

        _employeeDocumentServiceMock.Setup(x => x.CheckEmployee(employeeId)).ReturnsAsync(true);


        _unitOfWorkMock.Setup(x => x.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
            .Returns(new List<EmployeeDocument>().ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() =>
                    _employeeDocumentService.GetEmployeeDocument(employeeId, filename, documentType));
    }

    [Fact]
    public async Task GetAllEmployeeDocumentsPass()
    {
        var mockEmployeeDbSet = EmployeeTestData.EmployeeOne.EntityToList().AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet.Object);

        var employeeDocuments = new List<EmployeeDocument>
        {
            new() { EmployeeId = EmployeeId, FileName = "TestFile1.pdf", DocumentType = DocumentType.StarterKit },
            new() { EmployeeId = EmployeeId, FileName = "TestFile2.pdf", DocumentType = DocumentType.StarterKit }
        };

        var mockEmployeeDocumentDbSet = employeeDocuments.AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                      .Returns(mockEmployeeDocumentDbSet.Object);

        var service = new EmployeeDocumentService(_unitOfWorkMock.Object, _employeeServiceMock.Object);

        var result = await service.GetEmployeeDocuments(EmployeeId, DocumentType.StarterKit);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, doc => doc.FileName == "TestFile1.pdf");
        Assert.Contains(result, doc => doc.FileName == "TestFile2.pdf");
    }

    [Fact]
    public async Task GetAllEmployeeDocumentsFail()
    {
        var employeeId = 1;
        var documentType = DocumentType.StarterKit;

        _employeeServiceMock.Setup(x => x.GetById(employeeId))
                        .ReturnsAsync((EmployeeDto)null!);

        _employeeDocumentServiceMock.Setup(x => x.CheckEmployee(employeeId)).ReturnsAsync(false);

        _unitOfWorkMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(Enumerable.Empty<Employee>().ToMockIQueryable());

        _unitOfWorkMock.Setup(x => x.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                       .Returns(Enumerable.Empty<EmployeeDocument>().ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() =>
                        _employeeDocumentService.GetEmployeeDocuments(employeeId, documentType));
    }

    [Fact]
    public async Task UpdateEmployeeDocumentPass()
    {
        var mockEmployeeDbSet = EmployeeTestData.EmployeeOne.EntityToList().AsQueryable().BuildMockDbSet();

        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                  .Returns(mockEmployeeDbSet.Object);

        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Update(It.IsAny<EmployeeDocument>()))
                      .ReturnsAsync(EmployeeDocumentTestData.EmployeeDocumentPending);

        SetupMockRoles();
        var service = new EmployeeDocumentService(_unitOfWorkMock.Object, _employeeServiceMock.Object);

        var result = await service.UpdateEmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentPending.ToDto(), "test@retrorabbit.co.za");

        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateEmployeeDocumentFail()
    {
        var employeeId = 1;

        _employeeServiceMock.Setup(x => x.GetById(employeeId))
                        .ReturnsAsync((EmployeeDto)null!);

        _employeeDocumentServiceMock.Setup(x => x.CheckEmployee(employeeId)).ReturnsAsync(false);

        _unitOfWorkMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(Enumerable.Empty<Employee>().ToMockIQueryable());

        _unitOfWorkMock.Setup(x => x.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                       .Returns(Enumerable.Empty<EmployeeDocument>().ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() =>
                        _employeeDocumentService.UpdateEmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentApproved.ToDto(), "test@retrorabbit.co.za"));
    }

    [Fact]
    public async Task DeleteEmployeeDocumentPass()
    {
        var mockEmployeeDbSet = EmployeeTestData.EmployeeOne.EntityToList().AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployeeDbSet.Object);

        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Delete(It.IsAny<int>()))
                      .ReturnsAsync(EmployeeDocumentTestData.EmployeeDocumentPending);

        var service = new EmployeeDocumentService(_unitOfWorkMock.Object, _employeeServiceMock.Object);

        var result = await service.DeleteEmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentPending.Id);

        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(DocumentStatus.PendingApproval)]
    [InlineData(DocumentStatus.Approved)]
    [InlineData(DocumentStatus.Rejected)]
    public async Task GetEmployeeDocumentsByStatusPass(DocumentStatus status)
    {
        var mockEmployeeDbSet = EmployeeTestData.EmployeeOne.ToMockIQueryable();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                       .Returns(mockEmployeeDbSet);

        var mockEmployeeDocumentDbSet = GetEmployeeDocumentsByStatus(status).ToMockIQueryable();

        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                       .Returns(mockEmployeeDocumentDbSet);

        var result = await _employeeDocumentService.GetEmployeeDocumentsByStatus(EmployeeId, status);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetEmployeeDocumentsByStatusFail()
    {
        var employeeId = 1;

        _employeeServiceMock.Setup(x => x.GetById(employeeId))
                        .ReturnsAsync((EmployeeDto)null!);

        _employeeDocumentServiceMock.Setup(x => x.CheckEmployee(employeeId)).ReturnsAsync(false);

        _unitOfWorkMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(Enumerable.Empty<Employee>().ToMockIQueryable());

        _unitOfWorkMock.Setup(x => x.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                       .Returns(Enumerable.Empty<EmployeeDocument>().ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() =>
                        _employeeDocumentService.GetEmployeeDocumentsByStatus(employeeId, DocumentStatus.Approved));
    }

    [Fact]
    public async Task CheckEmployeeFail()
    {
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                       .Returns(new List<Employee>().ToMockIQueryable());

        var result = await _employeeDocumentService.CheckEmployee(EmployeeId);
        Assert.False(result);

    }

    [Fact]
    public async Task GetAllEmployeeDocumentsWithEmployee()
    {
        _unitOfWorkMock
            .Setup(x =>
                       x.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
            .Returns(new List<EmployeeDocument> {
                EmployeeDocumentTestData.EmployeeDocumentApproved,
                EmployeeDocumentTestData.EmployeeDocumentApproved
            }
            .ToMockIQueryable());

        var employeeDocumentGetAllList = new List<SimpleEmployeeDocumentGetAllDto>
        {
            new() { EmployeeDocumentDto = EmployeeDocumentTestData.EmployeeDocumentApproved.ToDto(), Name = "Pieter", Surname = "Pietersen" },
            new() { EmployeeDocumentDto = EmployeeDocumentTestData.EmployeeDocumentApproved.ToDto(), Name = "Pieter1", Surname = "Pietersen2"}
        };

        _employeeDocumentServiceMock.Setup(x => x.GetAllDocuments()).ReturnsAsync(employeeDocumentGetAllList);

        var result = await _employeeDocumentService.GetAllDocuments();

        Assert.NotNull(result);
        Assert.IsType<List<SimpleEmployeeDocumentGetAllDto>>(result);
    }
}