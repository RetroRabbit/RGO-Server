using HRIS.Models.EmployeeProfileModels;

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
}
