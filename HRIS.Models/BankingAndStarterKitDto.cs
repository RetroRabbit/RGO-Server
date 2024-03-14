using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Models;

public class BankingAndStarterKitDto
{
    public EmployeeBankingDto EmployeeBankingDto { get; set; }
    public EmployeeDocumentDto EmployeeDocumentDto { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
}
