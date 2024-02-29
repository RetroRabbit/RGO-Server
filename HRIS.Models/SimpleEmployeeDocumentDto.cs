using HRIS.Models.Enums;

namespace HRIS.Models;

public class SimpleEmployeeDocumentDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string FileName { get; set; }
    public FileCategory FileCategory { get; set; }
    public string Blob { get; set; }
    public DateTime UploadDate { get; set; }
}