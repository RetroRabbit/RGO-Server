﻿using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpGet]
        public async Task<ActionResult> GetAllClientProjects()
        {
            try
            {
                var clientProjects = await _clientProjectService.GetAllClientProject();
                return Ok(clientProjects);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpGet("all")]
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
        public async Task<IActionResult> PostClientProject(ClientProject clientProject)
        {
            var createdClientProject = await _clientProjectService.CreateClientProject(clientProject);

            if (createdClientProject == null)
            {
                return BadRequest("Unable to create the project.");
            }

            return CreatedAtAction(nameof(GetClientProject), new { id = createdClientProject.Id }, createdClientProject);
        }

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpPut]
        public async Task<IActionResult> PutClientProject(int id, ClientProject clientProject)
        {
            if (id != clientProject.Id)
            {
                return BadRequest("Mismatched project ID.");
            }

            try
            {
                var clientProjectObject = await _clientProjectService.UpdateClientProject(clientProject);
                return Ok(clientProjectObject);
            }
            catch
            {
                return StatusCode(500, "An error occurred while updating the project.");
            }
        }

        [Authorize(Policy = "AllRolesPolicy")]
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
}
