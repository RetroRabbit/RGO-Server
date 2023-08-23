using RGO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Interfaces
{
    public interface IEmployeeDocumentService
    {
        Task SaveEmployeeDocument(EmployeeDocumentDto employeeDocumentDto);
        Task<EmployeeDocumentDto> GetEmployeeDocument(int employeeId);
        Task UpdateEmployeeDocument(EmployeeDocumentDto employeeDocumentDto);
        Task DeleteEmployeeDocument(EmployeeDocumentDto employeeDocumentDto);
    }
}
