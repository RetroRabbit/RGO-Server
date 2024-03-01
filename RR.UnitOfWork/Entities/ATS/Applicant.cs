using ATS.Models;
using ATS.Models.Enums;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RR.UnitOfWork.Entities.ATS;

[Table("Applicant")]
public class Applicant : IModel<ApplicantDto>
{
    public Applicant() { }

    public Applicant(ApplicantDto applicantDto)
    {
        Id = applicantDto.Id;
        Name = applicantDto.Name;
        Surname = applicantDto.Surname;
        PersonalEmail = applicantDto.PersonalEmail;
        PotentialLevel = applicantDto.PotentialLevel;
        JobPosition = applicantDto.JobPosition;
        LinkedIn = applicantDto.LinkedIn;
        ProfilePicture = applicantDto.ProfilePicture;
    }

    [Key][Column("id")] public int Id { get; set; }

    [Column("name")] public string Name { get; set; }

    [Column("surname")] public string Surname { get; set; }

    [Column("personalEmail")] public string PersonalEmail { get; set; }

    [Column("potentialLevel")] public int PotentialLevel { get; set; }

    [Column("jobPosition")] public PositionType JobPosition { get; set; }

    [Column("linkedIn")] public string? LinkedIn { get; set; }

    [Column("profilePicture")] public string? ProfilePicture { get; set; }

    public ApplicantDto ToDto()
    {
        return new ApplicantDto
        {
            Id = this.Id,
            Name = this.Name,
            Surname = this.Surname,
            PersonalEmail = this.PersonalEmail,
            PotentialLevel = this.PotentialLevel,
            JobPosition = this.JobPosition,
            LinkedIn = this.LinkedIn,
            ProfilePicture = this.ProfilePicture
        };
    }
}
