using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("EmployeeRole")]
public class EmployeeRole : IModel<EmployeeRoleDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("roleId")]
    [ForeignKey("Role")]
    public int RoleId { get; set; }

    public Employee Employee { get; set; }
    public Role Role { get; set; }

    public EmployeeRole()
    {
    }



    public EmployeeRole(EmployeeRoleDto employeeRoleDto)
    {
        Id = employeeRoleDto.Id;
        EmployeeId = employeeRoleDto.Employee.Id;
        RoleId = employeeRoleDto.Role.Id;
    }

    public EmployeeRoleDto ToDto()
    {
        return new EmployeeRoleDto(
            Id,
            Employee?.ToDto(),
            Role?.ToDto());
    }
}
