using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.App.Controllers;
using Xunit;
using RGO.Models.Enums;

namespace RGO.App.Tests.Controllers
{
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
    }
}