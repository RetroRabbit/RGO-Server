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

        Task<MonthlyEmployeeTotalDto> GetEmployeePreviousMonthTotal();

        /// <summary>
        ///     Calculates employee chrunRate over a month
        /// </summary>
        /// <returns>ChurnRateDataCard</returns>
        Task<ChurnRateDataCardDto> CalculateEmployeeChurnRate();

        Task<double> CalculateEmployeeGrowthRate();
    }
}
