using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RGO.App.Controllers;

public class EmployeeBanking : ControllerBase
{
    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("getPending")]
    public async Task<IActionResult> FetchPending()
    {
        return Ok("Your controller works");
    }
}