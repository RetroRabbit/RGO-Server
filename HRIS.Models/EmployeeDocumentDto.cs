using HRIS.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class EmployeeDocumentDto
{
    [Required(ErrorMessage = "Employee Document 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Employee Document 'EmployeeId' field is missing.")]
    public int EmployeeId { get; set; }
    public string? Reference { get; set; }
    [Required(ErrorMessage = "Employee Document 'FileName' field is missing.")]
    public string? FileName { get; set; }
    [Required(ErrorMessage = "Employee Document 'FileCategory' field is missing.")]
    public FileCategory FileCategory { get; set; }
    [Required(ErrorMessage = "Employee Document 'EmployeeFileCategory' field is missing.")]
    public EmployeeFileCategory EmployeeFileCategory { get; set; }
    [Required(ErrorMessage = "Employee Document 'AdminFileCategory' field is missing.")]
    public AdminFileCategory AdminFileCategory { get; set; }
    [Required(ErrorMessage = "Employee Document 'Blob' field is missing.")]
    public byte[] Blob { get; set; } = Array.Empty<byte>();
    [Required(ErrorMessage = "Employee Document 'Status' field is missing.")]
    public DocumentStatus? Status { get; set; }
    [Required(ErrorMessage = "Employee Document 'UploadDate' field is missing.")]
    [DataType(DataType.DateTime)]
    public DateTime UploadDate { get; set; }
    public string? Reason { get; set; }
    public bool CounterSign { get; set; }
    [Required(ErrorMessage = "Employee Document 'DocumentType' field is missing.")]
    public DocumentType? DocumentType { get; set; }
    [Required(ErrorMessage = "Employee Document 'LastUpdatedDate' field is missing.")]
    [DataType(DataType.DateTime)]
    public DateTime LastUpdatedDate { get; set; }
}
