using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HRIS.Models;

namespace RR.App.Controllers;

public abstract class RRController : ControllerBase
{
    internal AuthorizeIdentity GetIdentity()
    {
        return new AuthorizeIdentity
        {
            Email = GetEmail(),
            NameIdentifier = GetNameIdentifier(),
            Role = GetRole()
        };
    }

    internal string GetEmail()
    {
        return GetUserType(ClaimTypes.Email);
    }

    internal string GetNameIdentifier()
    {
        return GetUserType(ClaimTypes.NameIdentifier);
    }

    internal string GetRole()
    {
        return GetUserType(ClaimTypes.Role);
    }

    internal string GetUserType(string type)
    {
        return (User.Identity as ClaimsIdentity)?.FindFirst(type)?.Value ?? throw new AuthenticationException("Not signed in");
    }
}