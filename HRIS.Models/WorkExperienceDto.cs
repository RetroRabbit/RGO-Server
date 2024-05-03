namespace HRIS.Models;
public class WorkExperienceDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? EmploymentType { get; set; }
    public string? CompanyName { get; set; }
    public string? Location { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
