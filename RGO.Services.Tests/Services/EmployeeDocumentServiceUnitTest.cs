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

namespace HRIS.Services.Tests.Services;

public class EmployeeDocumentServiceUnitTest
{
    private static readonly EmployeeTypeDto employeeTypeDto = new(1, "Developer");

    private static readonly EmployeeAddressDto employeeAddressDto =
        new(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

    private static readonly EmployeeDto employeeMock = new(1, "001", "34434434", new DateTime(), new DateTime(),
                                                           null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128,
                                                           100000, "Matt", "MT",
                                                           "Schoeman", new DateTime(), "South Africa", "South African",
                                                           "0000080000000", " ",
                                                           new DateTime(), null, Race.Black, Gender.Male, null,
                                                           "test@retrorabbit.co.za", "test.example@gmail.com",
                                                           "0000000000", null, null, employeeAddressDto,
                                                           employeeAddressDto, null, null, null);

    private static readonly Employee testEmployee = new(employeeMock, employeeTypeDto);
    private readonly EmployeeDocumentService _employeeDocumentService;
    private readonly Mock<IEmployeeService> _employeeServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly int employeeId = 1;

    public EmployeeDocumentServiceUnitTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _employeeServiceMock = new Mock<IEmployeeService>();
        _employeeDocumentService = new EmployeeDocumentService(_unitOfWorkMock.Object, _employeeServiceMock.Object);
    }

    [Fact]
    public async Task SaveEmployeeDocumentPass()
    {
        var employeeDocDto = new SimpleEmployeeDocumentDto(
                                                           1,
                                                           employeeId,
                                                           "TestFile.pdf",
                                                           FileCategory.FixedTerm,
                                                           "TestFileContent",
                                                           DateTime.Now
                                                          );

        _employeeServiceMock.Setup(x => x.GetById(employeeId))
                            .ReturnsAsync(employeeMock);

        _unitOfWorkMock.Setup(x => x.EmployeeDocument.Add(It.IsAny<EmployeeDocument>()))
                       .ReturnsAsync(new EmployeeDocumentDto(
                                                             1,
                                                             employeeId,
                                                             null,
                                                             "TestFile.pdf",
                                                             FileCategory.FixedTerm,
                                                             "TestFileContent",
                                                             DocumentStatus.PendingApproval,
                                                             DateTime.Now,
                                                             null,
                                                             false
                                                            ));

        var result = await _employeeDocumentService.SaveEmployeeDocument(employeeDocDto);

        Assert.NotNull(result);
        _employeeServiceMock.Verify(x => x.GetById(employeeId), Times.Once);
        _unitOfWorkMock.Verify(x => x.EmployeeDocument.Add(It.IsAny<EmployeeDocument>()), Times.Once);
    }

    [Fact]
    public async Task SaveEmployeeDocumentFail()
    {
        _employeeServiceMock.Setup(x => x.GetById(employeeId))
                            .ReturnsAsync((EmployeeDto)null);

        var employeeDocDto = new SimpleEmployeeDocumentDto(
                                                           1,
                                                           employeeId,
                                                           "TestFile.pdf",
                                                           FileCategory.FixedTerm,
                                                           "TestFileContent",
                                                           DateTime.Now
                                                          );

        var exception =
            await Assert.ThrowsAsync<Exception>(() => _employeeDocumentService.SaveEmployeeDocument(employeeDocDto));
        Assert.Equal("employee not found", exception.Message);

        _employeeServiceMock.Verify(x => x.GetById(employeeId), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeDocumentPass()
    {
        var fileName = "TestFile.pdf";

        var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                       .Returns(mockEmployeeDbSet.Object);

        var employeeDocument = new EmployeeDocument { EmployeeId = employeeId, FileName = fileName };
        var mockEmployeeDocumentDbSet = new List<EmployeeDocument> { employeeDocument }.AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                       .Returns(mockEmployeeDocumentDbSet.Object);

        var service = new EmployeeDocumentService(_unitOfWorkMock.Object, _employeeServiceMock.Object);

        var result = await service.GetEmployeeDocument(employeeId, fileName);

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
            new() { EmployeeId = employeeId, FileName = "TestFile1.pdf" },
            new() { EmployeeId = employeeId, FileName = "TestFile2.pdf" }
        };

        var mockEmployeeDocumentDbSet = employeeDocuments.AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                       .Returns(mockEmployeeDocumentDbSet.Object);

        var service = new EmployeeDocumentService(_unitOfWorkMock.Object, _employeeServiceMock.Object);

        var result = await service.GetAllEmployeeDocuments(employeeId);

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

        var employeeDocumentDto = new EmployeeDocumentDto(
                                                          1,
                                                          employeeId,
                                                          null,
                                                          "e2.pdf",
                                                          FileCategory.Medical,
                                                          "sadfasdf",
                                                          null,
                                                          DateTime.Now,
                                                          null,
                                                          false
                                                         );

        var employeeDocument = new EmployeeDocument(employeeDocumentDto);

        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Update(It.IsAny<EmployeeDocument>()))
                       .ReturnsAsync(employeeDocumentDto);
        var service = new EmployeeDocumentService(_unitOfWorkMock.Object, _employeeServiceMock.Object);

        var result = await service.UpdateEmployeeDocument(employeeDocumentDto);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteEmployeeDocumentPass()
    {
        var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMockDbSet();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                       .Returns(mockEmployeeDbSet.Object);

        var employeeDocumentDto = new EmployeeDocumentDto(
                                                          1,
                                                          employeeId,
                                                          null,
                                                          "e2.pdf",
                                                          FileCategory.Medical,
                                                          "sadfasdf",
                                                          null,
                                                          DateTime.Now,
                                                          null,
                                                          false
                                                         );

        var employeeDocument = new EmployeeDocument(employeeDocumentDto);

        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Delete(It.IsAny<int>()))
                       .Returns(Task.FromResult(employeeDocumentDto));

        var service = new EmployeeDocumentService(_unitOfWorkMock.Object, _employeeServiceMock.Object);

        var result = await service.DeleteEmployeeDocument(employeeDocumentDto.Id);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetEmployeeDocumentsByStatusPendingPass()
    {
        var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMock();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                       .Returns(mockEmployeeDbSet);

        var testDocument = new EmployeeDocumentDto(1, employeeId, null, "TestFile.pdf", FileCategory.FixedTerm,
                                                   "TestFileContent", DocumentStatus.PendingApproval, DateTime.Now,
                                                   null, false);

        var testDocumentTwo = new EmployeeDocumentDto(2, employeeId, null, "TestFile2.pdf", FileCategory.FixedTerm,
                                                      "TestFileContent", DocumentStatus.PendingApproval, DateTime.Now,
                                                      null, false);

        var employeeDocuments = new List<EmployeeDocument>
        {
            new(testDocument),
            new(testDocumentTwo)
        };

        var mockEmployeeDocumentDbSet = employeeDocuments.AsQueryable().BuildMock();
        _unitOfWorkMock.Setup(m => m.EmployeeDocument.Get(It.IsAny<Expression<Func<EmployeeDocument, bool>>>()))
                       .Returns(mockEmployeeDocumentDbSet);

        var result =
            await _employeeDocumentService.GetEmployeeDocumentsByStatus(employeeId, DocumentStatus.PendingApproval);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetEmployeeDocumentsByStatusApprovedPass()
    {
        var mockEmployeeDbSet = new List<Employee> { testEmployee }.AsQueryable().BuildMock();
        _unitOfWorkMock.Setup(m => m.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                       .Returns(mockEmployeeDbSet);

        var testDocument = new EmployeeDocumentDto(1, employeeId, null, "TestFile.pdf", FileCategory.FixedTerm,
                                                   "TestFileContent", DocumentStatus.Approved, DateTime.Now, null,
                                                   false);

        var testDocumentTwo = new EmployeeDocumentDto(2, employeeId, null, "TestFile2.pdf", FileCategory.FixedTerm,
                                                      "TestFileContent", DocumentStatus.Approved, DateTime.Now, null,
                                                      false);

        var employeeDocuments = new List<EmployeeDocument>
        {
            new(testDocument),
            new(testDocumentTwo)
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

        var testDocument = new EmployeeDocumentDto(1, employeeId, null, "TestFile.pdf", FileCategory.FixedTerm,
                                                   "TestFileContent", DocumentStatus.Rejected, DateTime.Now, null,
                                                   false);

        var testDocumentTwo = new EmployeeDocumentDto(2, employeeId, null, "TestFile2.pdf", FileCategory.FixedTerm,
                                                      "TestFileContent", DocumentStatus.Rejected, DateTime.Now, null,
                                                      false);

        var employeeDocuments = new List<EmployeeDocument>
        {
            new(testDocument),
            new(testDocumentTwo)
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
        Assert.Equal(false, result);
        
    }
}