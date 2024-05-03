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
        Title = workExperienceDto.Title;
        EmploymentType = workExperienceDto.EmploymentType;
        CompanyName = workExperienceDto.CompanyName;
        Location = workExperienceDto.Location;
        StartDate = workExperienceDto.StartDate;
        EndDate = workExperienceDto.EndDate;
    }

    [Column("workExperience")]
    [ForeignKey("Employee")]

    public int WorkExperienceId { get; set; }
    [Column("title")] public string Title { get; set; }

    [Column("employmentType")] public string? EmploymentType { get; set; }

    [Column("companyName")] public string? CompanyName { get; set; }

    [Column("location")] public string? Location { get; set; }

    [Column("startDate")] public DateOnly StartDate { get; set; }

    [Column("endDate")] public DateOnly EndDate { get; set; }

    [Key][Column("id")] public int Id { get; set; }

    public WorkExperienceDto ToDto()
    {
        return new WorkExperienceDto
        {
            Id = Id,
            Title = Title,
            EmploymentType = EmploymentType,
            CompanyName = CompanyName,
            StartDate = StartDate,
            EndDate = EndDate
        };
    }
}

