using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class BankingStarterKitControllerUnitTest
{
    private readonly Mock<IBankingAndStarterKitService> _bankingStarterKitServiceMock;
    private readonly BankingAndStarterKitController _controller;
    private readonly List<BankingAndStarterKitDto> _bankingStarterKitDtoList;
    public BankingStarterKitControllerUnitTest()
    {
        _bankingStarterKitServiceMock = new Mock<IBankingAndStarterKitService>();
        _controller = new BankingAndStarterKitController(_bankingStarterKitServiceMock.Object);
         
        _bankingStarterKitDtoList = new List<BankingAndStarterKitDto>
            { new BankingAndStarterKitDto
               {
                  Name = "Name",
                  Surname = "BankingStarterKit",
                  EmployeeId = 1,
               }
            };
    }

    [Fact]
    public async Task GetAllClientsReturnsOkResultWithClients()
    {
        _bankingStarterKitServiceMock.Setup(service => service.GetBankingAndStarterKitAsync())
                         .ReturnsAsync(_bankingStarterKitDtoList);

        var result = await _controller.GetAllDocuments();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualBankingStarterKit = Assert.IsAssignableFrom<List<BankingAndStarterKitDto>>(okResult.Value);
        Assert.Equal(_bankingStarterKitDtoList, actualBankingStarterKit);

  
    }
}