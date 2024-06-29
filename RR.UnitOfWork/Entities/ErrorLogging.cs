using ATS.Models;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RR.UnitOfWork.Entities;

[Table("ErrorLog")]
public class ErrorLogging: IModel
{
    public ErrorLogging() { }

    public ErrorLogging(ErrorLoggingDto errorLoggingDto)
    {   
        Id = errorLoggingDto.Id;
        dateOfIncident = errorLoggingDto.dateOfIncident;
        stackTrace = errorLoggingDto.stackTrace;
        message = errorLoggingDto.message;
    }

    [Key][Column("id")] public int Id { get; set; }
    [Column("dateOfIncident")] public DateTime dateOfIncident { get; set; }
    [Column("stackTrace")] public string stackTrace { get; set; }
    [Column("message")] public string message { get; set; }

    public ErrorLoggingDto ToDto()
    {
        return new ErrorLoggingDto
        {
            Id = this.Id,
            dateOfIncident = this.dateOfIncident,
            stackTrace = this.stackTrace,
            message = this.message
        };
    }
}
