using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("employee-address")]
[ApiController]
public class EmployeeAddressController : ControllerBase
{
    private readonly IEmployeeAddressService _employeeAddressService;
    private readonly AuthorizeIdentity _identity;

    public EmployeeAddressController(IEmployeeAddressService employeeAddressService)
    {
        _employeeAddressService = employeeAddressService;
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        
            var addresses = await _employeeAddressService.GetAll();

            return Ok(addresses);
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpPost]
    public async Task<IActionResult> SaveEmployeeAddress([FromBody] EmployeeAddressDto address)
    {
        
            var savedAddress = await _employeeAddressService.Create(address);

            return Ok(savedAddress);
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateEmployeeAddress([FromBody] EmployeeAddressDto address)
    {
        
            var updatedAddress = await _employeeAddressService.Update(address);

            return Ok(updatedAddress);
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteEmployeeAddress(int addressId)
    {
            var deletedAddress = await _employeeAddressService.Delete(addressId);

            return Ok(deletedAddress);
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpGet]
    public async Task<IActionResult> GetEmployeeAddressById(int employeeId)
    {
        var employeeAddress = await _employeeAddressService.GetById(employeeId);
        return Ok(employeeAddress);
    }
        
}