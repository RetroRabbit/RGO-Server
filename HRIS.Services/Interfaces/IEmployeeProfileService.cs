using HRIS.Models.EmployeeProfileModels;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Interfaces;

public interface IEmployeeProfileService
{
    /// <summary>
    ///     Get employee profile details by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<EmployeeProfileDetailsDto> GetEmployeeProfileDetailsById(int? id);

    /// <summary>
    ///     Get employee career summary by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<EmployeeProfileCareerSummaryDto> GetEmployeeCareerSummaryById(int? id);

    /// <summary>
    ///     Get employee career summary by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<EmployeeProfileBankingInformationDto> GetEmployeeBankingInformationById(int? id);

    /// <summary>
    ///     Get employee career summary by id
    /// </summary>
    /// <param name="employeeDetails"></param>
    /// <returns></returns>
    Task<Employee> UpdateEmployeeDetails(EmployeeDetailsDto employeeDetails);
}
