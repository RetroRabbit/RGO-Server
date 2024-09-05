using HRIS.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class SimpleEmployeeDocumentDto
{
    [Required(ErrorMessage = "Simple Employee Document 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Simple Employee Document 'EmployeeId' field is missing.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Simple Employee Document 'FileName' field is missing.")]
    public string FileName { get; set; }
    [Required(ErrorMessage = "Simple Employee Document 'FileCategory' field is missing.")]
    public FileCategory FileCategory { get; set; }
    [Required(ErrorMessage = "Simple Employee Document 'EmployeeFileCategory' field is missing.")]
    public int EmployeeFileCategory { get; set; }
    [Required(ErrorMessage = "Simple Employee Document 'AdminFileCategory' field is missing.")]
    public int AdminFileCategory { get; set; }
    [Required(ErrorMessage = "Simple Employee Document 'Blob' field is missing.")]
    public byte[] Blob { get; set; } = Array.Empty<byte>();
    [Required(ErrorMessage = "Simple Employee Document 'UploadDate' field is missing.")]
    public DateTime UploadDate { get; set; }
    public string? Reference { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
}
