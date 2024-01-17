using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Models
{
    public record MonthlyEmployeeTotalDto(
        int Id,
        int EmployeeTotal,
        string Month,
        int Year
        );
   
}
