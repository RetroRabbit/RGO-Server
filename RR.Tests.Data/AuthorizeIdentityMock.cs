using HRIS.Services.Session;
using Microsoft.AspNetCore.Http;
using RR.UnitOfWork;

namespace RR.Tests.Data;

public class AuthorizeIdentityMock : AuthorizeIdentity
{
    public AuthorizeIdentityMock() 
        : this(null, null)
    {

    }

    public AuthorizeIdentityMock(string email, string nameIdentifier, string role, int employeeId) 
        : this()
    {
        Email = email;
        NameIdentifier = nameIdentifier;
        Role = role;
        EmployeeId = employeeId;
    }
    public AuthorizeIdentityMock(int employeeId)
    : this()
    {
       EmployeeId = employeeId;
    }
    public AuthorizeIdentityMock(IUnitOfWork db, IHttpContextAccessor httpAccessor) 
        : base(db, httpAccessor)
    {
    }

    public override string Email { get; }
    public override string NameIdentifier { get; }
    public override string Role { get; }
    public override int EmployeeId { get; }
}