using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("EmployeeType")]
public class EmployeeType : IModel<EmployeeTypeDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    public EmployeeType() { }

    public EmployeeType(EmployeeTypeDto employmentType)
    {
        Id = employmentType.Id;
        Name = employmentType.Name;
    }

    public EmployeeTypeDto ToDto()
    {
        return new EmployeeTypeDto(
            Id,
            Name);
    }
}
