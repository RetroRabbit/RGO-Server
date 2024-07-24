using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IEmployeeAddressService
{
    /// <summary>
    ///     Check if Employee Address Exists
    /// </summary>
    /// <param name="employeeAddressDto"></param>
    /// <returns>True or False</returns>
    Task<bool> CheckIfExists(int id);

    /// <summary>
    ///     Save Employee Address
    /// </summary>
    /// <param name="employeeAddressDto"></param>
    /// <returns>Employee Address</returns>
    Task<EmployeeAddressDto> Create(EmployeeAddressDto employeeAddressDto);

    /// <summary>
    ///     Get Employee Address
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns>Employee Address</returns>
    Task<EmployeeAddressDto> Get(int id);

    /// <summary>
    ///     Update Employee Address
    /// </summary>
    /// <param name="employeeAddressDto"></param>
    /// <returns>Employee Address</returns>
    Task<EmployeeAddressDto> Update(EmployeeAddressDto employeeAddressDto);

    /// <summary>
    ///     Delete Employee Address
    /// </summary>
    /// <param name="employeeAddressDto"></param>
    /// <returns>Employee Address</returns>
    Task<EmployeeAddressDto> Delete(int addressId);

    /// <summary>
    ///     Get all Employees
    /// </summary>
    /// <param></param>
    /// <returns>List of all EmployeesDto</returns>
    Task<List<EmployeeAddressDto>> GetAll();
}