using System.Security.Claims;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeBankingControllerUnitTests
{
    private readonly List<Claim> _claimList;
    private readonly ClaimsPrincipal _claimsPrincipal;
    private readonly EmployeeBankingController _controller;
    private readonly ClaimsIdentity _claimsIdentity;

    private readonly Mock<IEmployeeBankingService> _employeeBankingServiceMock;

    private readonly SimpleEmployeeBankingDto _simpleEmployeeBankingDto;

    private readonly EmployeeBankingDto _employeeBankingDto;

    public EmployeeBankingControllerUnitTests()
    {
        _employeeBankingServiceMock = new Mock<IEmployeeBankingService>();
        _controller = new EmployeeBankingController(new AuthorizeIdentityMock(), _employeeBankingServiceMock.Object);

        _claimList = new List<Claim>
        {
            new(ClaimTypes.Email, "test@example.com")
        };

        _claimsIdentity = new ClaimsIdentity(_claimList, "TestAuthType");
        _claimsPrincipal = new ClaimsPrincipal(_claimsIdentity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = _claimsPrincipal }
        };

        _simpleEmployeeBankingDto = new SimpleEmployeeBankingDto
        {
            Id = 1,
            EmployeeId = 2,
            BankName = "BankName",
            Branch = "Branch",
            AccountNo = "AccountNo",
            AccountType = EmployeeBankingAccountType.Savings,
            Status = BankApprovalStatus.Approved,
            DeclineReason = "DeclineReason",
            File = "File.pdf"
        };

        _employeeBankingDto = EmployeeBankingTestData.EmployeeBankingOne.ToDto();
    }

    [Fact]
    public async Task AddBankingInfoValidInputReturnsOkResult()
    {
        var result = await _controller.AddBankingInfo(_simpleEmployeeBankingDto);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task AddBankingInfoExceptionWithDetailsAlreadyExistReturnsProblemResult()
    {
        _employeeBankingServiceMock.Setup(x => x.Save(It.IsAny<EmployeeBankingDto>(), "test@example.com"))
                   .ThrowsAsync(new Exception("Details already exists"));

        var result = await _controller.AddBankingInfo(_simpleEmployeeBankingDto);

        Assert.NotNull(result);
        Assert.IsType<ObjectResult>(result);

        var problemResult = (ObjectResult)result;

        Assert.NotNull(problemResult.Value);
        Assert.IsType<ProblemDetails>(problemResult.Value);

        var problemDetails = (ProblemDetails)problemResult.Value;

        Assert.Equal("Unexceptable", problemDetails.Detail);
        Assert.Equal(406, problemDetails.Status);
        Assert.Equal("Details already exists", problemDetails.Title);
    }

    [Fact]
    public async Task AddBankingInfoExceptionNotFound()
    {
        _employeeBankingServiceMock.Setup(x => x.Save(It.IsAny<EmployeeBankingDto>(), "test@example.com"))
                   .ThrowsAsync(new Exception("Banking information Not Found"));

        var result = await _controller.AddBankingInfo(_simpleEmployeeBankingDto);
        var notFoundResult = (NotFoundObjectResult)result;

        Assert.NotNull(result);
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.NotNull(notFoundResult.Value);
        Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Banking information Not Found", (string)notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteBankingInfoExceptionNotFound()
    {
        _employeeBankingServiceMock.Setup(s => s.Delete(_employeeBankingDto.Id))
                                  .ThrowsAsync(new Exception("Banking information Not Found"));

        var result = await _controller.DeleteBankingInfo(_employeeBankingDto.Id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Banking information Not Found", notFoundResult.Value);
    }

    [Fact]
    public async Task AddBankingInfoUnauthorizedAccess()
    {
        _employeeBankingServiceMock.Setup(x => x.Save(It.IsAny<EmployeeBankingDto>(), "test@example.com"))
                   .ThrowsAsync(new Exception("Unauthorized access"));

        var result = await _controller.AddBankingInfo(_simpleEmployeeBankingDto);
        var unauthorized = (ObjectResult)result;

        Assert.NotNull(result);
        Assert.Equal("Forbidden: Unauthorized access", unauthorized.Value);
    }

    [Fact]
    public async Task GetValidStatusReturnsOkResultWithEntries()
    {
        var expectedEntries = new List<EmployeeBanking>
        {
            new EmployeeBanking(_employeeBankingDto)
        };

        _employeeBankingServiceMock.Setup(x => x.Get(1))
                   .ReturnsAsync(expectedEntries);

        var result = await _controller.Get(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEntries = Assert.IsType<List<EmployeeBanking>>(okResult.Value);
        Assert.Equal(expectedEntries.Count, actualEntries.Count);
    }

    [Fact]
    public async Task GetExceptionThrownReturnsNotFoundResultWithErrorMessage()
    {
        _employeeBankingServiceMock.Setup(x => x.Get(1)).ThrowsAsync(new Exception("Unable to retrieve employee banking entries"));

        var result = await _controller.Get(1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Unable to retrieve employee banking entries", actualErrorMessage);
    }

    [Fact]
    public async Task UpdateValidDataReturnsOkResult()
    {
        _employeeBankingServiceMock.Setup(x => x.Update(It.IsAny<EmployeeBankingDto>(), "test@example.com"))
                   .ReturnsAsync(_employeeBankingDto);

        var result = await _controller.Update(_simpleEmployeeBankingDto);

        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task UpdateExceptionThrownReturnsNotFoundResultWithErrorMessage()
    {
        _employeeBankingServiceMock.Setup(x => x.Update(It.IsAny<EmployeeBankingDto>(), "test@example.com"))
                   .ThrowsAsync(new Exception("Some error message"));

        var result = await _controller.Update(_simpleEmployeeBankingDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Some error message", actualErrorMessage);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task GetBankingDetailsValidIdReturnsOkResultWithDetails()
    {
        _employeeBankingServiceMock.Setup(x => x.GetBanking(123))
            .ReturnsAsync(new List<EmployeeBankingDto> { _employeeBankingDto });

        var result = await _controller.GetBankingDetails(123);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDetails = Assert.IsType<List<EmployeeBankingDto>>(okResult.Value);
        Assert.Contains(_employeeBankingDto, actualDetails);
    }

    [Fact(Skip = "Tampering found")]
    public async Task GetBankingDetailsInvalidIdReturnsNotFoundResultWithErrorMessage()
    {
        _employeeBankingServiceMock.Setup(x => x.GetBanking(456)).ThrowsAsync(new Exception("Employee banking details not found"));

        var result = await _controller.GetBankingDetails(456);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Employee banking details not found", actualErrorMessage);
    }
}