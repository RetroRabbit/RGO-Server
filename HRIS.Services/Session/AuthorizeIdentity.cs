using System.Security.Authentication;
using System.Security.Claims;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Http;
using RR.UnitOfWork;

namespace HRIS.Services.Session;

public class AuthorizeIdentity
{
    private readonly IUnitOfWork _db;
    private readonly ClaimsPrincipal? _userIdentity;

    public AuthorizeIdentity(IUnitOfWork db, IHttpContextAccessor httpAccessor)
    {
        _db = db;
        _userIdentity = httpAccessor?.HttpContext?.User ?? null;
    }

    private string? _email;
    private string? _nameIdentifier;
    private string? _role;
    private int? _employeeId;

    public virtual string Email => _email ??= GetEmail();
    public virtual string NameIdentifier => _nameIdentifier ??= GetNameIdentifier();
    public virtual string Role => _role ??= GetRole();
    public virtual int EmployeeId
    {
        get
        {
            _employeeId ??= _db.GetActiveEmployeeId(Email, Role).Result;
            return _employeeId ?? throw new CustomException("Unauthorized Access");
        }
    }

    private string GetEmail()
    {
        return GetUserType(ClaimTypes.Email);
    }

    private string GetNameIdentifier()
    {
        return GetUserType(ClaimTypes.NameIdentifier);
    }

    private string GetRole()
    {
        return GetUserType(ClaimTypes.Role);
    }

    private string GetUserType(string type)
    {
        return _userIdentity.FindFirst(type)?.Value ?? throw new AuthenticationException("Not signed in");
    }
}