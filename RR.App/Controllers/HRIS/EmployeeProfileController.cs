using HRIS.Models.Employee.Commons;
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

    [HttpPut("employee-details")]
    public async Task<IActionResult> UpdateEmployeeDetails([FromBody] EmployeeDetailsDto employeeDetails)
    {
        var result = await _employeeProfileService.UpdateEmployeeDetails(employeeDetails);
        return Ok(result);
    }

    [HttpPut("personal-details")]
    public async Task<IActionResult> UpdatePersonalDetails([FromBody] PersonalDetailsDto personalDetails)
    {
        var result = await _employeeProfileService.UpdatePersonalDetails(personalDetails);
        return Ok(result);
    }

    [HttpPut("contact-details")]
    public async Task<IActionResult> UpdateContactDetails([FromBody] ContactDetailsDto contactDetails)
    {
        var result = await _employeeProfileService.UpdateContactDetails(contactDetails);
        return Ok(result);
    }
}