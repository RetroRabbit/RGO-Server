using ATS.Models;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RR.UnitOfWork.Entities;

[Table("ErrorLog")]
public class ErrorLogging: IModel<ErrorLoggingDto>
{
    public ErrorLogging() { }

    public ErrorLogging(ErrorLoggingDto errorLoggingDto)
    {   
        Id = errorLoggingDto.Id;
        dateOfIncident = errorLoggingDto.dateOfIncident;
        exceptionType = errorLoggingDto.exceptionType;
        message = errorLoggingDto.message;
    }

    [Key][Column("id")] public int Id { get; set; }
    [Column("dateOfIncident")] public DateTime dateOfIncident { get; set; }
    [Column("exceptionType")] public string exceptionType { get; set; }
    [Column("message")] public string message { get; set; }

    public ErrorLoggingDto ToDto()
    {
        return new ErrorLoggingDto
        {
            Id = this.Id,
            dateOfIncident = this.dateOfIncident,
            exceptionType = this.exceptionType,
            message = this.message
        };
    }
}
