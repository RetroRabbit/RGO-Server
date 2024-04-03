using HRIS.Models.Enums;

namespace HRIS.Models;

public class AuditLogDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public CRUDOperations CRUDOperation { get; set; }
    public EmployeeDto? CreatedBy { get; set; }
    public string? Table { get; set; }
    public string? Data { get; set; }
}