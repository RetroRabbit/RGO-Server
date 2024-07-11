using HRIS.Models;
using HRIS.Services.Interfaces;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _db;

        public DashboardService(IUnitOfWork db)
        {
            _db = db;
        }
        
        public async Task<ChurnRateDataCardDto> CalculateEmployeeChurnRate()
        {
            var today = DateTime.Today;
            var twelveMonthsAgo = today.AddMonths(-12);

            var employeeData = await _db.Employee
                .GetAll();

            var employeeStartOfPeriod = employeeData
                .Where(e => e.EngagementDate < twelveMonthsAgo && e.Active == true)
                .ToList();

            var terminatedEmployeesEndOfPeriod = employeeData
                .Where(e => e.Active == false && e.TerminationDate >= twelveMonthsAgo)
                .ToList();

            var employeeStartOfPreviousPeriod = employeeData
                 .Where(e => e.EngagementDate < twelveMonthsAgo.AddMonths(-12) && e.Active == true)
                .ToList();

            var terminatedEmployeesEndOfPreviousPeriod = employeeData
                .Where(e => e.Active == false && e.TerminationDate >= twelveMonthsAgo.AddMonths(-12))
                .ToList();

            int GetCount(List<Employee> data, int employeeType) => data.Count(x => x.EmployeeTypeId == employeeType);

            var churnRate = CalculateChurnRate(employeeStartOfPeriod.Count, terminatedEmployeesEndOfPeriod.Count);
         
            var previousChurnRate = CalculateChurnRate(employeeStartOfPreviousPeriod.Count, terminatedEmployeesEndOfPreviousPeriod.Count);

            var averageChurnRate = (previousChurnRate + churnRate) / 2;
            var percentageDifference = averageChurnRate != 0
                ? ((previousChurnRate - churnRate) / averageChurnRate) * 100
                : 0;

            var isIncrease = percentageDifference > 0;

            return new ChurnRateDataCardDto
            {
                ChurnRate = churnRate,
                DeveloperChurnRate = CalculateChurnRate(GetCount(employeeStartOfPeriod, 1), GetCount(terminatedEmployeesEndOfPeriod, 1)),
                DesignerChurnRate = CalculateChurnRate(GetCount(employeeStartOfPeriod, 2), GetCount(terminatedEmployeesEndOfPeriod, 2)),
                ScrumMasterChurnRate = CalculateChurnRate(GetCount(employeeStartOfPeriod, 3), GetCount(terminatedEmployeesEndOfPeriod, 3)),
                BusinessSupportChurnRate = CalculateChurnRate(GetCount(employeeStartOfPeriod, 4), GetCount(terminatedEmployeesEndOfPeriod, 4)),
                Month = twelveMonthsAgo.ToString("MMMM"),
                Year = twelveMonthsAgo.Year,
                ChurnRateDifference = percentageDifference,
                IsIncrease = isIncrease,
            };
        }

        private double CalculateChurnRate(int employeeStartOfPeriodTotal, int terminationsDuringPeriod)
        {
            return Math.Round(employeeStartOfPeriodTotal > 0
                ? (double)terminationsDuringPeriod / employeeStartOfPeriodTotal * 100
                : 0, 0);
        }

        public async Task<double> CalculateEmployeeGrowthRate()
        {
            var currentMonthTotal = await GetEmployeeCurrentMonthTotal();
            var previousMonthTotal = await GetEmployeePreviousMonthTotal();

            var currentTotal = currentMonthTotal.EmployeeTotal;
            var previousTotal = previousMonthTotal.EmployeeTotal;

            if (previousTotal == 0)
            {
                return 0;
            }

            var growthRate = ((double)(currentTotal - previousTotal) / previousTotal) * 100;
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
                TotalNumberOfEmployeesOnClients = totalNumberOfEmployeesOnClients,
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
