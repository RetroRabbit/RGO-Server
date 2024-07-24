using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("field-code")]
[ApiController]
public class FieldCodeController : ControllerBase
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
        var getFieldCodes = await _fieldCodeService.GetAllFieldCodes();
        return Ok(getFieldCodes);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPost]
    public async Task<IActionResult> SaveFieldCode([FromBody] FieldCodeDto fieldCodeDto)
    {
        var savedFieldCode = await _fieldCodeService.CreateFieldCode(fieldCodeDto);
        return Ok(savedFieldCode);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateFieldCode([FromBody] FieldCodeDto fieldCodeDto)
    {
        var updatedFieldCode = await _fieldCodeService.UpdateFieldCode(fieldCodeDto);
        return Ok(updatedFieldCode);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteFieldCode([FromBody] FieldCodeDto fieldCodeDto)
    {
        var deletedFieldCode = await _fieldCodeService.DeleteFieldCode(fieldCodeDto);
        return Ok(deletedFieldCode);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("category")]
    public async Task<IActionResult> GetByCategory([FromQuery] int category)
    {
        var categoryCodes = await _fieldCodeService.GetByCategory(category);
        return Ok(categoryCodes);
    }
}
