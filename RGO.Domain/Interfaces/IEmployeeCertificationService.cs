using RGO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Interfaces
{
    public interface IEmployeeCertificationService
    {
        Task SaveEmployeeCertification(EmployeeCertificationDto employeeCertificationDto);
        Task<EmployeeCertificationDto> GetEmployeeCertification(int employeeId);
        Task UpdateEmployeeCertification(EmployeeCertificationDto employeeCertificationDto);
        Task DeleteEmployeeCertification(EmployeeCertificationDto employeeCertificationDto);
    }
}
