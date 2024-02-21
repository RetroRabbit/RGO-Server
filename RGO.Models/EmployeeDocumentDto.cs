using HRIS.Models.Enums;

namespace HRIS.Models;

public class EmployeeDocumentDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeDocumentDto(int Id,
                               int EmployeeId,
                               string? Reference,
                               string? FileName,
                               FileCategory FileCategory,
                               string? Blob,
                               DocumentStatus? Status,
                               DateTime UploadDate,
                               string? Reason,
                               bool CounterSign)
    {
        this.Id = Id;
        this.EmployeeId = EmployeeId;
        this.Reference = Reference;
        this.FileName = FileName;
        this.FileCategory = FileCategory;
        this.Blob = Blob;
        this.Status = Status;
        this.UploadDate = UploadDate;
        this.Reason = Reason;
        this.CounterSign = CounterSign;
    }

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
}