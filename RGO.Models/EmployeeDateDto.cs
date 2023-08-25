using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Models
{
    public record EmployeeDateDto(
        int Id, 
        EmployeeDto Employee,
        string Code,
        DateTime Date
        );
}
