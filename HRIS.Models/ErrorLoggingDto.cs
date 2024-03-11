﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Models;

public class ErrorLoggingDto
{
    public int Id { get; set; }
    public required DateTime dateOfIncident { get; set; }
    public required string stackTrace { get; set; }
    public required string message { get; set; }

}
