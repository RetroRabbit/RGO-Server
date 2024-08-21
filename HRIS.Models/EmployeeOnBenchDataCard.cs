using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class EmployeeOnBenchDataCard
{
    [Required(ErrorMessage = "Employee On Bench Data Card 'DevsOnBenchCount' field is missing.")]
    public int DevsOnBenchCount { get; set; }
    [Required(ErrorMessage = "Employee On Bench Data Card 'DesignersOnBenchCount' field is missing.")]
    public int DesignersOnBenchCount { get; set; }
    [Required(ErrorMessage = "Employee On Bench Data Card 'ScrumMastersOnBenchCount' field is missing.")]
    public int ScrumMastersOnBenchCount { get; set; }
    [Required(ErrorMessage = "Employee On Bench Data Card 'TotalNumberOfEmployeesOnBench' field is missing.")]
    public int TotalNumberOfEmployeesOnBench { get; set; }
}