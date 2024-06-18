using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("clients")]
[ApiController]
public class ClientController : Controller
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAllClients()
    {
        try
        {
            var getClients = await _clientService.GetAllClients();

            if (getClients == null) throw new Exception("No clients found");

            return Ok(getClients);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}