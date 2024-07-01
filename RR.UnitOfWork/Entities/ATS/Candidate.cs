using ATS.Models;
using ATS.Models.Enums;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RR.UnitOfWork.Entities.ATS;

[Table("Candidate")]
public class Candidate : IModel
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
        CellphoneNumber = candidateDto.CellphoneNumber;
        Location = candidateDto.Location;
        CV = candidateDto.CV;
        PortfolioLink = candidateDto.PortfolioLink;
        PortfolioPdf = candidateDto.PortfolioPdf;
        Gender = candidateDto.Gender;
        Race = candidateDto.Race;
        IdNumber = candidateDto.IdNumber;
        Referral = candidateDto.Referral;
        HighestQualification = candidateDto.HighestQualification;
        School = candidateDto.School;
        QualificationEndDate = candidateDto.QualificationEndDate;
        BlacklistedStatus = candidateDto.BlacklistedStatus;
        BlacklistedReason = candidateDto.BlacklistedReason;
    }

    [Key][Column("id")] public int Id { get; set; }

    [Column("name")] public string Name { get; set; }

    [Column("surname")] public string Surname { get; set; }

    [Column("personalEmail")] public string PersonalEmail { get; set; }

    [Column("potentialLevel")] public int PotentialLevel { get; set; }

    [Column("jobPosition")] public PositionType JobPosition { get; set; }

    [Column("linkedIn")] public string? LinkedIn { get; set; }

    [Column("profilePicture")] public string? ProfilePicture { get; set; }

    [Column("cellphone")] public string? CellphoneNumber {  get; set; }

    [Column("location")] public string? Location { get; set; }

    [Column("cv")] public string? CV { get; set; }

    [Column("portfolioLink")] public string? PortfolioLink { get; set; }

    [Column("portfolioPdf")] public string? PortfolioPdf { get; set; }

    [Column("gender")] public Gender Gender { get; set; }

    [Column("race")] public Race Race { get; set; }

    [Column("idNumber")] public string? IdNumber { get; set; }

    [Column("referral")] public int Referral {  get; set; }

    [Column("highestQualification")] public string? HighestQualification { get; set; }

    [Column("school")] public string? School { get; set; }

    [Column("qualificationEndDate")] public int? QualificationEndDate { get; set; }

    [Column("blacklisted")] public BlacklistStatus BlacklistedStatus { get; set; }

    [Column("blacklistedReason")] public string? BlacklistedReason { get; set; }


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
            ProfilePicture = this.ProfilePicture,
            CellphoneNumber = this.CellphoneNumber,
            Location = this.Location,
            CV = this.CV,
            PortfolioLink = this.PortfolioLink,
            PortfolioPdf = this.PortfolioPdf,
            Gender = this.Gender,
            Race = this.Race,
            IdNumber = this.IdNumber,
            Referral = this.Referral,
            HighestQualification = this.HighestQualification,
            School = this.School,
            QualificationEndDate = this.QualificationEndDate,
            BlacklistedStatus = this.BlacklistedStatus,
            BlacklistedReason = this.BlacklistedReason
        };
    }
}
