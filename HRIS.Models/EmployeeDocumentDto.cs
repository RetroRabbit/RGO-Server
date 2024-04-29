using HRIS.Models.Enums;

namespace HRIS.Models;

public class EmployeeDocumentDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string? Reference { get; set; }
    public string? FileName { get; set; }
    public FileCategory FileCategory { get; set; }
    public string? Blob { get; set; }
    public DocumentStatus? Status { get; set; }
    public DateTime UploadDate { get; set; }
    public string? Reason { get; set; }
    public bool CounterSign { get; set; }
    public DateTime LastUpdatedDate { get; set; }
}
