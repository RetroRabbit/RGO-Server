﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;
using System.Security.Claims;

namespace RGO.App.Controllers;

[Route("/auth/")]
[ApiController]
public class AuthenticationController : ControllerBase
{

    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromQuery] string email)
    {
        try
        {
            bool userExists = await _authService.CheckUserExist(email);

            if (!userExists) throw new Exception("User not found");


            string token = await _authService.Login(email);

            return Ok(token);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeDto newEmployee)
    {
        try
        {
            bool userExists = await _authService.CheckUserExist(newEmployee.PersonalEmail);

            if (userExists) throw new Exception("Employee already exists");

            string token = await _authService.RegisterEmployee(newEmployee);

            return Ok(token);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("roles")]
    public async Task<IActionResult> GetUserRoles()
    {
        try
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            var email = claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email)) return Unauthorized("Token is missing claim(s)[e.g: email]");

            List<AuthRoleResult> roles = await _authService.GetUserRoles(email);

            return Ok(roles);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}