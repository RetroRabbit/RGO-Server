using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Models;

public class SimpleEmployeeDocumentGetAllDto
{
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}
