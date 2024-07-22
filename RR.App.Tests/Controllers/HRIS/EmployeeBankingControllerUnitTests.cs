using System.Security.Claims;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.App.Tests.Helper;
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
    private readonly Mock<AuthorizeIdentityMock> _identity;
    private readonly List<EmployeeBankingDto> _employeeBankingDtoList;
    private readonly Mock<IEmployeeBankingService> _employeeBankingServiceMock;
    private readonly EmployeeBankingDto _employeeBankingDto;

    public EmployeeBankingControllerUnitTests()
    {
        _identity = new Mock<AuthorizeIdentityMock>();
        _employeeBankingServiceMock = new Mock<IEmployeeBankingService>();
        _controller = new EmployeeBankingController(_identity.Object, _employeeBankingServiceMock.Object);

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

        _employeeBankingDtoList = new List<EmployeeBankingDto>
            {
                new EmployeeBankingDto
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
                },
        };

        _employeeBankingDto = EmployeeBankingTestData.EmployeeBankingOne.ToDto();
    }

    [Fact]
    public async Task AddBankingInfoValidInputReturnsOkResult()
    {
        var result = await _controller.AddBankingInfo(_employeeBankingDto);

        Assert.IsType<OkObjectResult>(result);
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
    public async Task UpdateValidDataReturnsOkResult()
    {
        _employeeBankingServiceMock.Setup(x => x.Update(It.IsAny<EmployeeBankingDto>(), "test@example.com"))
                   .ReturnsAsync(_employeeBankingDto);

        var result = await _controller.Update(_employeeBankingDto);

        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetBankingDetailsReturnsOkResult()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(2);
        _identity.SetupGet(i => i.Email).Returns("admin@example.com");

        _employeeBankingServiceMock.Setup(x => x.GetBanking(_employeeBankingDto.EmployeeId))
                                   .ReturnsAsync(_employeeBankingDtoList);

        var result = await _controller.GetBankingDetails(_employeeBankingDto.EmployeeId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<EmployeeBankingDto>>(okResult.Value);
        Assert.Equal(_employeeBankingDtoList, returnValue);
        _employeeBankingServiceMock.Verify(service => service.GetBanking(_employeeBankingDto.EmployeeId), Times.Once);
    }

    [Fact]
    public async Task GetBankingUnauthorizedAccess()
    {
        _identity.Setup(i => i.Role).Returns("Talent");
        _identity.SetupGet(i => i.EmployeeId).Returns(5);
        _identity.SetupGet(i => i.Email).Returns("talent@example.com");

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _controller.GetBankingDetails(2));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        Assert.Equal("User data being accessed does not match user making the request.", notFoundResult.Value);
    }

    [Fact]
    public async Task GetBankingDetailsAsSuperAdminReturnsOkResult()
    {
        _identity.Setup(i => i.Role).Returns("SuperAdmin");
        _identity.SetupGet(i => i.EmployeeId).Returns(2);
        _identity.SetupGet(i => i.Email).Returns("superadmin@example.com");

        _employeeBankingServiceMock.Setup(x => x.GetBanking(It.IsAny<int>()))
                                   .ReturnsAsync(_employeeBankingDtoList);

        var result = await _controller.GetBankingDetails(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<EmployeeBankingDto>>(okResult.Value);
        Assert.Equal(_employeeBankingDtoList, returnValue);
        _employeeBankingServiceMock.Verify(service => service.GetBanking(1), Times.Once);
    }

    [Fact]
    public async Task DeleteBankingInfo_ValidAddressId_ReturnsOkResult()
    {
        var addressId = 1;
        _employeeBankingServiceMock.Setup(x => x.Delete(addressId)).ReturnsAsync(_employeeBankingDto);

        var result = await _controller.DeleteBankingInfo(addressId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResult = Assert.IsType<EmployeeBankingDto>(okResult.Value);
        Assert.Equal(_employeeBankingDto, actualResult);
    }

}