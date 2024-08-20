using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class BankingAndStarterKitDto
{
    [Required(ErrorMessage = "Banking And Starter Kit 'EmployeeBankingDto' field is missing.")]
    public EmployeeBankingDto EmployeeBankingDto { get; set; }
    [Required(ErrorMessage = "Banking And Starter Kit 'EmployeeDocumentDto' field is missing.")]
    public EmployeeDocumentDto EmployeeDocumentDto { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    [Required(ErrorMessage = "Banking And Starter Kit 'EmployeeId' field is missing.")]
    public int EmployeeId { get; set; }
}
