﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.App.Controllers;
using Xunit;
using RGO.Models.Enums;
using Microsoft.AspNetCore.Http;
using RGO.UnitOfWork.Entities;

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
        (1, 2, "Bank Name", "Branch", "Account No", EmployeeBankingAccountType.Savings, 
        "Account Holder Name", BankApprovalStatus.Approved, "Decline Reason", "File.pdf");

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
        (1, 2, "Bank Name", "Branch", "Account No", EmployeeBankingAccountType.Savings, 
        "Account Holder Name", BankApprovalStatus.Approved, "Decline Reason", "File.pdf");

        mockService.Setup(x => x.Save(It.IsAny<EmployeeBankingDto>()))
            .ThrowsAsync(new Exception("Some error message containing NotFound"));

        var result = await controller.AddBankingInfo(newEntry);

        Assert.NotNull(result);
        Assert.IsType<NotFoundObjectResult>(result);

        var notFoundResult = (NotFoundObjectResult)result;

        Assert.NotNull(notFoundResult.Value);
        Assert.IsType<string>(notFoundResult.Value);

        var errorMessage = (string)notFoundResult.Value;

        Assert.Equal("Some error message containing NotFound", errorMessage);
    }

    [Fact]
    public async Task GetValidStatusReturnsOkResultWithEntries()
    {
        var mockService = new Mock<IEmployeeBankingService>();
        var controller = new EmployeeBankingController(mockService.Object);
        var status = 1;

        var expectedEntries = new List<EmployeeBanking>
        {
            new EmployeeBanking
            {   
                Id = 1, EmployeeId = 123, BankName = "Test Bank", Branch = "Test Branch", 
                AccountNo = "123456789", AccountType = EmployeeBankingAccountType.Savings, 
                AccountHolderName = "John Doe", Status = BankApprovalStatus.Approved, 
                DeclineReason = null, File = "file.pdf", 
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
        var mockService = new Mock<IEmployeeBankingService>();
        var controller = new EmployeeBankingController(mockService.Object);
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
        var mockService = new Mock<IEmployeeBankingService>();
        var controller = new EmployeeBankingController(mockService.Object);

        var updateEntry = new SimpleEmployeeBankingDto
        (1, 123, "Test Bank", "Test Branch", "123456789", EmployeeBankingAccountType.Savings, 
        "John Doe", BankApprovalStatus.Approved, null, "file.pdf");

        mockService.Setup(x => x.Update(It.IsAny<EmployeeBankingDto>()))
            .ReturnsAsync(new EmployeeBankingDto
            (1, 123, "Test Bank", "Test Branch", "123456789", EmployeeBankingAccountType.Savings,
            "John Doe", BankApprovalStatus.Approved, null, "file.pdf", new DateOnly(), new DateOnly()));

        var result = await controller.Update(updateEntry);

        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
}