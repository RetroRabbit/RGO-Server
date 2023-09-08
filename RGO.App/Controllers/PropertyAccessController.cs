using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Models.Update;
using RGO.Services.Interfaces;


namespace RGO.App.Controllers;


[Route("/access/")]
public class PropertyAccessController : ControllerBase
{
    private readonly IPropertyAccessService _propertyAccessService;

    public PropertyAccessController(IPropertyAccessService propertyAccessService)
    {
        _propertyAccessService = propertyAccessService;
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpGet("get")]
    public async Task<IActionResult> GetPropertyWithAccess([FromQuery] string email)
    {
        try
        {
            var access = await _propertyAccessService.GetPropertiesWithAccess(email);
            return Ok(access);

        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdatePropertyWithAccess([FromBody] List<UpdateFieldValueDto> fields, [FromQuery] string email)
    {
        try
        {
            await _propertyAccessService.UpdatePropertiesWithAccess( fields, email);
            return Ok(); 
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
