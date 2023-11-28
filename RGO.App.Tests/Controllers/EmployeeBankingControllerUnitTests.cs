using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.App.Controllers;
using Xunit;
using RGO.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace RGO.App.Tests.Controllers;

public class EmployeeBankingControllerUnitTests
{
    [Fact]
    public async Task AddBankingInfoValidInputReturnsOkResult()
    {
        var mockService = new Mock<IEmployeeBankingService>();
        var controller = new EmployeeBankingController(mockService.Object);
        var newEntry = new SimpleEmployeeBankingDto
            (1, 2, "BankName", "Branch", "AccountNo", EmployeeBankingAccountType.Savings,
            "AccountHolderName", BankApprovalStatus.Approved, "DeclineReason", "File.pdf");

        var result = await controller.AddBankingInfo(newEntry);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);

    }

    [Fact]
    public async Task UpdateInvalidInputReturnsBadRequest()
    {
        var mockService = new Mock<IEmployeeBankingService>();
        var controller = new EmployeeBankingController(mockService.Object);

        var updateEntry = new SimpleEmployeeBankingDto
            (1, 2, "BankName", "Branch", "AccountNo", EmployeeBankingAccountType.Savings,
            string.Empty, BankApprovalStatus.Approved, "DeclineReason", "File.pdf");

        var result = await controller.Update(updateEntry);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task AddBankingInfoExceptionWithDetailsAlreadyExistReturnsProblemResult()
    {
        var mockService = new Mock<IEmployeeBankingService>();
        var controller = new EmployeeBankingController(mockService.Object);

        var newEntry = new SimpleEmployeeBankingDto
        (1, 2, "Bank Name", "Branch", "Account No", EmployeeBankingAccountType.Savings, "Account Holder Name", BankApprovalStatus.Approved, "Decline Reason", "File.pdf");

        mockService.Setup(x => x.Save(It.IsAny<EmployeeBankingDto>()))
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
        var mockService = new Mock<IEmployeeBankingService>();
        var controller = new EmployeeBankingController(mockService.Object);

        var newEntry = new SimpleEmployeeBankingDto
        (1, 2, "Bank Name", "Branch", "Account No", EmployeeBankingAccountType.Savings, "Account Holder Name", BankApprovalStatus.Approved, "Decline Reason", "File.pdf");

        mockService.Setup(x => x.Save(It.IsAny<EmployeeBankingDto>()))
            .ThrowsAsync(new Exception("Some error message containing NotFound"));

        var result = await controller.AddBankingInfo(newEntry);

        Assert.NotNull(result);
        Assert.IsType<NotFoundObjectResult>(result);

        var notFoundResult = (NotFoundObjectResult)result;

        Assert.NotNull(notFoundResult.Value);
        Assert.IsType<string>(notFoundResult.Value); // Adjust this line based on the actual type returned

        var errorMessage = (string)notFoundResult.Value;

        Assert.Equal("Some error message containing NotFound", errorMessage);
    }
}