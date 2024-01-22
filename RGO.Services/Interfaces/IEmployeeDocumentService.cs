using RGO.Models;
using RGO.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Interfaces
{
    public interface IEmployeeDocumentService
    {
        /// <summary>
        /// Save Employee Document
        /// </summary>
        /// <param name="employeeDocumentDto"></param>
        /// <returns>Employee Document</returns>
        Task<EmployeeDocumentDto> SaveEmployeeDocument(SimpleEmployeeDocumentDto employeeDocumentDto);

        /// <summary>
        /// Get Employee Document
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="filename"></param>
        /// <returns>Employee Document</returns>
        Task<EmployeeDocumentDto> GetEmployeeDocument(int employeeId,string filename);

        /// <summary>
        /// Get All Employee Documents
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>List of Employee Document</returns>
        Task<List<EmployeeDocumentDto>> GetAllEmployeeDocuments(int employeeId);

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
        Task<EmployeeDocumentDto> DeleteEmployeeDocument(EmployeeDocumentDto employeeDocumentDto);

        /// <summary>
        /// Get Employee Document
        /// </summary>
        /// <param name="status"></param>
        /// <returns>Employee Document</returns>
        Task<List<EmployeeDocumentDto>> GetStatusEmployeeDocument(int employeeId, DocumentStatus status);
    }
}
