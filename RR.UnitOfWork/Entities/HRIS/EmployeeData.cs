using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeData")]
public class EmployeeData : IModel<EmployeeDataDto>
{
    public EmployeeData()
    {
    }

    public EmployeeData(EmployeeDataDto employeeDataDto)
    {
        Id = employeeDataDto.Id;
        EmployeeId = employeeDataDto.EmployeeId;
        FieldCodeId = employeeDataDto.FieldCodeId;
        Value = employeeDataDto.Value;
    }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("fieldCodeId")]
    [ForeignKey("FieldCode")]
    public int FieldCodeId { get; set; }

    [Column("value")] public string Value { get; set; } = null!;

    public virtual Employee? Employee { get; set; }
    public virtual FieldCode? FieldCode { get; set; }

    [Key] [Column("Id")] public int Id { get; set; }

    public EmployeeDataDto ToDto()
    {
        return new EmployeeDataDto
        {
            Id = Id,
            EmployeeId = EmployeeId,
            FieldCodeId = FieldCodeId,
            Value = Value,
        };
    }
}
