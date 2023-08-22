using RGO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Interfaces
{
    public interface IEmployeeAddressService
    {
        Task SaveEmployeeAddress(EmployeeAddressDto employeeAddressDto);
        Task<EmployeeAddressDto> GetEmployeeAddress(int employeeId);
        Task UpdateEmployeeAddress(EmployeeAddressDto employeeAddressDto);
        Task DeleteEmployeeAddress(EmployeeAddressDto employeeAddressDto);
    }
}
