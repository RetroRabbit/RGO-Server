﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("WorkExperience")]
public class WorkExperience : IModel<WorkExperienceDto>
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
        EmployeeId = workExperienceDto.EmployeeId;
    }

    [Key][Column("id")] public int Id { get; set; }

    [Column("clientName")] public string? ClientName { get; set; }

    [Column("projectName")] public string? ProjectName { get; set; }

    [Column("skillSet")] public List<string>? SkillSet { get; set; }

    [Column("software")] public List<string>? Software { get; set; }

    [Column("startDate")] public DateOnly StartDate { get; set; }

    [Column("endDate")] public DateOnly EndDate { get; set; }

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
        };
    }
}

