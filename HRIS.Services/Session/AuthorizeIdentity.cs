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
        _userIdentity = httpAccessor?.HttpContext?.User;
    }

    private string? _email;
    private string? _nameIdentifier;
    private string? _role;
    private int? _employeeId;

    public virtual string Email => _email ??= GetEmail();
    public virtual string NameIdentifier => _nameIdentifier ??= GetNameIdentifier();
    public virtual string Role => _role ??= GetRole();

    public bool IsAdmin => Role is "Admin" or "SuperAdmin";
    public bool IsTalent => Role is "Talent";
    public bool IsJourney => Role is "Journey";
    public bool IsEmployee => Role is "Employee";
    public virtual bool IsInactive => Role is "Inactive";
    public bool IsSupport => IsAdmin || IsTalent || IsJourney || IsInactive;

    public virtual int EmployeeId
    {
        get
        {
            _employeeId ??= _db.GetActiveEmployeeId(Email).Result;
            return _employeeId ?? throw new CustomException("Unauthorized Access");
        }
    }

    private string GetEmail()
    {
        return GetClaimValue(ClaimTypes.Email) ?? throw new CustomException("Email claim is missing.");
    }

    private string GetNameIdentifier()
    {
        return GetClaimValue(ClaimTypes.NameIdentifier) ?? throw new CustomException("NameIdentifier claim is missing.");
    }

    private string GetRole()
    {
        return GetClaimValue(ClaimTypes.Role) ?? "Inactive";
    }

    private string? GetClaimValue(string claimType)
    {
        if (_userIdentity == null || !_userIdentity.Identity.IsAuthenticated)
        {
            throw new AuthenticationException("User is not signed in.");
        }
        return _userIdentity.FindFirst(claimType)?.Value;
    }
}