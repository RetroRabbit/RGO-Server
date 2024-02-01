namespace RGO.Models;

public class AuditLogDto
{
    public int Id { get; set; }
    public EmployeeDto? EditFor { get; set; }
    public EmployeeDto? EditBy { get; set; }
    public DateTime EditDate { get; set; }
    public string Description { get; set; }
}