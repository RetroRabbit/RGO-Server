using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IEmployeeService
{
    /// <summary>
    ///     Check if user exist
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<bool> CheckUserExist(string email);

    /// <summary>
    ///     Get all employees
    /// </summary>
    /// <returns></returns>
    Task<List<EmployeeDto>> GetAll(string userEmail = "");

    /// <summary>
    ///     Get employee by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns>EmployeeDto</returns>
    Task<EmployeeDto> GetEmployee(string email);

    /// <summary>
    ///     Get employee by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>An employee dto including type and addresses</returns>
    Task<EmployeeDto> GetEmployeeById(int id);

    /// <summary>
    ///     Save employee
    /// </summary>
    /// <param name="employeeDto"></param>
    /// <returns>EmployeeDto</returns>
    Task<EmployeeDto> SaveEmployee(EmployeeDto employeeDto);

    /// <summary>
    ///     Update employee
    /// </summary>
    /// <param name="employeeDto"></param>
    /// <param name="email"></param>
    /// <returns>EmployeeDto</returns>
    Task<EmployeeDto> UpdateEmployee(EmployeeDto employeeDto, string email);

    /// <summary>
    ///     Delete employee
    /// </summary>
    /// <param name="email"></param>
    /// <returns>EmployeeDto</returns>
    Task<EmployeeDto> DeleteEmployee(string email);

    /// <summary>
    ///     Get employee by the id
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns>EmployeeDto</returns>
    Task<EmployeeDto> GetById(int employeeId);

    /// <summary>
    ///     Get simple employee profile for non admin users
    /// </summary>
    /// <returns>SimpeEmployeeProfileDto</returns>
    Task<SimpleEmployeeProfileDto> GetSimpleProfile(string employeeEmail);

    /// <summary>
    ///     Returns Employees data count
    /// </summary>
    /// <returns>EmployeeDataCard</returns>
    Task<EmployeeCountDataCard> GenerateDataCardInformation();

    /// <summary>
    ///     Get the total number of employees for the current month
    /// </summary>
    /// <returns>MonthlyEmployeeTotalDto</returns>
    Task<MonthlyEmployeeTotalDto> GetEmployeeCurrentMonthTotal();

    /// <summary>
    ///     Calculates employee chrunRate over a month
    /// </summary>
    /// <returns>ChurnRateDataCard</returns>
    Task<ChurnRateDataCard> CalculateEmployeeChurnRate();

    /// <summary>
    ///     Get Employees filtered by Peoples champion or employee type
    /// </summary>
    /// <param name="peopleChampId"></param>
    /// <param name="employeeType"></param>
    /// <returns>
    ///     Filtered list of Employees based on assigned Peoples Champion or Employee Type if 0 is passed as parameter it
    ///     will ignore the filter
    /// </returns>
    Task<List<EmployeeDto>> FillerEmployees(int peopleChampId = 0, int employeeType = 0);
}