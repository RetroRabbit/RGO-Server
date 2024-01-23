using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Models
{
    public class EmployeeCountDto
    {
        public int DevsCount { get; set; }
        public int DesignersCount { get; set; }
        public int ScrumMastersCount { get; set; }
        public int BusinessSupportCount { get; set; }
        public int DevsOnBenchCount { get; set; }
        public int DesignersOnBenchCount { get;set; }
        public int ScrumMastersOnBenchCount { get; set; }
        public int TotalNumberOfEmployeesOnBench { get; set; }
        public double BillableEmployeesPercentage { get; set; }
        public int EmployeeTotalDifference { get; set; }
        public Boolean isIncrease { get; set; }
    }
}
