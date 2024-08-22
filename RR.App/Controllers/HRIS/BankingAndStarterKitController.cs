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

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet()]
    public async Task<IActionResult> GetAllDocuments()
    {
        var employeeDocuments = await _bankingAndStarterKitService.GetBankingAndStarterKitAsync();
        return Ok(employeeDocuments);
    }
}
