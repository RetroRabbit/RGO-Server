using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeType")]
public class EmployeeType : IModel<EmployeeTypeDto>
{
    public EmployeeType()
    {
    }

    public EmployeeType(EmployeeTypeDto employmentType)
    {
        Id = employmentType.Id;
        Name = employmentType.Name;
    }

    [Column("name")] public string? Name { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public EmployeeTypeDto ToDto()
    {
        return new EmployeeTypeDto
        {
            Id = Id,
            Name = Name
        };
    }
}
