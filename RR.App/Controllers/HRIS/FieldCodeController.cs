using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("field-code")]
[ApiController]
public class FieldCodeController : Controller
{
    private readonly IFieldCodeService _fieldCodeService;

    public FieldCodeController(IFieldCodeService fieldCodeService)
    {
        _fieldCodeService = fieldCodeService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAllFieldCodes()
    {
        try
        {
            var getFieldCodes = await _fieldCodeService.GetAllFieldCodes();

            if (getFieldCodes == null) throw new Exception("No field codes found");

            return Ok(getFieldCodes);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> SaveFieldCode([FromBody] FieldCodeDto fieldCodeDto)
    {
        try
        {
            var savedFieldCode = await _fieldCodeService.SaveFieldCode(fieldCodeDto);
            return Ok(savedFieldCode);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateFieldCode([FromBody] FieldCodeDto fieldCodeDto)
    {
        try
        {
            var updatedFieldCode = await _fieldCodeService.UpdateFieldCode(fieldCodeDto);
            return Ok(updatedFieldCode);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFieldCode([FromBody] FieldCodeDto fieldCodeDto)
    {
        try
        {
            var deletedFieldCode = await _fieldCodeService.DeleteFieldCode(fieldCodeDto);
            return Ok(deletedFieldCode);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("category")]
    public async Task<IActionResult> GetByCategory([FromQuery] int category)
    {
        try
        {
            var categoryCodes = await _fieldCodeService.GetByCategory(category);
            return Ok(categoryCodes);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
