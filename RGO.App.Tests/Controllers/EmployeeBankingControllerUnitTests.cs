using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.App.Controllers;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class EmployeeBankingControllerUnitTests
{
    [Fact]
    public async Task AddBankingInfoReturnsOkResult()
    {
        var newEntry = new EmployeeBankingDto
        (
            1,
            1001,
            "Bank 1",
            "Branch 1",
            "Account Number 1",
            EmployeeBankingAccountType.Cheque,
            "Account Holder Name 1",
            BankApprovalStatus.Approved,
            " ",
            "Document.pdf"
        );

        var mockEmployeeBankingService = new Mock<IEmployeeBankingService>();
        mockEmployeeBankingService.Setup(s => s.Save(newEntry)).ReturnsAsync(newEntry);

        var controller = new EmployeeBankingController(mockEmployeeBankingService.Object);

        var result = await controller.AddBankingInfo(newEntry);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEntry = Assert.IsAssignableFrom<EmployeeBankingDto>(okResult.Value);
        Assert.Equal(newEntry, actualEntry);
    }

    [Fact]
    public async Task UpdateReturnsOkResult()
    {
        var updateEntry = new EmployeeBankingDto
        (1, 1001, "Bank 1", "Branch 1", "Account Number 1", EmployeeBankingAccountType.Cheque, "Account Holder Name 1", BankApprovalStatus.Approved, " ", "Document.pdf");

        var mockEmployeeBankingService = new Mock<IEmployeeBankingService>();
        mockEmployeeBankingService.Setup(s => s.Update(updateEntry)).ReturnsAsync(updateEntry);

        var controller = new EmployeeBankingController(mockEmployeeBankingService.Object);

        var result = await controller.Update(updateEntry);

        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetBankingDetailsReturnsOkResult()
    {
        var expectedId = 1;

        var mockEmployeeBankingService = new Mock<IEmployeeBankingService>();
        mockEmployeeBankingService.Setup(s => s.GetBanking(expectedId)).ReturnsAsync(new EmployeeBankingDto
            (
            1,
            1001,
            "Bank 1",
            "Branch 1",
            "Account Number 1",
            EmployeeBankingAccountType.Cheque,
            "Account Holder Name 1",
            BankApprovalStatus.Approved,
            " ",
            "Document.pdf"
            ));

        var controller = new EmployeeBankingController(mockEmployeeBankingService.Object);

        var result = await controller.GetBankingDetails(expectedId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEntry = Assert.IsAssignableFrom<EmployeeBankingDto>(okResult.Value);
        Assert.Equal(expectedId, actualEntry.Id);
    
    }
}