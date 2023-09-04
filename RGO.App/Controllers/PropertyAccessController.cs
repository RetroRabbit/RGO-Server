using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using System.Security.Claims;

namespace RGO.App.Controllers;

[Route("/access/")]
public class PropertyAccessController : ControllerBase
{
    private readonly IPropertyAccessService _propertyService;

    public PropertyAccessController(IPropertyAccessService propertyAccess)
    {
        _propertyService = propertyAccess;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetPropertyWithAccess([FromQuery] string email)
    {
        try
        {
            var access = await _propertyService.GetPropertiesWithAccess(email);
            return Ok(access);

        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdatePropertyWithAccess([FromBody] EmployeeDto employee, [FromQuery] string email)
    {
        try
        {
            return NotFound("This section of the feature is still under Development");
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
