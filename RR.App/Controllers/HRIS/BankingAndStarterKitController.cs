using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("banking-starterkits")]
[ApiController]
public class BankingAndStarterKitController : ControllerBase
{
    private readonly IBankingAndStarterKitService _bankingAndStarterKitService;

    public BankingAndStarterKitController(IBankingAndStarterKitService bankingAndStarterKitService)
    {
        _bankingAndStarterKitService = bankingAndStarterKitService ;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet()]
    public async Task<IActionResult> GetAllDocuments()
    {
        try
        {
            var employeeDocuments = await _bankingAndStarterKitService.GetBankingAndStarterKitAsync();
            return Ok(employeeDocuments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching the employee documents.");
        }
    }
}