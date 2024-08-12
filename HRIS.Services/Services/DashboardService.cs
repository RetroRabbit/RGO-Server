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
            var employeeCountTotalsByRole = await GetEmployeeCountTotalByRole();

            var totalNumberOfEmployeesOnBench = await GetTotalNumberOfEmployeesOnBench();

            var totalNumberOfEmployeesOnClients = await GetTotalNumberOfEmployeesOnClients();

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

            var previousEmployeeTotal = await _db.MonthlyEmployeeTotal
                                           .Get()
                                           .FirstOrDefaultAsync(e => e.Month == previousMonth);

            if (previousEmployeeTotal == null) return await GetEmployeeCurrentMonthTotal();

            return previousEmployeeTotal.ToDto();
        }

        public async Task<MonthlyEmployeeTotalDto> GetEmployeeCurrentMonthTotal()
        {
            var currentMonth = DateTime.Now.ToString("MMMM");

            var currentYear = DateTime.Now.Year;

            var currentEmployeeTotal = await _db.MonthlyEmployeeTotal
                                          .Get()
                                          .FirstOrDefaultAsync(e => e.Month == currentMonth && e.Year == currentYear);

            if (currentEmployeeTotal == null)
            {
                var employeeTotalCount = await _db.Employee.GetAll();

                var employeeCountTotalsByRole = await GetEmployeeCountTotalByRole();

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

        public async Task<EmployeeOnBenchDataCard> GetTotalNumberOfEmployeesOnBench()
        {
            var totalNumberOfDevsOnBench = await _db.Employee.Get()
                                              .CountAsync(c => c.ClientAllocated == null && c.EmployeeTypeId == 2 && c.Active == true);

            var totalNumberOfDesignersOnBench = await _db.Employee.Get()
                                                   .CountAsync(c => c.ClientAllocated == null && c.EmployeeTypeId == 3 && c.Active == true);

            var totalNumberOfScrumMastersOnBench = await _db.Employee.Get()
                                                      .CountAsync(c => c.ClientAllocated == null && c.EmployeeTypeId == 4 && c.Active == true);        

            var totalNumberOfEmployeesOnBench = totalNumberOfDevsOnBench +
                                                totalNumberOfDesignersOnBench +
                                                totalNumberOfScrumMastersOnBench;

            return new EmployeeOnBenchDataCard
            {
                DevsOnBenchCount = totalNumberOfDevsOnBench,
                DesignersOnBenchCount = totalNumberOfDesignersOnBench,
                ScrumMastersOnBenchCount = totalNumberOfScrumMastersOnBench,
                TotalNumberOfEmployeesOnBench = totalNumberOfEmployeesOnBench
            };
        }

        public async Task<EmployeeCountByRoleDataCard> GetEmployeeCountTotalByRole()
        {
            var devsTotal = await _db.Employee.Get()
                               .CountAsync(e => e.EmployeeTypeId == 2 && e.Active == true);

            var designersTotal = await _db.Employee.Get()
                                    .CountAsync(e => e.EmployeeTypeId == 3 && e.Active == true);

            var scrumMastersTotal = await _db.Employee.Get()
                                       .CountAsync(e => e.EmployeeTypeId == 4 && e.Active == true);


            var businessSupportTotal = await _db.Employee.Get()
                                          .CountAsync(e => e.EmployeeTypeId > 4 && e.Active == true);
                                      
            return new EmployeeCountByRoleDataCard
            {
                DevsCount = devsTotal,
                DesignersCount = designersTotal,
                ScrumMastersCount = scrumMastersTotal,
                BusinessSupportCount = businessSupportTotal
            };
        }

        public async Task<int> GetTotalNumberOfEmployeesOnClients()
        {
            var totalOfDevsDesignersAndScrumsOnClients = await _db.Employee
                                                            .Get()
                                                            .CountAsync(e => (e.EmployeeTypeId == 2 || e.EmployeeTypeId == 3 ||
                                                                         e.EmployeeTypeId == 4) && e.ClientAllocated != 1 && e.Active == true);
            return totalOfDevsDesignersAndScrumsOnClients;
        }

        public async Task<List<EmployeeDto>> GetAllActiveEmployees()
        {
            return await _db.Employee
                            .Get(employee => employee.Active == true)
                            .AsNoTracking()
                            .Include(employee => employee.EmployeeType)
                            .Include(employee => employee.PhysicalAddress)
                            .Include(employee => employee.PostalAddress)
                            .OrderBy(employee => employee.Name)
                            .Select(employee => employee.ToDto())
                            .ToListAsync();
        }
    }
}
