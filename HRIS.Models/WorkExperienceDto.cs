using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;
public class WorkExperienceDto
{
    [Required(ErrorMessage = "Work Experience 'Id' field is missing.")]
    public int Id { get; set; }
    public string? ClientName { get; set; }
    public string? ProjectName { get; set; }
    public List<string>? SkillSet { get; set; }
    public List<string>? Software { get; set; }
    [Required(ErrorMessage = "Work Experience 'EmployeeId' field is missing.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Work Experience 'StartDate' field is missing.")]
    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? ProjectDescription { get; set; }
}
