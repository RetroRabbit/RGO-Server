using RGO.Models;

namespace RGO.Services.Interfaces
{
    public interface IEmployeeAddressService
    {
        /// <summary>
        /// Save Employee Address
        /// </summary>
        /// <param name="employeeAddressDto"></param>
        /// <returns>Employee Address</returns>
        Task<EmployeeAddressDto> SaveEmployeeAddress(EmployeeAddressDto employeeAddressDto);

        /// <summary>
        /// Get Employee Address
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>Employee Address</returns>
        Task<EmployeeAddressDto> GetEmployeeAddress(int employeeId);

        /// <summary>
        /// Update Employee Address
        /// </summary>
        /// <param name="employeeAddressDto"></param>
        /// <returns>Employee Address</returns>
        Task<EmployeeAddressDto> UpdateEmployeeAddress(EmployeeAddressDto employeeAddressDto);

        /// <summary>
        /// Delete Employee Address
        /// </summary>
        /// <param name="employeeAddressDto"></param>
        /// <returns>Employee Address</returns>
        Task<EmployeeAddressDto> DeleteEmployeeAddress(EmployeeAddressDto employeeAddressDto);
    }
}
