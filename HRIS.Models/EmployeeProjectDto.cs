namespace HRIS.Models;

public class EmployeeProjectDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Client { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
