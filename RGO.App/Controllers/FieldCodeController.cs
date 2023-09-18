using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.UnitOfWork.Entities;

namespace RGO.App.Controllers
{
    [Route("/fieldcode/")]
    [ApiController]
    public class FieldCodeController : Controller
    {
        private readonly IFieldCodeService _fieldCodeService;
        private readonly IFieldCodeOptionsService _fieldCodeOptionsService;

        public FieldCodeController(IFieldCodeService fieldCodeService, IFieldCodeOptionsService fieldCodeOptionsService)
        {
            _fieldCodeService = fieldCodeService;
            _fieldCodeOptionsService = fieldCodeOptionsService;
        }

        [HttpGet("get")]
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

        [HttpPost("save")]
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

        //TODO: Update this controller 
        [HttpPut("update")]
        public async Task<IActionResult> UpdateFieldCode([FromBody] FieldCodeDto fieldCodeDto)
        {
            try
            {
                await _fieldCodeService.UpdateFieldCode(fieldCodeDto);

                if (fieldCodeDto.Options.Count > 0)
                {
                    await _fieldCodeOptionsService.UpdateFieldCodeOptions(fieldCodeDto.Options);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFieldCode([FromBody] FieldCodeDto fieldCodeDto)
        {
            try
            {

                await _fieldCodeService.DeleteFieldCode(fieldCodeDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
