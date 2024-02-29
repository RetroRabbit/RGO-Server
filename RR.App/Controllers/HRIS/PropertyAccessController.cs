using HRIS.Models.Update;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("access")]
public class PropertyAccessController : ControllerBase
{
    private readonly IPropertyAccessService _propertyAccessService;

    public PropertyAccessController(IPropertyAccessService propertyAccessService)
    {
        _propertyAccessService = propertyAccessService;
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpGet]
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
    [HttpPut]
    public async Task<IActionResult> UpdatePropertyWithAccess([FromBody] List<UpdateFieldValueDto> fields,
                                                              [FromQuery] string email)
    {
        try
        {
            await _propertyAccessService.UpdatePropertiesWithAccess(fields, email);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}