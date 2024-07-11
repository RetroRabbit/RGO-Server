using HRIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        EmployeeCountByRoleDataCard GetEmployeeCountTotalByRole();
        /// <summary>
        ///     Get the total number of employees on bench
        /// </summary>
        /// <returns>MonthlyEmployeeTotalDto</returns>
        Task<EmployeeOnBenchDataCard> GetTotalNumberOfEmployeesOnBench();
        /// <summary>
        ///     Get the total number of employees onn client
        /// <returns>MonthlyEmployeeTotalDto</returns>
        int GetTotalNumberOfEmployeesOnClients();

        /// <summary>
        ///     Calculates employee chrunRate over a month
        /// </summary>
        /// <returns>ChurnRateDataCard</returns>
        Task<ChurnRateDataCardDto> CalculateEmployeeChurnRate();

        Task<double> CalculateEmployeeGrowthRate();
    }
}
