using System.Security.Authentication;
using System.Security.Claims;

namespace HRIS.Models;

public class AuthorizeIdentity
{
    public string Email { get; set; }

    public string NameIdentifier { get; set; }

    public string Role { get; set; }
}