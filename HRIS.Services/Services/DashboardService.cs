using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _db;
        private readonly IEmployeeAddressService _employeeAddressService;
        private readonly IEmployeeTypeService _employeeTypeService;
        private readonly IRoleService _roleService;
        private readonly IErrorLoggingService _errorLoggingService;
        private readonly IEmailService _emailService;

        public async Task<ChurnRateDataCardDto> CalculateEmployeeChurnRate()
        {
            var today = DateTime.Today;
            var twelveMonthsAgo = today.AddMonths(-12);

            var employeeData = await _db.Employee
                .Get()
                .Where(e => e.EngagementDate <= today && (e.TerminationDate == null || e.TerminationDate >= twelveMonthsAgo))
                .ToListAsync();

            var employeeStartOfPeriod = employeeData
                .Where(e => e.EngagementDate < twelveMonthsAgo && (e.TerminationDate == null || e.TerminationDate >= twelveMonthsAgo))
                .GroupBy(e => e.EmployeeType)
                .Select(g => new { EmployeeType = g.Key, Count = g.Count() })
                .ToList()
                .Cast<dynamic>()
                .ToList();

            var employeeEndOfPeriod = employeeData
                .Where(e => e.EngagementDate < today && (e.TerminationDate == null || e.TerminationDate >= today))
                .GroupBy(e => e.EmployeeType)
                .Select(g => new { EmployeeType = g.Key, Count = g.Count() })
                .ToList()
                .Cast<dynamic>()
                .ToList();

            var terminationsDuringPeriod = employeeData
                .Where(e => e.TerminationDate >= twelveMonthsAgo && e.TerminationDate < today)
                .GroupBy(e => e.EmployeeType)
                .Select(g => new { EmployeeType = g.Key, Count = g.Count() })
                .ToList()
                .Cast<dynamic>()
                .ToList();

            int GetCount(List<dynamic> data, int employeeType) => data.FirstOrDefault(x => x.EmployeeType == employeeType)?.Count ?? 0;

            var totalAtStartOfPeriod = employeeStartOfPeriod.Sum(x => x.Count);
            var totalTerminationsDuringPeriod = terminationsDuringPeriod.Sum(x => x.Count);

            return new ChurnRateDataCardDto
            {
                ChurnRate = CalculateChurnRate(totalAtStartOfPeriod, totalTerminationsDuringPeriod),
                DeveloperChurnRate = CalculateChurnRate(GetCount(employeeStartOfPeriod, 1), GetCount(terminationsDuringPeriod, 1)),
                DesignerChurnRate = CalculateChurnRate(GetCount(employeeStartOfPeriod, 2), GetCount(terminationsDuringPeriod, 2)),
                ScrumMasterChurnRate = CalculateChurnRate(GetCount(employeeStartOfPeriod, 3), GetCount(terminationsDuringPeriod, 3)),
                BusinessSupportChurnRate = CalculateChurnRate(GetCount(employeeStartOfPeriod, 4), GetCount(terminationsDuringPeriod, 4)),
                Month = twelveMonthsAgo.ToString("MMMM"),
                Year = twelveMonthsAgo.Year
            };
        }

        public async Task<double> CalculateEmployeeGrowthRate()
        {
            var currentMonthTotal = await GetEmployeeCurrentMonthTotal();
            var previousMonthTotal = await GetEmployeePreviousMonthTotal();

            if (previousMonthTotal == null || currentMonthTotal == null)
            {
                throw new Exception("Employee totals for current or previous month are not available");
            }

            int currentTotal = currentMonthTotal.EmployeeTotal;
            int previousTotal = previousMonthTotal.EmployeeTotal;

            if (previousTotal == 0)
            {
                return 0;
            }

            double growthRate = ((double)(currentTotal - previousTotal) / previousTotal) * 100;
            return Math.Round(growthRate, 2);
        }

        public async Task<EmployeeCountDataCard> GenerateDataCardInformation()
        {
            var employeeCountTotalsByRole = GetEmployeeCountTotalByRole();

            var totalNumberOfEmployeesOnBench = GetTotalNumberOfEmployeesOnBench();

            var totalNumberOfEmployeesOnClients = GetTotalNumberOfEmployeesOnClients();

            var totalNumberOfEmployeesDevsScrumsAndDesigners = totalNumberOfEmployeesOnBench.TotalNumberOfEmployeesOnBench
                                                               + totalNumberOfEmployeesOnClients;

            var billableEmployeesPercentage = totalNumberOfEmployeesDevsScrumsAndDesigners > 0
                ? (double)totalNumberOfEmployeesOnClients / totalNumberOfEmployeesDevsScrumsAndDesigners * 100
                : 0;

            var currentMonthTotal = await GetEmployeeCurrentMonthTotal();

            var previousMonthTotal = await GetEmployeePreviousMonthTotal();

            var employeeTotalDifference = currentMonthTotal.EmployeeTotal - previousMonthTotal.EmployeeTotal;
            var isIncrease = employeeTotalDifference > 0;

            return new EmployeeCountDataCard
            {
                DevsCount = employeeCountTotalsByRole.DevsCount,
                DesignersCount = employeeCountTotalsByRole.DesignersCount,
                ScrumMastersCount = employeeCountTotalsByRole.ScrumMastersCount,
                BusinessSupportCount = employeeCountTotalsByRole.BusinessSupportCount,
                DevsOnBenchCount = totalNumberOfEmployeesOnBench.DevsOnBenchCount,
                DesignersOnBenchCount = totalNumberOfEmployeesOnBench.DesignersOnBenchCount,
                ScrumMastersOnBenchCount = totalNumberOfEmployeesOnBench.ScrumMastersOnBenchCount,
                TotalNumberOfEmployeesOnBench = totalNumberOfEmployeesOnBench.TotalNumberOfEmployeesOnBench,
                BillableEmployeesPercentage = Math.Round(billableEmployeesPercentage, 0),
                EmployeeTotalDifference = employeeTotalDifference,
                isIncrease = isIncrease
            };
        }

        public async Task<MonthlyEmployeeTotalDto> GetEmployeePreviousMonthTotal()
        {
            var previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");

            var previousEmployeeTotal = _db.MonthlyEmployeeTotal
                                           .Get()
                                           .Where(e => e.Month == previousMonth)
                                           .FirstOrDefault();

            if (previousEmployeeTotal == null) return await GetEmployeeCurrentMonthTotal();

            return previousEmployeeTotal.ToDto();
        }

        public async Task<MonthlyEmployeeTotalDto> GetEmployeeCurrentMonthTotal()
        {
            var currentMonth = DateTime.Now.ToString("MMMM");

            var currentYear = DateTime.Now.Year;

            var currentEmployeeTotal = _db.MonthlyEmployeeTotal
                                          .Get()
                                          .Where(e => e.Month == currentMonth && e.Year == currentYear)
                                          .FirstOrDefault();

            if (currentEmployeeTotal == null)
            {
                var employeeTotalCount = await _db.Employee.GetAll();

                var employeeCountTotalsByRole = GetEmployeeCountTotalByRole();

                var monthlyEmployeeTotalDto = new MonthlyEmployeeTotalDto
                {
                    Id = 0,
                    EmployeeTotal = employeeTotalCount.Count,
                    DeveloperTotal = employeeCountTotalsByRole.DevsCount,
                    DesignerTotal = employeeCountTotalsByRole.DesignersCount,
                    ScrumMasterTotal = employeeCountTotalsByRole.ScrumMastersCount,
                    BusinessSupportTotal = employeeCountTotalsByRole.BusinessSupportCount,
                    Month = currentMonth,
                    Year = currentYear
                };
                var newMonthlyEmployeeTotal = new MonthlyEmployeeTotal(monthlyEmployeeTotalDto);

                return (await _db.MonthlyEmployeeTotal.Add(newMonthlyEmployeeTotal)).ToDto();
            }
            return currentEmployeeTotal.ToDto();
        }

        private double CalculateChurnRate(int employeeStartOfPeriodTotal, int terminationsDuringPeriod)
        {
            return Math.Round((employeeStartOfPeriodTotal > 0)
                ? (double)terminationsDuringPeriod / employeeStartOfPeriodTotal * 100
                : 0, 0);
        }


        public EmployeeOnBenchDataCard GetTotalNumberOfEmployeesOnBench()
        {
            var totalNumberOfDevsOnBench = _db.Employee.Get()
                                              .Where(c => c.ClientAllocated == null && c.EmployeeTypeId == 2)
                                              .ToList().Count;

            var totalNumberOfDesignersOnBench = _db.Employee.Get()
                                                   .Where(c => c.ClientAllocated == null && c.EmployeeTypeId == 3)
                                                   .ToList().Count;

            var totalNumberOfScrumMastersOnBench = _db.Employee.Get()
                                                      .Where(c => c.ClientAllocated == null && c.EmployeeTypeId == 4)
                                                      .ToList().Count;

            var totalnumberOfEmployeesOnBench = totalNumberOfDevsOnBench +
                                                totalNumberOfDesignersOnBench +
                                                totalNumberOfScrumMastersOnBench;

            return new EmployeeOnBenchDataCard
            {
                DevsOnBenchCount = totalNumberOfDevsOnBench,
                DesignersOnBenchCount = totalNumberOfDesignersOnBench,
                ScrumMastersOnBenchCount = totalNumberOfScrumMastersOnBench,
                TotalNumberOfEmployeesOnBench = totalnumberOfEmployeesOnBench
            };
        }


        public EmployeeCountByRoleDataCard GetEmployeeCountTotalByRole()
        {
            var devsTotal = _db.Employee.Get()
                               .Where(e => e.EmployeeTypeId == 2)
                               .ToList().Count;

            var designersTotal = _db.Employee.Get()
                                    .Where(e => e.EmployeeTypeId == 3)
                                    .ToList().Count;

            var scrumMastersTotal = _db.Employee.Get()
                                       .Where(e => e.EmployeeTypeId == 4)
                                       .ToList().Count;

            var businessSupportTotal = _db.Employee.Get()
                                          .Where(e => e.EmployeeTypeId > 4)
                                          .ToList().Count;

            return new EmployeeCountByRoleDataCard
            {
                DevsCount = devsTotal,
                DesignersCount = designersTotal,
                ScrumMastersCount = scrumMastersTotal,
                BusinessSupportCount = businessSupportTotal
            };
        }

        public int GetTotalNumberOfEmployeesOnClients()
        {
            var totalOfDevsDesignersAndScrumsOnClients = _db.Employee
                                                            .Get()
                                                            .Where(e => (e.EmployeeTypeId == 2 || e.EmployeeTypeId == 3 ||
                                                                         e.EmployeeTypeId == 4) && e.ClientAllocated != 1)
                                                            .ToList()
                                                            .Count;

            return totalOfDevsDesignersAndScrumsOnClients;
        }

    }
}
