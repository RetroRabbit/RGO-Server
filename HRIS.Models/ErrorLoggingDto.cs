using System;

namespace ATS.Models
{
    public class ErrorLoggingDto
    {
        public int Id { get; set; }
        public DateTime DateOfIncident { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }

        // Additional properties for error details
        public int? StatusCode { get; set; }
        public string ErrorDetails { get; set; }
    }
}
