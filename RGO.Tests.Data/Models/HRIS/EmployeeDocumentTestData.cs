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
        FileCategory = FileCategory.FixedTerm,
        Blob = "TestFileContent",
        Status = DocumentStatus.ActionRequired,
        UploadDate = DateTime.Now,
        Reason = null,
        CounterSign = false
    };

    public static EmployeeDocumentDto EmployeeDocumentApproved = new EmployeeDocumentDto
    {
        Id = 1,
        EmployeeId = EmployeeTestData.EmployeeDto.Id,
        Reference = null,
        FileName = "TestFile.pdf",
        FileCategory = FileCategory.FixedTerm,
        Blob = "TestFileContent",
        Status = DocumentStatus.Approved,
        UploadDate = DateTime.Now,
        Reason = null,
        CounterSign = false
    };

    public static EmployeeDocumentDto EmployeeDocumentRejected = new EmployeeDocumentDto
    {
        Id = 1,
        EmployeeId = EmployeeTestData.EmployeeDto.Id,
        Reference = null,
        FileName = "TestFile.pdf",
        FileCategory = FileCategory.FixedTerm,
        Blob = "TestFileContent",
        Status = DocumentStatus.Rejected,
        UploadDate = DateTime.Now,
        Reason = null,
        CounterSign = false
    };

    public static EmployeeDocumentDto EmployeeDocumentActionRequired = new EmployeeDocumentDto
    {
        Id = 1,
        EmployeeId = EmployeeTestData.EmployeeDto.Id,
        Reference = null,
        FileName = "TestFile.pdf",
        FileCategory = FileCategory.FixedTerm,
        Blob = "TestFileContent",
        Status = DocumentStatus.ActionRequired,
        UploadDate = DateTime.Now,
        Reason = null,
        CounterSign = false
    };
    public static SimpleEmployeeDocumentDto SimpleDocumentDto = new SimpleEmployeeDocumentDto(
           Id: 1, EmployeeId: EmployeeTestData.EmployeeDto.Id,
           FileName: "TestFile.pdf", FileCategory: FileCategory.FixedTerm,
           Blob: "TestFileContent", UploadDate: DateTime.Now);
}
