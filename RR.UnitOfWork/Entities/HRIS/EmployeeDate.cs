using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeDate")]
public class EmployeeDate : IModel<EmployeeDateDto>
{
    public EmployeeDate()
    {
    }

    public EmployeeDate(EmployeeDateDto employeeDateDto)
    {
        Id = employeeDateDto.Id;
        EmployeeId = employeeDateDto.Employee!.Id;
        Subject = employeeDateDto.Subject;
        Note = employeeDateDto.Note;
        Date = employeeDateDto.Date;
    }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("subject")] public string? Subject { get; set; }

    [Column("note")] public string? Note { get; set; }

    [Column("date")] public DateOnly Date { get; set; }

    public virtual Employee? Employee { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public EmployeeDateDto ToDto()
    {
        return new EmployeeDateDto
        {
            Id = Id,
            Employee = Employee?.ToDto(),
            Subject = Subject,
            Note = Note,
            Date = Date
        };
    }
}
