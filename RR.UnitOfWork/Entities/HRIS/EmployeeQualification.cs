using HRIS.Models;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;


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
        Qualification = employeeQualificationDto.
        School = employeeQualificationDto.School;
        Degree = employeeQualificationDto.Degree;
        FieldOfStudy = employeeQualificationDto.FieldOfStudy;
        NQF = employeeQualificationDto.NQF;
        StartDate = employeeQualificationDto.StartDate;
        EndDate = employeeQualificationDto.EndDate;
    }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("qualification")] public string? Qualification { get; set; }

    [Column("school")] public string? School { get; set; }

    [Column("degree")] public string? Degree { get; set; }

    [Column("field")] public string? FieldOfStudy { get; set; }

    [Column("nqf")] public string? NQF { get; set; }

    [Column("startDate")] public DateTime StartDate { get; set; }

    [Column("endDate")] public DateTime? EndDate { get; set; }

    public virtual Employee? Employee { get; set; }

    [Key][Column("id")] public int Id { get; set; }

    public EmployeeQualificationDto ToDto()
    {
        return new EmployeeQualificationDto
        {
            Id = Id,
            EmployeeId = EmployeeId,
            Qualification = Qualification,
            School = School,
            Degree = Degree,
            FieldOfStudy = FieldOfStudy,
            NQF = NQF,
            StartDate = StartDate,
            EndDate = EndDate
        };
    }
}