using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("EmployeeDate")]
public class EmployeeDate : IModel<EmployeeDateDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("subject")]
    public string Subject { get; set; }

    [Column("note")]
    public string Note { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }

    public virtual Employee Employee { get; set; }
    public EmployeeDate() { }

    public EmployeeDate(EmployeeDateDto employeeDateDto)
    {
        Id = employeeDateDto.Id;
        EmployeeId = employeeDateDto.Employee!.Id;
        Subject = employeeDateDto.Subject;
        Date = employeeDateDto.Date;
    }

    public EmployeeDateDto ToDto()
    {
        return new EmployeeDateDto(
            Id,
            Employee?.ToDto(),
            Subject,
            Note,
            Date);
    }
}
