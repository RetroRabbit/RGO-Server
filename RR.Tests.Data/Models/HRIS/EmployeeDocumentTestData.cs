using HRIS.Models.Enums;
using HRIS.Models;
using RR.Tests.Data.Models.HRIS;

namespace RGO.Tests.Data.Models;

public class EmployeeDocumentTestData
{
    public static EmployeeDocumentDto EmployeeDocumentPending = new EmployeeDocumentDto
    {
        Id = 1,
        EmployeeId = EmployeeTestData.EmployeeDto.Id,
        Reference = null,
        FileName = "TestFile.pdf",
        FileCategory = FileCategory.EmploymentContract,
        Blob = "TestFileContent",
        Status = DocumentStatus.ActionRequired,
        UploadDate = DateTime.Now,
        Reason = null,
        CounterSign = false,
        DocumentType = DocumentType.StarterKit,
        LastUpdatedDate = DateTime.Now
    };

    public static EmployeeDocumentDto EmployeeDocumentApproved = new EmployeeDocumentDto
    {
        Id = 2,
        EmployeeId = EmployeeTestData.EmployeeDto.Id,
        Reference = null,
        FileName = "TestFile.pdf",
        FileCategory = FileCategory.EmploymentContract,
        Blob = "TestFileContent",
        Status = DocumentStatus.Approved,
        UploadDate = DateTime.Now,
        Reason = null,
        CounterSign = false,
        DocumentType = DocumentType.StarterKit,
        LastUpdatedDate = DateTime.Now
    };

    public static EmployeeDocumentDto EmployeeDocumentRejected = new EmployeeDocumentDto
    {
        Id = 3,
        EmployeeId = EmployeeTestData.EmployeeDto.Id,
        Reference = null,
        FileName = "TestFile.pdf",
        FileCategory = FileCategory.EmploymentContract,
        Blob = "TestFileContent",
        Status = DocumentStatus.Rejected,
        UploadDate = DateTime.Now,
        Reason = null,
        CounterSign = false,
        DocumentType = DocumentType.StarterKit,
        LastUpdatedDate = DateTime.Now
    };

    public static EmployeeDocumentDto EmployeeDocumentActionRequired = new EmployeeDocumentDto
    {
        Id = 4,
        EmployeeId = EmployeeTestData.EmployeeDto.Id,
        Reference = null,
        FileName = "TestFile.pdf",
        FileCategory = FileCategory.EmploymentContract,
        Blob = "TestFileContent",
        Status = DocumentStatus.ActionRequired,
        UploadDate = DateTime.Now,
        Reason = null,
        CounterSign = false,
        DocumentType = DocumentType.StarterKit,
    };

    public static SimpleEmployeeDocumentDto SimpleDocumentDto = new SimpleEmployeeDocumentDto
    {
        Id = 1,
        EmployeeId = EmployeeTestData.EmployeeDto.Id,
        FileName = "TestFile.pdf",
        FileCategory = FileCategory.EmploymentContract,
        Blob = "TestFileContent",
        UploadDate = DateTime.Now,
        Reference = null,
        LastUpdatedDate = DateTime.Now
    };
}
