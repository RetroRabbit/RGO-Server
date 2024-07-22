using HRIS.Models;

namespace HRIS.Services.Interfaces
{
    public interface IDashboardService
    {
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
        ///     Get the total number of employees for the previous month
        /// </summary>
        /// <returns>MonthlyEmployeeTotalDto</returns>
        Task<MonthlyEmployeeTotalDto> GetEmployeePreviousMonthTotal();
        /// <summary>
        ///     Get the total number of employees for roles
        /// </summary>
        /// <returns>MonthlyEmployeeTotalDto</returns>
        Task<EmployeeCountByRoleDataCard> GetEmployeeCountTotalByRole();
        /// <summary>
        ///     Get the total number of employees on bench
        /// </summary>
        /// <returns>MonthlyEmployeeTotalDto</returns>
        Task<EmployeeOnBenchDataCard> GetTotalNumberOfEmployeesOnBench();
        /// <summary>
        ///     Get the total number of employees onn client
        /// <returns>MonthlyEmployeeTotalDto</returns>
        Task<int> GetTotalNumberOfEmployeesOnClients();
        /// <summary>
        ///     Calculates employee chrunRate over a month
        /// </summary>
        /// <returns>ChurnRateDataCard</returns>
        Task<ChurnRateDataCardDto> CalculateEmployeeChurnRate();
        /// <summary>
        ///     Calculates employeegrowth rate
        /// </summary>
        /// <returns>ChurnRateDataCard</returns>
        Task<double> CalculateEmployeeGrowthRate();

        /// <summary>
        ///     Get Total Number of Active Employees
        /// </summary>
        /// <returns>List<Employee></returns>
        Task<List<EmployeeDto>> GetAllActiveEmployees();
    }
}
