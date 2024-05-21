using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.App.Controllers.HRIS;

[Route("client-project")]
[ApiController]

public class ClientProjectsController : ControllerBase
{
    private readonly IClientProjectService _clientProjectService;
    public ClientProjectsController(IClientProjectService clientProjectService)
    {
        _clientProjectService = clientProjectService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet]
    public async Task<ActionResult<ClientProjectsDto>> GetAllClientProjects()
    {
        try
        {
            var clientProjects = await _clientProjectService.GetAllClientProject();

            if (clientProjects == null) throw new Exception();

            return Ok(clientProjects);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ClientProjectsDto>> GetClientProject(int id)
    {
        var clientProjectDto = await _clientProjectService.GetClientProject(id);

        if (clientProjectDto == null)
        {
            return NotFound();
        }

        return Ok(clientProjectDto);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPost]
    public async Task<ActionResult<ClientProjectsDto>> PostClientProject(ClientProjectsDto clientProjectsDto)
    {
        var createdClientProject = await _clientProjectService.CreateClientProject(clientProjectsDto);

        if (createdClientProject == null)
        {
            return BadRequest("Unable to create the project.");
        }
        return CreatedAtAction(nameof(GetClientProject), new { id = createdClientProject.Id }, createdClientProject);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut]
    public async Task<IActionResult> PutClientProject(int id, ClientProjectsDto clientProjectsDto)
    {
        if (id != clientProjectsDto.Id)
        {
            return BadRequest("Mismatched project ID.");
        }

        try
        {
            var clientProjectObject = await _clientProjectService.UpdateClientProject(clientProjectsDto);
            return Ok(clientProjectObject);
        }
        catch
        {
            return StatusCode(500, "An error occurred while updating the project.");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteClientProject(int id)
    {
            var clientProject = await _clientProjectService.GetClientProject(id);
            if (clientProject == null)
            {
                return NotFound("Project not found");
            }

            var clientProjectObject =  await _clientProjectService.DeleteClientProject(id);
            return Ok(clientProjectObject);
    }
}
