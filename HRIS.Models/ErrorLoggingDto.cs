using System;

namespace ATS.Models
{
    public class ErrorLoggingDto
    {
        public int Id { get; set; }
        public DateTime DateOfIncident { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public string IpAddress { get; set; }
        public int? StatusCode { get; set; }
        public string RequestUrl { get; set; }
        public string RequestMethod { get; set; }
        public string? RequestContentType { get; set; }
        public string? RequestBody { get; set; }
    }
}
