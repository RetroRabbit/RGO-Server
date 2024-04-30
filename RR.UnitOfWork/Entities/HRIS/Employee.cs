using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using ATS.Models;
using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("Employee")]
public class Employee : IModel<EmployeeDto>
{
    public Employee()
    {
    }

    public Employee(EmployeeDto employeeDto, EmployeeTypeDto employeeType)
    {
        Id = employeeDto.Id;
        EmployeeNumber = employeeDto.EmployeeNumber;
        TaxNumber = employeeDto.TaxNumber;
        EngagementDate = employeeDto.EngagementDate;
        TerminationDate = employeeDto.TerminationDate;
        PeopleChampion = employeeDto.PeopleChampion;
        Disability = employeeDto.Disability;
        DisabilityNotes = employeeDto.DisabilityNotes;
        Level = employeeDto.Level;
        EmployeeTypeId = employeeType.Id;
        Notes = employeeDto.Notes;
        LeaveInterval = employeeDto.LeaveInterval;
        SalaryDays = employeeDto.SalaryDays;
        PayRate = employeeDto.PayRate;
        Salary = employeeDto.Salary;
        Initials = employeeDto.Initials;
        Name = employeeDto.Name;
        Surname = employeeDto.Surname;
        DateOfBirth = employeeDto.DateOfBirth;
        CountryOfBirth = employeeDto.CountryOfBirth;
        Nationality = employeeDto.Nationality;
        IdNumber = employeeDto.IdNumber;
        PassportNumber = employeeDto.PassportNumber;
        PassportExpirationDate = employeeDto.PassportExpirationDate;
        PassportCountryIssue = employeeDto.PassportCountryIssue;
        Race = employeeDto.Race;
        Gender = employeeDto.Gender;
        Photo = employeeDto.Photo;
        Email = employeeDto.Email;
        PersonalEmail = employeeDto.PersonalEmail;
        CellphoneNo = employeeDto.CellphoneNo;
        ClientAllocated = employeeDto.ClientAllocated;
        TeamLead = employeeDto.TeamLead;
        PhysicalAddressId = employeeDto.PhysicalAddress?.Id;
        PostalAddressId = employeeDto.PostalAddress?.Id;
        HouseNo = employeeDto.HouseNo;
        EmergencyContactName = employeeDto.EmergencyContactName;
        EmergencyContactNo = employeeDto.EmergencyContactNo;
        Active = employeeDto.Active;
        InactiveReason = employeeDto.InactiveReason;
        EmployeeType = new EmployeeType(employeeType);
        LinkedIn = employeeDto.LinkedIn;
        CV = employeeDto.CV;
        PortfolioLink = employeeDto.PortfolioLink;
        PortfolioPDF = employeeDto.PortfolioPDF;
        Referral = employeeDto.Referral;
        HighestQualification = employeeDto.HighestQualification;
        School = employeeDto.School;
        QualificationEndDate = employeeDto.QualificationEndDate;
        BlackListStatus = employeeDto.BlackListStatus;
        BlackListReason = employeeDto.BlackListReason;
        IsCandidate = employeeDto.IsCandidate;
    }

    public Employee(CandidateDto candidateDto)
    {
        Id = candidateDto.Id;
        Name = candidateDto.Name;
        Surname = candidateDto.Surname;
        PersonalEmail = candidateDto.PersonalEmail;
        Level = candidateDto.PotentialLevel;
        EmployeeTypeId = (int)candidateDto.JobPosition;
        LinkedIn = candidateDto.LinkedIn;
        Photo = candidateDto.ProfilePicture;
        CellphoneNo = candidateDto.CellphoneNumber;
        HouseNo = candidateDto.Location;
        CV = candidateDto.CV;
        PortfolioLink = candidateDto.PortfolioLink;
        PortfolioPDF = candidateDto.PortfolioPdf;
        Gender = candidateDto.Gender;
        Race = candidateDto.Race;
        IdNumber = candidateDto.IdNumber;
        Referral = candidateDto.Referral;
        HighestQualification = candidateDto.HighestQualification;
        School = candidateDto.School;
        QualificationEndDate = candidateDto.QualificationEndDate;
        BlackListStatus = candidateDto.BlacklistedStatus;
        BlackListReason = candidateDto.BlacklistedReason;
        IsCandidate = true;
    }

    [Column("employeeNumber")] public string? EmployeeNumber { get; set; }

    [Column("taxNumber")] public string? TaxNumber { get; set; }

    [Column("engagementDate")] public DateTime EngagementDate { get; set; }

    [Column("terminationDate")] public DateTime? TerminationDate { get; set; }

    [Column("peopleChampion")]
    [ForeignKey("ChampionEmployee")]
    public int? PeopleChampion { get; set; }

    [Column("disability")] public bool Disability { get; set; }

    [Column("disabilityNotes")] public string? DisabilityNotes { get; set; }

    [Column("level")] public int? Level { get; set; }

    [Column("employeeTypeId")]
    [ForeignKey("EmployeeType")]
    public int? EmployeeTypeId { get; set; }

    [Column("notes")] public string? Notes { get; set; }

    [Column("leaveInterval")] public float? LeaveInterval { get; set; }

    [Column("salaryDays")] public float? SalaryDays { get; set; }

    [Column("payRate")] public float? PayRate { get; set; }

    [Column("salary")] public int? Salary { get; set; }

    [Column("name")] public string? Name { get; set; }

    [Column("initials")] public string? Initials { get; set; }

    [Column("surname")] public string? Surname { get; set; }

    [Column("dateOfBirth")] public DateTime DateOfBirth { get; set; }

    [Column("countryOfBirth")] public string? CountryOfBirth { get; set; }

    [Column("nationality")] public string? Nationality { get; set; }

    [Column("idNumber")] public string? IdNumber { get; set; }

    [Column("passportNumber")] public string? PassportNumber { get; set; }

    [Column("passportExpirationDate")] public DateTime? PassportExpirationDate { get; set; }

    [Column("passportCountryIssue")] public string? PassportCountryIssue { get; set; }

    [Column("race")] public Race? Race { get; set; }

    [Column("gender")] public Gender? Gender { get; set; }

    [Column("photo")] public string? Photo { get; set; }

    [Column("email")] public string? Email { get; set; }

    [Column("personalEmail")] public string? PersonalEmail { get; set; }

    [Column("cellphoneNo")] public string? CellphoneNo { get; set; }

    [Column("clientAllocated")]
    [ForeignKey("ClientAssigned")]
    public int? ClientAllocated { get; set; }

    [Column("teamLead")]
    [ForeignKey("TeamLeadAssigned")]
    public int? TeamLead { get; set; }

    [Column("physicalAddress")]
    [ForeignKey("PhysicalAddress")]
    public int? PhysicalAddressId { get; set; }

    [Column("postalAddress")]
    [ForeignKey("PostalAddress")]
    public int? PostalAddressId { get; set; }

    [Column("houseNo")] public string? HouseNo { get; set; }

    [Column("emergencyContactName")] public string? EmergencyContactName { get; set; }

    [Column("emergencyContactNo")] public string? EmergencyContactNo { get; set; }

    [Column("active")] public bool Active { get; set; }

    [Column("inactiveReason")] public string? InactiveReason { get; set; }
    [Column("linkedIn")] public string? LinkedIn { get; set; }
    [Column("cv")] public string? CV { get; set; }
    [Column("portfolioLink")] public string? PortfolioLink { get; set; }
    [Column("portfolioPdf")] public string? PortfolioPDF { get; set; }
    [Column("referal")] public int? Referral { get; set; }
    [Column("highestQualification")] public string? HighestQualification { get; set; }
    [Column("school")] public string? School { get; set; }
    [Column("qualificationEndDate")] public int? QualificationEndDate { get; set; }
    [Column("blackListStatus")] public BlacklistStatus? BlackListStatus { get; set; }
    [Column("blackListReason")] public string? BlackListReason { get; set; }
    [Column("isCandidate")] public bool IsCandidate { get; set; }

    public virtual EmployeeType? EmployeeType { get; set; }
    public virtual Employee? ChampionEmployee { get; set; }
    public virtual Employee? TeamLeadAssigned { get; set; }
    public virtual Client? ClientAssigned { get; set; }
    public virtual EmployeeAddress? PhysicalAddress { get; set; }
    public virtual EmployeeAddress? PostalAddress { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public EmployeeDto ToDto()
    {
        return new EmployeeDto
        {
            Id = Id,
            EmployeeNumber = EmployeeNumber,
            TaxNumber = TaxNumber,
            EngagementDate = EngagementDate,
            TerminationDate = TerminationDate,
            PeopleChampion = PeopleChampion,
            Disability = Disability,
            DisabilityNotes = DisabilityNotes,
            Level = Level,
            EmployeeType = EmployeeType?.ToDto(),
            Notes = Notes,
            LeaveInterval = LeaveInterval,
            SalaryDays = SalaryDays,
            PayRate = PayRate,
            Salary = Salary,
            Name = Name,
            Initials = Initials,
            Surname = Surname,
            DateOfBirth = DateOfBirth,
            CountryOfBirth = CountryOfBirth,
            Nationality = Nationality,
            IdNumber = IdNumber,
            PassportNumber = PassportNumber,
            PassportExpirationDate = PassportExpirationDate,
            PassportCountryIssue = PassportCountryIssue,
            Race = Race,
            Gender = Gender,
            Photo = Photo,
            Email = Email,
            PersonalEmail = PersonalEmail,
            CellphoneNo = CellphoneNo,
            ClientAllocated = ClientAllocated,
            TeamLead = TeamLead,
            PhysicalAddress = PhysicalAddress?.ToDto(),
            PostalAddress = PostalAddress?.ToDto(),
            HouseNo = HouseNo,
            EmergencyContactName = EmergencyContactName,
            EmergencyContactNo = EmergencyContactNo,
            Active = Active,
            InactiveReason = InactiveReason,
            LinkedIn = LinkedIn,
            CV = CV,
            PortfolioLink = PortfolioLink,
            PortfolioPDF = PortfolioPDF,
            Referral = Referral,
            HighestQualification = HighestQualification,
            School = School,
            QualificationEndDate = QualificationEndDate,
            BlackListStatus = BlackListStatus,
            BlackListReason = BlackListReason,
            IsCandidate = IsCandidate
    };
    }

    public CandidateDto ToCandidateDto()
    {
        return new CandidateDto
        {
            Id = this.Id,
            Name = this.Name!,
            Surname = this.Surname!,
            PersonalEmail = this.PersonalEmail!,
            PotentialLevel = this.Level!.Value,
            JobPosition = (PositionType)this.EmployeeTypeId,
            LinkedIn = this.LinkedIn,
            ProfilePicture = this.Photo,
            CellphoneNumber = this.CellphoneNo,
            Location = this.HouseNo,
            CV = this.CV,
            PortfolioLink = this.PortfolioLink,
            PortfolioPdf = this.PortfolioPDF,
            Gender = this.Gender!.Value,
            Race = this.Race!.Value,
            IdNumber = this.IdNumber,
            Referral = this.Referral!.Value,
            HighestQualification = this.HighestQualification,
            School = this.School,
            QualificationEndDate = this.QualificationEndDate,
            BlacklistedStatus = this.BlackListStatus,
            BlacklistedReason = this.BlackListReason
        };
    }
}
