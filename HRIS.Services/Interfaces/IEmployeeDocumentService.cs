using HRIS.Models;
using HRIS.Models.Enums;

namespace HRIS.Services.Interfaces;

public interface IEmployeeDocumentService
{
    /// <summary>
    /// Save Employee Document
    /// </summary>
    /// <param name="employeeDocumentDto"></param>
    /// <returns>Employee Document</returns>
    Task<EmployeeDocumentDto> SaveEmployeeDocument(SimpleEmployeeDocumentDto employeeDocumentDto, string email, int documentType);

    /// <summary>
    /// Add a new additional Document
    /// </summary>
    /// <param name="employeeDocumentDto"></param>
    /// <returns>Employee Document</returns>
    Task<EmployeeDocumentDto> addNewAdditionalDocument(SimpleEmployeeDocumentDto employeeDocDto, string email, int documentType);

    /// <summary>
    /// Get Employee Document
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="filename"></param>
    /// <returns>Employee Document</returns>
    /// 
    Task<EmployeeDocumentDto> GetEmployeeDocument(int employeeId, string filename, DocumentType documentType);

    /// <summary>
    /// Get All Employee Documents
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns>List of Employee Document</returns>
    Task<List<EmployeeDocumentDto>> GetAllEmployeeDocuments(int employeeId, DocumentType documentType);

    /// <summary>
    /// Update Employee Document
    /// </summary>
    /// <param name="employeeDocumentDto"></param>
    /// <returns>Employee Document</returns>
    Task<EmployeeDocumentDto> UpdateEmployeeDocument(EmployeeDocumentDto employeeDocumentDto);

    /// <summary>
    /// Delete Employee Document
    /// </summary>
    /// <param name="employeeDocumentDto"></param>
    /// <returns>Employee Document</returns>
    Task<EmployeeDocumentDto> DeleteEmployeeDocument(int documentId);

    /// <summary>
    /// Get List of Employee Document
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="status"></param>
    /// <returns>Employee Document</returns>
    Task<List<EmployeeDocumentDto>> GetEmployeeDocumentsByStatus(int employeeId, DocumentStatus status);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<SimpleEmployeeDocumentGetAllDto>> GetAllDocuments();

    Task<bool> CheckEmployee(int employeeId);

    Task<bool> IsAdmin(string email);
}