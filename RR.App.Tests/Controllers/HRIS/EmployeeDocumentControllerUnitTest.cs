using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.Tests.Data.Models;
using RR.App.Controllers.HRIS;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeDocumentControllerUnitTest
{
    private readonly Mock<IEmployeeDocumentService> _employeeMockDocumentService;
    private readonly AuthorizeIdentityMock _authorizeIdentityMock;
    private readonly SimpleEmployeeDocumentDto _simpleEmployeeDocument;
    private readonly EmployeeDocumentController _controller;
    private readonly List<EmployeeDocumentDto> _employeeDocumentDtoList;
    private readonly EmployeeDocumentDto _employeeDocumentDto;

    public EmployeeDocumentControllerUnitTest()
    {
        _employeeMockDocumentService = new Mock<IEmployeeDocumentService>();
        _authorizeIdentityMock = new AuthorizeIdentityMock();

        _controller = new EmployeeDocumentController(_authorizeIdentityMock, _employeeMockDocumentService.Object);
      
        _simpleEmployeeDocument = new SimpleEmployeeDocumentDto
        {
            Id = 1,
            EmployeeId = EmployeeTestData.EmployeeOne.Id,
            FileName = "TestFile.pdf",
            FileCategory = FileCategory.EmploymentContract,
            Blob = "TestFileContent",
            UploadDate = DateTime.Now
        };

        _employeeDocumentDtoList = new List<EmployeeDocumentDto>()
        {
            EmployeeDocumentTestData.EmployeeDocumentPending.ToDto()
        };

        _employeeDocumentDto = EmployeeDocumentTestData.EmployeeDocumentPending.ToDto();
    }

    [Fact]
    public async Task GetEmployeeDocumentReturnsOkfoundResult()
    {
        _employeeMockDocumentService
            .Setup(x => x.GetEmployeeDocument(_employeeDocumentDto.Id, _employeeDocumentDto.FileName!, _employeeDocumentDto.DocumentType!.Value))
            .ReturnsAsync(_employeeDocumentDto);

        var result = await _controller.GetEmployeeDocument(_employeeDocumentDto.Id, _employeeDocumentDto.FileName!, 0);

        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDetails = Assert.IsType<EmployeeDocumentDto>(okResult.Value);
        Assert.Equivalent(EmployeeDocumentTestData.EmployeeDocumentPending.ToDto(), actualDetails);
    }

    [Fact]
    public async Task GetAllEmployeeDocumentReturnsOkResult()
    {
        _employeeMockDocumentService.Setup(x => x.GetEmployeeDocuments(_employeeDocumentDto.Id, _employeeDocumentDto.DocumentType!.Value))
            .ReturnsAsync(_employeeDocumentDtoList);

        var result = await _controller.GetEmployeeDocuments(EmployeeDocumentTestData.EmployeeDocumentPending.Id,0);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDetails = Assert.IsAssignableFrom<List<EmployeeDocumentDto>>(okResult.Value);
        Assert.NotNull(result);
        Assert.Equal(_employeeDocumentDtoList, actualDetails);
    }

    [Fact]
    public async Task SaveEmployeeDocumentReturnsOkResult()
    {
        _employeeMockDocumentService.Setup(c => c.SaveEmployeeDocument(_simpleEmployeeDocument, _authorizeIdentityMock.Email, 0))
            .ReturnsAsync(EmployeeDocumentTestData.EmployeeDocumentPending.ToDto());

        var result = await _controller.Save(_simpleEmployeeDocument, 0);

        var okresult = Assert.IsType<OkObjectResult>(result);
        var actualSavedEmployeeDocument = Assert.IsType<EmployeeDocumentDto>(okresult.Value);
        Assert.Equivalent(EmployeeDocumentTestData.EmployeeDocumentPending.ToDto(), actualSavedEmployeeDocument);
    }

    [Fact]
    public async Task UpdateEmployeeDocumentReturnsOkResult()
    {
        _employeeMockDocumentService.Setup(x => x.UpdateEmployeeDocument(It.IsAny<EmployeeDocumentDto>(), ""))
            .ReturnsAsync(EmployeeDocumentTestData.EmployeeDocumentPending.ToDto());

        var result = await _controller.Update(EmployeeDocumentTestData.EmployeeDocumentPending.ToDto());

        var okresult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okresult.StatusCode);
    }


    [Fact]
    public async Task DeleteEmployeeDocumentsReturnsOkResult()
    {
        _employeeMockDocumentService
            .Setup(e => e.DeleteEmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentPending.Id))
            .ReturnsAsync(EmployeeDocumentTestData.EmployeeDocumentPending.ToDto());

        var result = await _controller.Delete(EmployeeDocumentTestData.EmployeeDocumentPending.Id);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualemployeeDocument = Assert.IsAssignableFrom<EmployeeDocumentDto>(okResult.Value);

        Assert.Equivalent(EmployeeDocumentTestData.EmployeeDocumentPending.ToDto(), actualemployeeDocument);
    }

    [Fact]
    public async Task GetEmployeeDocumentByStatusReturnsOkResult()
    {
        var listOfEmployeeDocumentsDto = new List<EmployeeDocumentDto>()
        {
                EmployeeDocumentTestData.EmployeeDocumentPending.ToDto()
        };

        _employeeMockDocumentService
            .Setup(x => x.GetEmployeeDocumentsByStatus(EmployeeDocumentTestData.EmployeeDocumentPending.Id, (DocumentStatus)EmployeeDocumentTestData.EmployeeDocumentPending.Status!))
            .ReturnsAsync(listOfEmployeeDocumentsDto);

        var result = await _controller
            .GetEmployeeDocumentsByStatus(EmployeeDocumentTestData.EmployeeDocumentPending.Id, (DocumentStatus)EmployeeDocumentTestData.EmployeeDocumentPending.Status!);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDetails = Assert.IsAssignableFrom<List<EmployeeDocumentDto>>(okResult.Value);
        Assert.NotNull(result);
        Assert.Equal(listOfEmployeeDocumentsDto, actualDetails);
    }

    [Fact]
    public async Task GetAllDocumentsReturnsOkResult()
    {
        var listOfEmployeeDocumentsGetAllDto = new List<SimpleEmployeeDocumentGetAllDto>()
        {
                EmployeeDocumentTestData.SimpleGetAllDto,
        }; 

        _employeeMockDocumentService
            .Setup(x => x.GetAllDocuments())
            .Returns(Task.FromResult(listOfEmployeeDocumentsGetAllDto));

        var result = await _controller
            .GetAllDocuments();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDetails = Assert.IsAssignableFrom<List<SimpleEmployeeDocumentGetAllDto>>(okResult.Value);
        Assert.NotNull(result);
        Assert.Equal(listOfEmployeeDocumentsGetAllDto, actualDetails);
    }
}