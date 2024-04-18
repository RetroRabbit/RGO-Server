using System.Security.Claims;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeBankingControllerUnitTests
{
    private readonly List<Claim> claims;
    private readonly ClaimsPrincipal claimsPrincipal;
    private readonly EmployeeBankingController controller;
    private readonly ClaimsIdentity identity;

    private readonly Mock<IEmployeeBankingService> mockService;

    private readonly SimpleEmployeeBankingDto newEntry = new SimpleEmployeeBankingDto
    {
        Id = 1,
        EmployeeId = 2,
        BankName = "BankName",
        Branch = "Branch",
        AccountNo = "AccountNo",
        AccountType = EmployeeBankingAccountType.Savings,
        AccountHolderName = "AccountHolderName",
        Status = BankApprovalStatus.Approved,
        DeclineReason = "DeclineReason",
        File = "File.pdf"
    };

    private readonly SimpleEmployeeBankingDto updateEntry = new SimpleEmployeeBankingDto
    {
        Id = 1,
        EmployeeId = 123,
        BankName = "Test Bank",
        Branch = "Test Branch",
        AccountNo = "123456789",
        AccountType = EmployeeBankingAccountType.Savings,
        AccountHolderName = "John Doe",
        Status = BankApprovalStatus.Approved,
        DeclineReason = null,
        File = "file.pdf"
    };
    public EmployeeBankingControllerUnitTests()
    {
        mockService = new Mock<IEmployeeBankingService>();
        controller = new EmployeeBankingController(mockService.Object);

        claims = new List<Claim>
        {
            new(ClaimTypes.Email, "test@example.com")
        };

        identity = new ClaimsIdentity(claims, "TestAuthType");
        claimsPrincipal = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }

    [Fact]
    public async Task AddBankingInfoValidInputReturnsOkResult()
    {
        var result = await controller.AddBankingInfo(newEntry);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task UpdateInvalidInputReturnsBadRequest()
    {
        var updateEntry = new SimpleEmployeeBankingDto
        {
            Id = 1,
            EmployeeId = 2,
            BankName = "BankName",
            Branch = "Branch",
            AccountNo = "AccountNo",
            AccountType = EmployeeBankingAccountType.Savings,
            AccountHolderName = "",
            Status = BankApprovalStatus.Approved,
            DeclineReason = "DeclineReason",
            File = "File.pdf"
        };

        var result = await controller.Update(updateEntry);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task AddBankingInfoExceptionWithDetailsAlreadyExistReturnsProblemResult()
    {
        mockService.Setup(x => x.Save(It.IsAny<EmployeeBankingDto>(), "test@example.com"))
                   .ThrowsAsync(new Exception("Details already exists"));

        var result = await controller.AddBankingInfo(newEntry);

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
        mockService.Setup(x => x.Save(It.IsAny<EmployeeBankingDto>(), "test@example.com"))
                   .ThrowsAsync(new Exception("Banking information Not Found"));

        var result = await controller.AddBankingInfo(newEntry);

        Assert.NotNull(result);
        Assert.IsType<NotFoundObjectResult>(result);

        var notFoundResult = (NotFoundObjectResult)result;

        Assert.NotNull(notFoundResult.Value);
        Assert.IsType<string>(notFoundResult.Value);

        var errorMessage = (string)notFoundResult.Value;

        Assert.Equal("Banking information Not Found", errorMessage);
    }

    [Fact]
    public async Task DeleteBankingInfoExceptionNotFound()
    {
        var bankingInfoToDelete = EmployeeBankingTestData.EmployeeBankingDto3;
        var exceptionMessage = "Banking information Not Found";

        var mockEmployeeBankingService = new Mock<IEmployeeBankingService>();
        mockEmployeeBankingService.Setup(s => s.Delete(bankingInfoToDelete.Id))
                                  .ThrowsAsync(new Exception(exceptionMessage));

        var controller = new EmployeeBankingController(mockEmployeeBankingService.Object);

        var result = await controller.DeleteBankingInfo(bankingInfoToDelete.Id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task AddBankingInfoUnauthorizedAccess()
    {
        mockService.Setup(x => x.Save(It.IsAny<EmployeeBankingDto>(), "test@example.com"))
                   .ThrowsAsync(new Exception("Unauthorized access"));

        var result = await controller.AddBankingInfo(newEntry);
        Assert.NotNull(result);

        var unauthorized = (ObjectResult)result;
        Assert.Equal("Forbidden: Unauthorized access", unauthorized.Value);
    }

    [Fact]
    public async Task GetValidStatusReturnsOkResultWithEntries()
    {
        var status = 1;

        var expectedEntries = new List<EmployeeBanking>
        {
            new()
            {
                Id = 1, EmployeeId = 123, BankName = "Test Bank", Branch = "Test Branch",
                AccountNo = "123456789", AccountType = EmployeeBankingAccountType.Savings,
                AccountHolderName = "John Doe", Status = BankApprovalStatus.Approved,
                DeclineReason = string.Empty, File = "file.pdf",
                LastUpdateDate = new DateOnly(2023, 11, 28), PendingUpdateDate = new DateOnly(2023, 11, 29)
            }
        };

        mockService.Setup(x => x.Get(status))
                   .ReturnsAsync(expectedEntries);

        var result = await controller.Get(status);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEntries = Assert.IsType<List<EmployeeBanking>>(okResult.Value);

        Assert.Equal(expectedEntries.Count, actualEntries.Count);
    }

    [Fact]
    public async Task GetExceptionThrownReturnsNotFoundResultWithErrorMessage()
    {
        var status = 1;
        var errorMessage = "Unable to retrieve employee banking entries";

        mockService.Setup(x => x.Get(status)).ThrowsAsync(new Exception(errorMessage));

        var result = await controller.Get(status);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

        Assert.Equal(errorMessage, actualErrorMessage);
    }

    [Fact]
    public async Task UpdateValidDataReturnsOkResult()
    {
        mockService.Setup(x => x.Update(It.IsAny<EmployeeBankingDto>(), "test@example.com"))
                   .ReturnsAsync(new EmployeeBankingDto
                   {
                       Id = 1,
                       EmployeeId = 123,
                       BankName = "Test Bank",
                       Branch = "Test Branch",
                       AccountNo = "123456789",
                       AccountType = EmployeeBankingAccountType.Savings,
                       AccountHolderName = "John Doe",
                       Status = BankApprovalStatus.Approved,
                       DeclineReason = null,
                       File = "file.pdf",
                       LastUpdateDate = new DateOnly(),
                       PendingUpdateDate = new DateOnly()
                   });

        var result = await controller.Update(updateEntry);

        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task UpdateExceptionThrownReturnsNotFoundResultWithErrorMessage()
    {
        var errorMessage = "Some error message";
        mockService.Setup(x => x.Update(It.IsAny<EmployeeBankingDto>(), "test@example.com"))
                   .ThrowsAsync(new Exception(errorMessage));

        var result = await controller.Update(updateEntry);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

        Assert.Equal(errorMessage, actualErrorMessage);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetBankingDetailsValidIdReturnsOkResultWithDetails()
    {
        var id = 123;

        var newEntry = new EmployeeBankingDto
        {
            Id = 1,
            EmployeeId = 123,
            BankName = "Test Bank",
            Branch = "Test Branch",
            AccountNo = "123456789",
            AccountType = EmployeeBankingAccountType.Savings,
            AccountHolderName = "John Doe",
            Status = BankApprovalStatus.Approved,
            DeclineReason = null,
            File = "file.pdf",
            LastUpdateDate = new DateOnly(),
            PendingUpdateDate = new DateOnly()
        };
        mockService.Setup(x => x.GetBanking(id)).ReturnsAsync(new List<EmployeeBankingDto> { newEntry });

        var result = await controller.GetBankingDetails(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDetails = Assert.IsType<List<EmployeeBankingDto>>(okResult.Value);

        Assert.Contains(newEntry, actualDetails);
    }

    [Fact]
    public async Task GetBankingDetailsInvalidIdReturnsNotFoundResultWithErrorMessage()
    {
        var id = 456;
        var errorMessage = "Employee banking details not found";

        mockService.Setup(x => x.GetBanking(id)).ThrowsAsync(new Exception(errorMessage));

        var result = await controller.GetBankingDetails(id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

        Assert.Equal(errorMessage, actualErrorMessage);
    }
}
