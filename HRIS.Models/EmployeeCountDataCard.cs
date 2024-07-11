namespace HRIS.Models;

public class EmployeeCountDataCard
{
    public int DevsCount { get; set; }
    public int DesignersCount { get; set; }
    public int ScrumMastersCount { get; set; }
    public int BusinessSupportCount { get; set; }
    public int DevsOnBenchCount { get; set; }
    public int DesignersOnBenchCount { get; set; }
    public int ScrumMastersOnBenchCount { get; set; }
    public int TotalNumberOfEmployeesOnClients { get; set; }
    public int TotalNumberOfEmployeesOnBench { get; set; }
    public double BillableEmployeesPercentage { get; set; }
    public int EmployeeTotalDifference { get; set; }
    public bool isIncrease { get; set; }
}
