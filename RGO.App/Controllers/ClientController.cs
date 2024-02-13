using Microsoft.AspNetCore.Mvc;
using RGO.Services.Interfaces;
using RGO.Services.Services;

namespace RGO.App.Controllers
{
    [Route("clients")]
    [ApiController]
    public class ClientController : Controller
    {
       private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet()]
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
}
