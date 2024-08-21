using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class MonthlyEmployeeTotalDto
{
    [Required(ErrorMessage = "Monthly Employee Total 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Monthly Employee Total 'EmployeeTotal' field is missing.")]
    public int EmployeeTotal { get; set; }
    [Required(ErrorMessage = "Monthly Employee Total 'DeveloperTotal' field is missing.")]
    public int DeveloperTotal { get; set; }
    [Required(ErrorMessage = "Monthly Employee Total 'DesignerTotal' field is missing.")]
    public int DesignerTotal { get; set; }
    [Required(ErrorMessage = "Monthly Employee Total 'ScrumMasterTotal' field is missing.")]
    public int ScrumMasterTotal { get; set; }
    [Required(ErrorMessage = "Monthly Employee Total 'BusinessSupportTotal' field is missing.")]
    public int BusinessSupportTotal { get; set; }
    [Required(ErrorMessage = "Monthly Employee Total 'Month' field is missing.")]
    public string? Month { get; set; }
    [Required(ErrorMessage = "Monthly Employee Total 'Year' field is missing.")]
    public int Year { get; set; }
}
