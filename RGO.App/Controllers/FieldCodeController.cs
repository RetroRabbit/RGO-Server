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
                var getFieldCodesOptions = await _fieldCodeOptionsService.GetAllFieldCodeOptions();

                if (getFieldCodes == null) throw new Exception("No field codes found");
                if (getFieldCodesOptions == null) throw new Exception("No field code options found");

                FieldCodeData employeeData = new FieldCodeData
                {
                    NewFieldCode = getFieldCodes,
                    FieldCodeOptions = getFieldCodesOptions
                };
                return Ok(employeeData);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveFieldCode([FromBody] FieldCodeData fieldCodeData)
        {
            try
            {
                if (fieldCodeData.NewFieldCode.Count > 0)
                {
                    foreach (var item in fieldCodeData.NewFieldCode)
                    {
                        await _fieldCodeService.SaveFieldCode(item);
                    }
                }
                var getFieldCode = await _fieldCodeService.GetFieldCode(fieldCodeData.NewFieldCode[0].Name);

                if (fieldCodeData.FieldCodeOptions.Count > 0)
                {
                    foreach (var item in fieldCodeData.FieldCodeOptions)
                    {
                        var fieldCodeOptionsDto = new FieldCodeOptionsDto(
                            Id: 0,
                            FieldCode: getFieldCode,
                            Option: item.Option);
                        await _fieldCodeOptionsService.SaveFieldCodeOptions(fieldCodeOptionsDto);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateFieldCode([FromBody] FieldCodeData fieldCodeData)
        {
            try
            {
                if (fieldCodeData.NewFieldCode.Count > 0)
                {
                    foreach (var item in fieldCodeData.NewFieldCode)
                    {
                        await _fieldCodeService.UpdateFieldCode(item);
                    }
                }

                if (fieldCodeData.FieldCodeOptions.Count > 0)
                {
                    await _fieldCodeOptionsService.UpdateFieldCodeOptions(fieldCodeData.FieldCodeOptions);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFieldCode([FromBody] FieldCodeData fieldCodeData)
        {
            try
            {
                if (fieldCodeData.NewFieldCode.Count > 0)
                {
                    foreach (var item in fieldCodeData.NewFieldCode)
                    {
                        await _fieldCodeService.DeleteFieldCode(item);
                    }
                }

                if (fieldCodeData.FieldCodeOptions.Count > 0)
                {
                    foreach (var item in fieldCodeData.FieldCodeOptions)
                    {
                        await _fieldCodeOptionsService.DeleteFieldCodeOptions(item);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
