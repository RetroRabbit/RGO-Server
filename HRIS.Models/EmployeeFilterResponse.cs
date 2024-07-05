using HRIS.Models.Enums;

namespace HRIS.Models;

public class EmployeeFilterResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public int? Level { get; set; }
    public string? ClientAllocated { get; set; }
    public int RoleId { get; set; }
    public string RoleDescription { get; set; } = string.Empty;
    public DateTime? EngagementDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public string? InactiveReason { get; set; }
    public string? Position { get; set; }
}