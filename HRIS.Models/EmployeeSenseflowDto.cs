using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Models;
public class EmployeeSenseflowDto
{
    public string Fullname { get; set; }
    public EmployeeRoleDto Role { get; set; }
    public string Level { get; set; }
    public DateTime Start { get; set; }
    public string Email { get; set; }
}
