using HRIS.Models.Employee.Commons;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Interfaces;

public interface IEmployeeBankingService
{
    /// <summary>
    ///     Fetches all banking entries by approval status
    /// </summary>
    /// <param name="approvalStatus"></param>
    /// <returns>List of pending EmployeeBanking objects</returns>
    Task<List<EmployeeBanking>> Get(int approvalStatus);

    /// <summary>
    ///     Delete Employee Banking
    /// </summary>
    /// <param name="employeeBankingDto"></param>
    /// <returns>Employee Banking</returns>
    Task<EmployeeBankingDto> Delete(int addressId);

    /// <summary>
    ///     Updates a banking entry
    /// </summary>
    /// <param name="newEntry"></param>
    /// <returns></returns>
    Task<EmployeeBankingDto> Update(EmployeeBankingDto newEntry);

    /// <summary>
    ///     Fetch banking of Employee
    /// </summary>
    /// <param id="employeeId"></param>
    /// <returns>EmployeeBankingDto</returns>
    Task<List<EmployeeBankingDto>> GetBankingById(int id);

    /// <summary>
    ///     Save a new EmployeeBankingDto for an Employee
    /// </summary>
    /// <param name="newEntry"></param>
    /// <returns>EmployeeBankingDto</returns>
    Task<EmployeeBankingDto> Create(EmployeeBankingDto newEntry);

    /// <summary>
    ///     Check if user exist
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Boolean to check if the employee qualification exists</returns>
    Task<bool> EmployeeBankingDetailsExist(int id);
}