using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("EmployeeProjects")]
public class EmployeeProject : IModel<EmployeeProjectDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("client")]
    public string Client { get; set; }

    [Column("startDate")]
    public DateTime StartDate { get; set; }

    [Column("endDate")]
    public DateTime? EndDate { get; set; }

    public virtual Employee Employee { get; set; }

    public EmployeeProject() { }

    public EmployeeProject(EmployeeProjectDto employeeProjectDto)
    {
        Id = employeeProjectDto.Id;
        EmployeeId = employeeProjectDto.EmployeeId;
        Name = employeeProjectDto.Name;
        Description = employeeProjectDto.Description;
        Client = employeeProjectDto.Client;
        StartDate = employeeProjectDto.StartDate;
        EndDate = employeeProjectDto.EndDate;
    }

    public EmployeeProjectDto ToDto()
    {
        return new EmployeeProjectDto(
            Id,
            EmployeeId,
            Name,
            Description,
            Client,
            StartDate,
            EndDate);
    }
}
