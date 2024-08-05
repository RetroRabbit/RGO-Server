using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Mvc;
namespace RR.App.Controllers.HRIS;

[Route("employee-profile")]
[ApiController]
public class EmployeeProfileController : ControllerBase
{
    private readonly IEmployeeProfileService _employeeProfileService;

    public EmployeeProfileController(AuthorizeIdentity identity, IEmployeeProfileService employeeProfileService)
    {
        _employeeProfileService = employeeProfileService;
    }

    [HttpGet("profile-details")]
    public async Task<IActionResult> GetEmployeeProfileDetailsById([FromQuery] int? id)
    {
        var profileDetails = await _employeeProfileService.GetEmployeeProfileDetailsById(id);
        return Ok(profileDetails);
    }


    [HttpGet("career-summary")]
    public async Task<IActionResult> GetEmployeeCareerSummaryById([FromQuery] int? id)
    {
        var careerSummary = await _employeeProfileService.GetEmployeeCareerSummaryById(id);
        return Ok(careerSummary);
    }

    [HttpGet("banking-information")]
    public async Task<IActionResult> GetEmployeeBankingInformationById([FromQuery] int? id)
    {
        var bankingInformation = await _employeeProfileService.GetEmployeeBankingInformationById(id);
        return Ok(bankingInformation);
    }
}