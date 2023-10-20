using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers
{
    [Route("/emploayeeaddress/")]
    [ApiController]
    public class EmployeeAddressController : ControllerBase
    {
        private readonly IEmployeeAddressService _employeeAddressService;

        public EmployeeAddressController(IEmployeeAddressService employeeAddressService)
        {
            _employeeAddressService = employeeAddressService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromQuery] string? email = null)
        {
            try
            {
                var addresses = email != null ? await _employeeAddressService.GetAllByEmployee(email) :  await _employeeAddressService.GetAll();

                return Ok(addresses);
            } catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveEmployeeAddress([FromBody] EmployeeAddressDto address)
        {
            try
            {
                await _employeeAddressService.Save(address);

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateEmployeeAddress([FromBody] EmployeeAddressDto address)
        {
            try
            {
                await _employeeAddressService.Update(address);

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteEmployeeAddress([FromBody] EmployeeAddressDto address)
        {
            try
            {
                await _employeeAddressService.Delete(address);

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
