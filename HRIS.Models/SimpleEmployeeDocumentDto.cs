using HRIS.Models.Enums;

namespace HRIS.Models;

public class SimpleEmployeeDocumentDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string FileName { get; set; }
    public FileCategory FileCategory { get; set; }
    public int EmployeeFileCategory { get; set; }
    public string Blob { get; set; }
    public DateTime UploadDate { get; set; }
    public string? Reference { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
}
