using HRIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RR.Tests.Data.Models.HRIS
{
    public class MonthlyEmployeeTotalTestData
    {
        public static MonthlyEmployeeTotalDto monthlyEmployeeTotalDtoCurrentYearCurrentMonth = new MonthlyEmployeeTotalDto
        {
            Id = 1,
            EmployeeTotal = 1,
            DeveloperTotal = 1,
            ScrumMasterTotal = 0,
            BusinessSupportTotal = 0,
            Month = DateTime.Now.ToString("MMMM"),
            Year = DateTime.Now.Year
        };

        public static MonthlyEmployeeTotalDto monthlyEmployeeTotalDtoPreviuosMonthCurrentYear = new MonthlyEmployeeTotalDto
        {
            Id = 1,
            EmployeeTotal = 1,
            DeveloperTotal = 1,
            ScrumMasterTotal = 0,
            BusinessSupportTotal = 0,
            Month = DateTime.Now.AddMonths(-1).ToString("MMMM"),
            Year = DateTime.Now.Year
        };

        public static MonthlyEmployeeTotalDto monthlyEmployeeTotalDtoPreviuosMonthFurtureYear = new MonthlyEmployeeTotalDto
        {
            Id = 1,
            EmployeeTotal = 1,
            DeveloperTotal = 1,
            ScrumMasterTotal = 1,
            BusinessSupportTotal = 1,
            Month = DateTime.Now.AddMonths(-1).ToString("MMMM"),
            Year = 2032
        };

        public static MonthlyEmployeeTotalDto monthlyEmployeeTotalDtoMonthNovCurrentYear = new MonthlyEmployeeTotalDto
        {
            Id = 1,
            EmployeeTotal = 1,
            DeveloperTotal = 1,
            ScrumMasterTotal = 1,
            BusinessSupportTotal = 1,
            Month = "November",
            Year = DateTime.Now.Year
        };
    }
}
