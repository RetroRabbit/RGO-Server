using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class EmployeeCountByRoleDataCard
{
    [Required(ErrorMessage = "Employee Count By Role Data Card 'DevsCount' field is missing.")]
    public int DevsCount { get; set; }
    [Required(ErrorMessage = "Employee Count By Role Data Card 'DesignersCount' field is missing.")]
    public int DesignersCount { get; set; }
    [Required(ErrorMessage = "Employee Count By Role Data Card 'ScrumMastersCount' field is missing.")]
    public int ScrumMastersCount { get; set; }
    [Required(ErrorMessage = "Employee Count By Role Data Card 'BusinessSupportCount' field is missing.")]
    public int BusinessSupportCount { get; set; }
}