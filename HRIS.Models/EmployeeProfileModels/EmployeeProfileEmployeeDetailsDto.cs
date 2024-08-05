namespace HRIS.Models.EmployeeProfileModels;

public class EmployeeProfileEmployeeDetailsDto
{
    public int Id { get; set; }
    public DateTime EngagementDate { get; set; }
    public int? PeopleChampionId { get; set; }
    public string? PeopleChampionName { get; set; }
    public int? Level { get; set; }
    public EmployeeTypeDto? EmployeeType { get; set; }
    public string? Name { get; set; }
    public string? Initials { get; set; }
    public string? Surname { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? IdNumber { get; set; }
    public int? ClientAllocatedId { get; set; }
    public string? ClientAllocatedName { get; set; }
    public int? TeamLeadId { get; set; }
    public string? TeamLeadName { get; set; }
}
