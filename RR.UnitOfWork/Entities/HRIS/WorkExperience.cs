using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RR.UnitOfWork.Interfaces;
using HRIS.Models.Employee.Commons;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("WorkExperience")]
public class WorkExperience : IModel
{
    public WorkExperience() { }

    public WorkExperience(WorkExperienceDto workExperienceDto)
    {
        Id = workExperienceDto.Id;
        ClientName = workExperienceDto.ClientName;
        ProjectName = workExperienceDto.ProjectName;
        SkillSet = workExperienceDto.SkillSet;
        Software = workExperienceDto.Software;
        StartDate = workExperienceDto.StartDate;
        EndDate = workExperienceDto.EndDate;
        ProjectDescription = workExperienceDto.ProjectDescription;
        EmployeeId = workExperienceDto.EmployeeId;
    }

    [Key][Column("id")] public int Id { get; set; }

    [Column("clientName")] public string? ClientName { get; set; }

    [Column("projectName")] public string? ProjectName { get; set; }

    [Column("skillSet")] public List<string>? SkillSet { get; set; }

    [Column("software")] public List<string>? Software { get; set; }

    [Column("startDate")] public DateTime StartDate { get; set; }

    [Column("endDate")] public DateTime EndDate { get; set; }

    [Column("projectDescription")] public string? ProjectDescription {  get; set; }

    [ForeignKey("Employee")]

    [Column("employeeId")] public int EmployeeId {  get; set; }

    public virtual Employee? Employee { get; set; }

    public WorkExperienceDto ToDto()
    {
        return new WorkExperienceDto
        {
            Id = Id,
            ClientName = ClientName,
            ProjectName = ProjectName,
            SkillSet = SkillSet,
            Software = Software,
            EmployeeId = EmployeeId,
            StartDate = StartDate,
            EndDate = EndDate,
            ProjectDescription = ProjectDescription,
        };
    }
}

