using HRIS.Models.Employee.Commons;

namespace HRIS.Models;

public class BankingAndStarterKitDto
{
    public EmployeeBankingDto EmployeeBankingDto { get; set; }
    public EmployeeDocumentDto EmployeeDocumentDto { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int EmployeeId { get; set; }
}
