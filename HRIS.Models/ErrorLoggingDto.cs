using System;
using System.ComponentModel.DataAnnotations;

namespace ATS.Models
{
    public class ErrorLoggingDto
    {
        [Required(ErrorMessage = "Error Logging 'Id' field is missing.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Error Logging 'DateOfIncident' field is missing.")]
        [DataType(DataType.DateTime)]
        public DateTime DateOfIncident { get; set; }
        [Required(ErrorMessage = "Error Logging 'StackTrace' field is missing.")]
        public string StackTrace { get; set; }
        [Required(ErrorMessage = "Error Logging 'Message' field is missing.")]
        public string Message { get; set; }
        public string IpAddress { get; set; }
        public int? StatusCode { get; set; }
        public string RequestUrl { get; set; }
        public string RequestMethod { get; set; }
        public string? RequestContentType { get; set; }
        public string? RequestBody { get; set; }
    }
}
