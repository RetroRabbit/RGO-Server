using ATS.Models;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RR.UnitOfWork.Entities
{
    [Table("ErrorLog")]
    public class ErrorLogging : IModel
    {
        public ErrorLogging() { }

        public ErrorLogging(ErrorLoggingDto errorLoggingDto)
        {
            Id = errorLoggingDto.Id;
            DateOfIncident = errorLoggingDto.DateOfIncident;
            StackTrace = errorLoggingDto.StackTrace;
            Message = errorLoggingDto.Message;
            IpAddress = errorLoggingDto.IpAddress;
            RequestUrl = errorLoggingDto.RequestUrl;
            RequestMethod = errorLoggingDto.RequestMethod;
            RequestContentType = errorLoggingDto.RequestContentType;
            RequestBody = errorLoggingDto.RequestBody;
        }

        [Key]
        [Column("id")] public int Id { get; set; }
        [Column("dateOfIncident")] public DateTime DateOfIncident { get; set; }
        [Column("stackTrace")] public string StackTrace { get; set; }
        [Column("message")] public string Message { get; set; }
        [Column("ipAddress")] public string IpAddress { get; set; }
        [Column("requestUrl")] public string RequestUrl { get; set; }
        [Column("requestMethod")] public string RequestMethod { get; set; }
        [Column("requestContentType")] public string? RequestContentType { get; set; }
        [Column("requestBody")] public string? RequestBody { get; set; }

        public ErrorLoggingDto ToDto()
        {
            return new ErrorLoggingDto
            {
                Id = this.Id,
                DateOfIncident = this.DateOfIncident,
                StackTrace = this.StackTrace,
                Message = this.Message,
                IpAddress = this.IpAddress,
                RequestUrl = this.RequestUrl,
                RequestMethod = this.RequestMethod,
                RequestContentType = this.RequestContentType,
                RequestBody = this.RequestBody
            };
        }
    }
}
