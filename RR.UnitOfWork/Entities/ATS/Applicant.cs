using ATS.Models;
using ATS.Models.Enums;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RR.UnitOfWork.Entities.ATS;

[Table("Candidate")]
public class Candidate : IModel<CandidateDto>
{
    public Candidate() { }

    public Candidate(CandidateDto candidateDto)
    {
        Id = candidateDto.Id;
        Name = candidateDto.Name;
        Surname = candidateDto.Surname;
        PersonalEmail = candidateDto.PersonalEmail;
        PotentialLevel = candidateDto.PotentialLevel;
        JobPosition = candidateDto.JobPosition;
        LinkedIn = candidateDto.LinkedIn;
        ProfilePicture = candidateDto.ProfilePicture;
    }

    [Key][Column("id")] public int Id { get; set; }

    [Column("name")] public string Name { get; set; }

    [Column("surname")] public string Surname { get; set; }

    [Column("personalEmail")] public string PersonalEmail { get; set; }

    [Column("potentialLevel")] public int PotentialLevel { get; set; }

    [Column("jobPosition")] public PositionType JobPosition { get; set; }

    [Column("linkedIn")] public string? LinkedIn { get; set; }

    [Column("profilePicture")] public string? ProfilePicture { get; set; }

    public CandidateDto ToDto()
    {
        return new CandidateDto
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
