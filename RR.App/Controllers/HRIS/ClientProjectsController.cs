using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.App.Controllers.HRIS
{
    [Route("client-project")]
    [ApiController]
    public class ClientProjectsController : ControllerBase
    {
        private readonly IClientProjectService _clientProjectService;
        public ClientProjectsController(IClientProjectService clientProjectService)
        {
            _clientProjectService = clientProjectService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllClientProjects()
        {
            try
            {
                var clientProjects = await _clientProjectService.GetAllClientProjects();
                return Ok(clientProjects);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientProjectsDto>> GetClientProjectById(int id)
        {
            try
            {
                var clientProjectDto = await _clientProjectService.GetClientProjectById(id);
                return Ok(clientProjectDto);
            } 
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveClientProject(ClientProjectsDto clientProjectsDto)
        {
            try
            {
                var createdClientProject = await _clientProjectService.CreateClientProject(clientProjectsDto);
                return Ok(createdClientProject);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClientProject(ClientProjectsDto clientProjectsDto)
        {
            try
            {
                var clientProjectObject = await _clientProjectService.UpdateClientProject(clientProjectsDto);
                return Ok(clientProjectObject);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClientProject(int id)
        {
            var clientProject = await _clientProjectService.GetClientProjectById(id);

            if (clientProject == null)
            {
                return NotFound("Client Project not found");
            }

            var clientProjectObject = await _clientProjectService.DeleteClientProject(id);
            return Ok(clientProjectObject);
        }
    }
}
