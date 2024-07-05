using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeRole")]
public class EmployeeRole : IModel
{
    public EmployeeRole()
    {
    }


    public EmployeeRole(EmployeeRoleDto employeeRoleDto)
    {
        Id = employeeRoleDto.Id;
        EmployeeId = employeeRoleDto.Employee!.Id;
        RoleId = employeeRoleDto.Role!.Id;
    }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("roleId")]
    [ForeignKey("Role")]
    public int RoleId { get; set; }

    public Employee? Employee { get; set; }
    public Role? Role { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public EmployeeRoleDto ToDto()
    {
        return new EmployeeRoleDto
        {
            Id = Id,
            Employee = Employee?.ToDto(),
            Role = Role?.ToDto()
        };
    }
}
