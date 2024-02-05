using RGO.Models.Enums;

namespace RGO.Models;

public class SimpleEmployeeDocumentDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public SimpleEmployeeDocumentDto(int Id,
        int EmployeeId,
        string FileName,
        FileCategory FileCategory,
        string File,
        DateTime UploadDate)
    {
        this.Id = Id;
        this.EmployeeId = EmployeeId;
        this.FileName = FileName;
        this.FileCategory = FileCategory;
        this.File = File;
        this.UploadDate = UploadDate;
    }

    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string FileName { get; set; }
    public FileCategory FileCategory { get; set; }
    public string File { get; set; }
    public DateTime UploadDate { get; set; }
}
