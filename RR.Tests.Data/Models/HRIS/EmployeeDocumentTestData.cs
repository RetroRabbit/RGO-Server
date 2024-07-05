using HRIS.Models.Enums;
using HRIS.Models;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork.Entities.HRIS;

namespace RGO.Tests.Data.Models;

public class EmployeeDocumentTestData
{
    public static EmployeeDocument EmployeeDocumentPending = new()
    {
        Id = 1,
        EmployeeId = EmployeeTestData.EmployeeOne.Id,
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

    public static EmployeeDocument EmployeeDocumentApproved = new()
    {
        Id = 2,
        EmployeeId = EmployeeTestData.EmployeeOne.Id,
        Reference = null,
        FileName = "TestFile.pdf",
        FileCategory = FileCategory.EmploymentContract,
        Blob = "TestFileContent",
        Status = DocumentStatus.Approved,
        UploadDate = DateTime.Now,
        Reason = null,
        CounterSign = false,
        DocumentType = DocumentType.StarterKit,
        LastUpdatedDate = DateTime.Now,
        Employee = EmployeeTestData.EmployeeOne
    };

    public static EmployeeDocument EmployeeDocumentRejected = new()
    {
        Id = 3,
        EmployeeId = EmployeeTestData.EmployeeOne.Id,
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

    public static EmployeeDocument EmployeeDocumentActionRequired = new()
    {
        Id = 4,
        EmployeeId = EmployeeTestData.EmployeeOne.Id,
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

    public static SimpleEmployeeDocumentDto SimpleDocumentDto = new()
    {
        Id = 1,
        EmployeeId = EmployeeTestData.EmployeeOne.Id,
        FileName = "TestFile.pdf",
        FileCategory = FileCategory.EmploymentContract,
        Blob = "TestFileContent",
        UploadDate = DateTime.Now,
        Reference = null,
        LastUpdatedDate = DateTime.Now
    };

    public static SimpleEmployeeDocumentGetAllDto SimpleGetAllDto = new()
    {
        EmployeeDocumentDto = EmployeeDocumentPending.ToDto(),
        Name = EmployeeTestData.EmployeeOne.Name,
        Surname = EmployeeTestData.EmployeeOne.Surname,
    };
}
