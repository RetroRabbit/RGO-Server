using HRIS.Models.Employee.Commons;
using HRIS.Models.Employee.Profile;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Interfaces;

public interface IEmployeeProfileService
{
    /// <summary>
    ///     Get employee profile details by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ProfileDetailsDto> GetEmployeeProfileDetailsById(int? id);

    /// <summary>
    ///     Get employee career summary by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<CareerSummaryDto> GetEmployeeCareerSummaryById(int? id);

    /// <summary>
    ///     Get employee career summary by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BankingInformationDto> GetEmployeeBankingInformationById(int? id);

    /// <summary>
    ///     Get employee career summary by id
    /// </summary>
    /// <param name="employeeDetails"></param>
    /// <returns></returns>
    Task<Employee> UpdateEmployeeDetails(EmployeeDetailsDto employeeDetails);
}
