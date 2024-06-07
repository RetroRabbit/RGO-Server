using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.Tests.Data.Models;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork.Entities.HRIS;
using System.Reflection.Metadata;
using System.Security.Claims;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeDocumentControllerUnitTest
{
    private readonly Mock<IEmployeeDocumentService> _employeeMockDocumentService;
    //private readonly EmployeeDocumentDto _employeeDocument;
    private readonly SimpleEmployeeDocumentDto _simpleEmployeeDocument;
    private readonly EmployeeDocumentController _controller;

    List<Claim> claims;
    ClaimsPrincipal claimsPrincipal;
    ClaimsIdentity identity;

    public EmployeeDocumentControllerUnitTest()
    {
        _employeeMockDocumentService = new Mock<IEmployeeDocumentService>();
        _controller = new EmployeeDocumentController(_employeeMockDocumentService.Object);

        claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "test@example.com"),
            };

        identity = new ClaimsIdentity(claims, "TestAuthType");
        claimsPrincipal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
      
        _simpleEmployeeDocument = new SimpleEmployeeDocumentDto
        {
            Id = 1,
            EmployeeId = EmployeeTestData.EmployeeDto.Id,
            FileName = "TestFile.pdf",
            FileCategory = FileCategory.FixedTerm,
            Blob = "TestFileContent",
            UploadDate = DateTime.Now
        };
    }

    [Fact]
    public async Task GetEmployeeDocumentReturnsOkfoundResult()
    {
        _employeeMockDocumentService
            .Setup(x => x.GetEmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentPending.Id, EmployeeDocumentTestData.EmployeeDocumentPending.FileName!, EmployeeDocumentTestData.EmployeeDocumentPending.DocumentType!.Value))
            .ReturnsAsync(EmployeeDocumentTestData.EmployeeDocumentPending);

        var result = await _controller.GetEmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentPending.Id, EmployeeDocumentTestData.EmployeeDocumentPending.FileName!, 0);

        Assert.NotNull(result);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDetails = Assert.IsType<EmployeeDocumentDto>(okResult.Value);

        Assert.Equal(EmployeeDocumentTestData.EmployeeDocumentPending, actualDetails);
    }

    [Fact]
    public async Task GetEmployeeDocumentReturnsExceptionNotfoundResult()
    {
        var id = 15;
        var filename = "";
        var err = "An error occurred while fetching the employee document.";

        _employeeMockDocumentService.Setup(x => x.GetEmployeeDocument(id, filename, DocumentType.StarterKit)).ThrowsAsync(new Exception(err));

        var result = await _controller.GetEmployeeDocument(id, filename, 0);
        var notfoundResult = Assert.IsType<ObjectResult>(result);
        var actualExceptionMessage = Assert.IsType<string>(notfoundResult.Value);

        Assert.Equal("An error occurred while fetching the employee document.", actualExceptionMessage);

    }

    [Fact]
    public async Task GetAllEmployeeDocumentReturnsOkResult()
    {
        var listOfEmployeeDocumentsDto = new List<EmployeeDocumentDto>()
            {
                EmployeeDocumentTestData.EmployeeDocumentPending
            };

        var listOfEmployeeDocuments = new List<EmployeeDocument>()
            {
                new EmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentPending)
            };

        _employeeMockDocumentService.Setup(x => x.GetEmployeeDocuments(EmployeeDocumentTestData.EmployeeDocumentPending.Id, EmployeeDocumentTestData.EmployeeDocumentPending.DocumentType!.Value)).ReturnsAsync(listOfEmployeeDocumentsDto);

        var result = await _controller.GetEmployeeDocuments(EmployeeDocumentTestData.EmployeeDocumentPending.Id,0);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDetails = Assert.IsAssignableFrom<List<EmployeeDocumentDto>>(okResult.Value);

        Assert.NotNull(result);
        Assert.Equal(listOfEmployeeDocumentsDto, actualDetails);

    }

    [Fact]
    public async Task GetAllReturnsNotFoundResultWhenExceptionThrown()
    {
        var id = 15;
        var exceptionMessage = "An error occurred while fetching employee documents.";

        _employeeMockDocumentService.Setup(x => x.GetEmployeeDocuments(id, DocumentType.StarterKit)).ThrowsAsync(new Exception(exceptionMessage));

        var result = await _controller.GetEmployeeDocuments(id,0);

        var noFoundResult = Assert.IsType<ObjectResult>(result);
        var actualExceptionMessage = Assert.IsType<string>(noFoundResult.Value);

        Assert.Equal(exceptionMessage, actualExceptionMessage);
    }

    [Fact]
    public async Task SaveEmployeeDocumentReturnsOkResult()
    {

        _employeeMockDocumentService
            .Setup(c => c.SaveEmployeeDocument(_simpleEmployeeDocument, "test@example.com", 0))
            .ReturnsAsync(EmployeeDocumentTestData.EmployeeDocumentPending);

        var result = await _controller.Save(_simpleEmployeeDocument!, 0);
        var okresult = Assert.IsType<OkObjectResult>(result);
        var actualSavedEmployeeDocument = Assert.IsType<EmployeeDocumentDto>(okresult.Value);

        Assert.Equal(EmployeeDocumentTestData.EmployeeDocumentPending, actualSavedEmployeeDocument);

    }

    [Fact]
    public async Task SaveEmployeeDocumentThrowsExceptionReturnsNotFoundResult()
    {
        _employeeMockDocumentService
            .Setup(x => x.SaveEmployeeDocument(It.IsAny<SimpleEmployeeDocumentDto>(), "test@example.com", 0))
            .Throws(new Exception("An error occurred while saving the employee document."));

        var result = await _controller.Save(_simpleEmployeeDocument!, 0);
        var notfoundResult = Assert.IsType<ObjectResult>(result);
        var exceptionMessage = Assert.IsType<string>(notfoundResult.Value);

        Assert.Equal("An error occurred while saving the employee document.", exceptionMessage);
    }

    [Fact]
    public async Task UpdateEmployeeDocumentReturnsOkResult()
    {
        _employeeMockDocumentService.Setup(x => x.UpdateEmployeeDocument(It.IsAny<EmployeeDocumentDto>(), ""))
            .ReturnsAsync(EmployeeDocumentTestData.EmployeeDocumentPending);

        var result = await _controller.Update(EmployeeDocumentTestData.EmployeeDocumentPending);
        var okresult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, okresult.StatusCode);
    }

    [Fact]
    public async Task UpdateEmployeeDocumentReturnsNotFoundResultWhenExceptionThrown()
    {

        EmployeeDocumentDto broken = new EmployeeDocumentDto
        {
            Id = 1,
            DocumentType = DocumentType.StarterKit,
            LastUpdatedDate = DateTime.UtcNow,
            UploadDate = DateTime.UtcNow,
            AdminFileCategory = 0,
            CounterSign = false,
            EmployeeFileCategory = 0,
            FileName = "Test",
            EmployeeId = 0,
            Blob = null,
            FileCategory = 0,
            Reason = "",
            Reference = "",
            Status = DocumentStatus.Approved
        };

        _employeeMockDocumentService.Setup(service => service.UpdateEmployeeDocument(broken, It.IsAny<string>()))
                             .ThrowsAsync(new Exception("Employee exists"));
        var result = await _controller.Update(broken);

        var problemDetails = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, problemDetails.StatusCode);
        Assert.Equal("An error occurred while updating the employee document.", problemDetails.Value);
    }

    [Fact]
    public async Task DeleteEmployeeDocumentsReturnsOkResult()
    {
        _employeeMockDocumentService
            .Setup(e => e.DeleteEmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentPending.Id))
            .ReturnsAsync(EmployeeDocumentTestData.EmployeeDocumentPending);

        var result = await _controller.Delete(EmployeeDocumentTestData.EmployeeDocumentPending.Id);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualemployeeDocument = Assert.IsAssignableFrom<EmployeeDocumentDto>(okResult.Value);

        Assert.Equal(EmployeeDocumentTestData.EmployeeDocumentPending, actualemployeeDocument);

    }

    [Fact]
    public async Task DeleteEmployeeDocumentReturnsNotFoundResultWhenExceptionThrown()
    {
        var exceptionMessage = "An error occurred while deleting the employee document.";

        _employeeMockDocumentService.Setup(e => e.DeleteEmployeeDocument(EmployeeDocumentTestData.EmployeeDocumentPending.Id)).ThrowsAsync(new Exception(exceptionMessage));

        var result = await _controller.Delete(EmployeeDocumentTestData.EmployeeDocumentPending.Id);
        var notFoundResult = Assert.IsType<ObjectResult>(result);

        Assert.Equal(exceptionMessage, notFoundResult.Value);

    }

    [Fact]
    public async Task GetEmployeeDocumentByStatusReturnsOkResult()
    {
        //var status = DocumentStatus.Rejected;

        var listOfEmployeeDocumentsDto = new List<EmployeeDocumentDto>()
            {
                EmployeeDocumentTestData.EmployeeDocumentPending
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
    public async Task GetEmployeeDocumentByStatusReturnsNotFoundResultWhenExceptionThrown()
    {
        var id = 15;
        var documentStatus = (DocumentStatus)(-1);
        var exceptionMessage = "An error occurred while fetching the employee documents.";

        _employeeMockDocumentService.Setup(x => x.GetEmployeeDocumentsByStatus(id, documentStatus))
            .ThrowsAsync(new Exception(exceptionMessage));

        var result = await _controller.GetEmployeeDocumentsByStatus(id, documentStatus);
        var notfoundResult = Assert.IsType<ObjectResult>(result);

        Assert.Equal("An error occurred while fetching the employee documents.", notfoundResult.Value);

    }

    [Fact]
    public async Task GetAllDocuments_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        _employeeMockDocumentService
                .Setup(ex => ex.GetAllDocuments())
                .ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.GetAllDocuments();

        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("An error occurred while fetching the employee documents.", statusCodeResult.Value);
    }

    [Fact]
    public async Task GetAllDocuments_ReturnsOk_WhenDocumentsAreFetchedSuccessfully()
    {
        var documents = new List<SimpleEmployeeDocumentGetAllDto> { };
        _employeeMockDocumentService
            .Setup(service => service.GetAllDocuments())
            .ReturnsAsync(documents);

        var result = await _controller.GetAllDocuments();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(documents, okResult.Value);
    }
}
