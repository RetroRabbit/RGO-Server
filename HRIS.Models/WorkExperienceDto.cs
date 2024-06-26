namespace HRIS.Models;
public class WorkExperienceDto
{
    public int Id { get; set; }
    public string? ClientName { get; set; }
    public string? ProjectName { get; set; }
    public List<string>? SkillSet { get; set; }
    public List<string>? Software { get; set; }
    public int EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? ProjectDescription { get; set; }
}
