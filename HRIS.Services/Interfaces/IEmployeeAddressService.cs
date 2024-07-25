using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IEmployeeAddressService
{
    /// <summary>
    /// Check if Employee Address Exists
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> CheckIfExists(int id);

    /// <summary>
    ///     Create Employee Address
    /// </summary>
    /// <param name="employeeAddressDto"></param>
    /// <returns>Employee Address</returns>
    Task<EmployeeAddressDto> Create(EmployeeAddressDto employeeAddressDto);

    /// <summary>
    /// Get Employee Address
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<EmployeeAddressDto> GetById(int id);

    /// <summary>
    ///     Update Employee Address
    /// </summary>
    /// <param name="employeeAddressDto"></param>
    /// <returns>Employee Address</returns>
    Task<EmployeeAddressDto> Update(EmployeeAddressDto employeeAddressDto);

    /// <summary>
    /// Delete Employee Address
    /// </summary>
    /// <param name="addressId"></param>
    /// <returns></returns>
    Task<EmployeeAddressDto> Delete(int addressId);

    /// <summary>
    ///     Get all Employees
    /// </summary>
    /// <param></param>
    /// <returns>List of all EmployeesDto</returns>
    Task<List<EmployeeAddressDto>> GetAll();
}