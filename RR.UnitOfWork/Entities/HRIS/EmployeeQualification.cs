using HRIS.Models;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models.Enums.QualificationEnums;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeQualifications")]
public class EmployeeQualification : IModel<EmployeeQualificationDto>
{
    public EmployeeQualification()
    {
        School = string.Empty;
        FieldOfStudy = string.Empty;
        Year = DateOnly.MinValue;
    }

    public EmployeeQualification(EmployeeQualificationDto employeeQualificationDto)
    {
        Id = employeeQualificationDto.Id;
        EmployeeId = employeeQualificationDto.EmployeeId;
        HighestQualification = employeeQualificationDto.HighestQualification;
        School = employeeQualificationDto.School;
        FieldOfStudy = employeeQualificationDto.FieldOfStudy;
        NQFLevel = employeeQualificationDto.NQFLevel;
        Year = employeeQualificationDto.Year;
        ProofOfQualification = employeeQualificationDto.ProofOfQualification;
        DocumentName = employeeQualificationDto.DocumentName;
    }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("highestQualification")]
    public HighestQualification HighestQualification { get; set; }

    [Column("school")]
    public string School { get; set; }

    [Column("fieldOfStudy")]
    public string FieldOfStudy { get; set; }

    [Column("nqfLevel")]
    public NQFLevel NQFLevel { get; set; }

    [Column("year")]
    public DateOnly Year { get; set; }

    [Column("proofOfQualification")]
    public string ProofOfQualification { get; set; }

    [Column("documentName")]
    public string DocumentName { get; set; }

    public virtual Employee Employee { get; set; }

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
            FieldOfStudy = FieldOfStudy,
            NQFLevel = NQFLevel,
            Year = Year,
            ProofOfQualification = ProofOfQualification,
            DocumentName = DocumentName,
        };
    }
}