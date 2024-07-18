using RR.UnitOfWork.Entities.HRIS;

namespace RR.Tests.Data.Models.HRIS
{
    public class MonthlyEmployeeTotalTestData
    {
        public static MonthlyEmployeeTotal MonthlyEmployeeTotal_CurrentYear_CurrentMonth = new()
        {
            Id = 1,
            EmployeeTotal = 1,
            DeveloperTotal = 1,
            ScrumMasterTotal = 0,
            BusinessSupportTotal = 0,
            Month = DateTime.Now.ToString("MMMM"),
            Year = DateTime.Now.Year
        };

        public static MonthlyEmployeeTotal MonthlyEmployeeTotal_PreviuosMonth_CurrentYear = new()
        {
            Id = 1,
            EmployeeTotal = 1,
            DeveloperTotal = 1,
            ScrumMasterTotal = 0,
            BusinessSupportTotal = 0,
            Month = DateTime.Now.AddMonths(-1).ToString("MMMM"),
            Year = DateTime.Now.Year
        };

        public static MonthlyEmployeeTotal MonthlyEmployeeTotal_PreviuosMonth_FurtureYear = new()
        {
            Id = 1,
            EmployeeTotal = 1,
            DeveloperTotal = 1,
            ScrumMasterTotal = 1,
            BusinessSupportTotal = 1,
            Month = DateTime.Now.AddMonths(-1).ToString("MMMM"),
            Year = 2032
        };

        public static MonthlyEmployeeTotal MonthlyEmployeeTotal_MonthNov_CurrentYear = new()
        {
            Id = 1,
            EmployeeTotal = 1,
            DeveloperTotal = 1,
            ScrumMasterTotal = 1,
            BusinessSupportTotal = 1,
            Month = "November",
            Year = DateTime.Now.Year
        };

        public static MonthlyEmployeeTotal MonthlyEmployeeTotal_PreviousMonth_Zero = new()
        {
            Id = 1,
            EmployeeTotal = 0,
            DeveloperTotal = 0,
            ScrumMasterTotal = 0,
            BusinessSupportTotal = 0,
            Month = DateTime.Now.AddMonths(-1).ToString("MMMM"),
            Year = DateTime.Now.Year
        };
    }
}
