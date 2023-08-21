using RGO.Models;
using RGO.Models.Enums;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("EmployeeData")]
public class EmployeeData : IModel<EmployeeDataDto>
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("dataType")]
    public EmployeeDataType DataType { get; set; }

    [Column("dataSubType")]
    public EmployeeDataSubType SubType { get; set; }

    [Column("dataValue")]
    public string Value { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public EmployeeData()
    {
    }

    public EmployeeData(EmployeeDataDto employeeDataDto)
    {
        Id = employeeDataDto.Id;
        EmployeeId = employeeDataDto.Employee.Id;
        DataType = employeeDataDto.DataType;
        SubType = employeeDataDto.SubType;
        Value = employeeDataDto.Value;
    }

    public EmployeeDataDto ToDto()
    {
        return new EmployeeDataDto(
            Id,
            Employee.ToDto(),
            DataType,
            SubType,
            Value);
    }
}
