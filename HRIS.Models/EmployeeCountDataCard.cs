using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class EmployeeCountDataCard
{
    [Required(ErrorMessage = "Employee Count Data Card 'DevsCount' field is missing.")]
    public int DevsCount { get; set; }
    [Required(ErrorMessage = "Employee Count Data Card 'DesignersCount' field is missing.")]
    public int DesignersCount { get; set; }
    [Required(ErrorMessage = "Employee Count Data Card 'ScrumMastersCount' field is missing.")]
    public int ScrumMastersCount { get; set; }
    [Required(ErrorMessage = "Employee Count Data Card 'BusinessSupportCount' field is missing.")]
    public int BusinessSupportCount { get; set; }
    [Required(ErrorMessage = "Employee Count Data Card 'DevsOnBenchCount' field is missing.")]
    public int DevsOnBenchCount { get; set; }
    [Required(ErrorMessage = "Employee Count Data Card 'DesignersOnBenchCount' field is missing.")]
    public int DesignersOnBenchCount { get; set; }
    [Required(ErrorMessage = "Employee Count Data Card 'ScrumMastersOnBenchCount' field is missing.")]
    public int ScrumMastersOnBenchCount { get; set; }
    [Required(ErrorMessage = "Employee Count Data Card 'TotalNumberOfEmployeesOnClients' field is missing.")]
    public int TotalNumberOfEmployeesOnClients { get; set; }
    [Required(ErrorMessage = "Employee Count Data Card 'TotalNumberOfEmployeesOnBench' field is missing.")]
    public int TotalNumberOfEmployeesOnBench { get; set; }
    [Required(ErrorMessage = "Employee Count Data Card 'BillableEmployeesPercentage' field is missing.")]
    public double BillableEmployeesPercentage { get; set; }
    [Required(ErrorMessage = "Employee Count Data Card 'EmployeeTotalDifference' field is missing.")]
    public int EmployeeTotalDifference { get; set; }
    [Required(ErrorMessage = "Employee Count Data Card 'isIncrease' field is missing.")]
    public bool isIncrease { get; set; }
}
