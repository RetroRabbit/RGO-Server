using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers
{
    [Route("/employeeaddress/")]
    [ApiController]
    public class EmployeeAddressController : ControllerBase
    {
        private readonly IEmployeeAddressService _employeeAddressService;

        public EmployeeAddressController(IEmployeeAddressService employeeAddressService)
        {
            _employeeAddressService = employeeAddressService;
        }

        [Authorize(Policy = "AdminOrEmployeePolicy")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var addresses = await _employeeAddressService.GetAll();

                return Ok(addresses);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "AdminOrEmployeePolicy")]
        [HttpPost("save")]
        public async Task<IActionResult> SaveEmployeeAddress([FromBody] EmployeeAddressDto address)
        {
            try
            {
                var savedAddress = await _employeeAddressService.Save(address);

                return Ok(savedAddress);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "AdminOrEmployeePolicy")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateEmployeeAddress([FromBody] EmployeeAddressDto address)
        {
            try
            {
                var updatedAddress = await _employeeAddressService.Update(address);

                return Ok(updatedAddress);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "AdminOrEmployeePolicy")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteEmployeeAddress([FromBody] EmployeeAddressDto address)
        {
            try
            {
                var deletedAddress = await _employeeAddressService.Delete(address);

                return Ok(deletedAddress);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
