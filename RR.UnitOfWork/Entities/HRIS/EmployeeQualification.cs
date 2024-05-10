using HRIS.Models;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using HRIS.Models.Enums.QualificationEnums;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeQualifications")]
public class EmployeeQualification : IModel<EmployeeQualificationDto>
{
    public EmployeeQualification()
    {
    }

    public EmployeeQualification(EmployeeQualificationDto employeeQualificationDto)
    {
        Id = employeeQualificationDto.Id;
        EmployeeId = employeeQualificationDto.EmployeeId;
        HighestQualification = employeeQualificationDto.HighestQualification;
        School = employeeQualificationDto.School;
        Degree = employeeQualificationDto.Degree;
        FieldOfStudy = employeeQualificationDto.FieldOfStudy;
        NQFLevel = employeeQualificationDto.NQFLevel;
        Year = employeeQualificationDto.Year;
    }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("highestQualification")] 
    public HighestQualification HighestQualification { get; set; }

    [Column("school")] 
    public string? School { get; set; }

    [Column("degree")] 
    public string? Degree { get; set; }

    [Column("fieldOfStudy")] 
    public string? FieldOfStudy { get; set; }

    [Column("nqfLevel")] 
    public NQFLevel NQFLevel { get; set; }

    [Column("year")] 
    public DateOnly? Year { get; set; }

    public virtual Employee? Employee { get; set; }

    [Key]
    [Column("id")] 
    public int Id { get; set; }

    public EmployeeQualificationDto ToDto()
    {
        return new EmployeeQualificationDto
        {
            Id = Id,
            EmployeeId = EmployeeId,
            HighestQualification = HighestQualification,
            School = School,
            Degree = Degree,
            FieldOfStudy = FieldOfStudy,
            NQFLevel = NQFLevel,
            Year = Year
        };
    }
}