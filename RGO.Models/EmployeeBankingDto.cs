using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Models
{
    public record EmployeeBankingDto(
        int Id,
        EmployeeDto Employee,
        string BankName,
        string Branch,
        string AccountNo,
        int AccountType,
        string AccountHolderName
        );
}
