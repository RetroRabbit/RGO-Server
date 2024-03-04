using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Models;

public class ErrorLoggingDto
{
    public int Id { get; set; }
    public DateOnly dateOfIncident { get; set; }
    public required string exceptionType { get; set; }
    public required string message { get; set; }

}
