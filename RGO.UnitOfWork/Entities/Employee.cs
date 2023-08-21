using RGO.Models;
using RGO.Models.Enums;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("Employee")]
public class Employee : IModel<EmployeeDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("employeeNumber")]
    public string EmployeeNumber { get; set; }

    [Column("taxNumber")]
    public string TaxNumber { get; set; }

    [Column("engagementDate")]
    public DateTime EngagementDate { get; set; }

    [Column("terminationDate")]
    public DateTime TerminationDate { get; set; }

    [Column("reportingLine")]
    public int? ReportingLine { get; set; }

    [Column("highestQualification")]
    public string HighestQualification { get; set; }

    [Column("disability")]
    public bool Disability { get; set; }

    [Column("disabilityNotes")]
    public string DisabilityNotes { get; set; }

    [Column("countryOfBirth")]
    public string CountryOfBirth { get; set; }

    [Column("nationality")]
    public string Nationality { get; set; }

    [Column("level")]
    public int Level { get; set; }

    [Column("employeeTypeId")]
    [ForeignKey("EmployeeType")]
    public int EmployeeTypeId { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("initials")]
    public string Initials { get; set; }

    [Column("surname")]
    public string Surname { get; set; }

    [Column("dateOfBirth")]
    public DateTime DateOfBirth { get; set; }

    [Column("idNumber")]
    public string IdNumber { get; set; }

    [Column("passportNumber")]
    public string PassportNumber { get; set; }

    [Column("passportExpirationDate")]
    public DateTime PassportExpirationDate { get; set; }

    [Column("passportCountryIssue")]
    public string PassportCountryIssue { get; set; }

    [Column("race")]
    public Race Race { get; set; }

    [Column("gender")]
    public Gender Gender { get; set; }

    [Column("knownAs")]
    public string KnownAs { get; set; }

    [Column("pronouns")]
    public string Pronouns { get; set; }

    [Column("personalEmail")]
    public string PersonalEmail { get; set; }

    [Column("cellphoneNo")]
    public string CellphoneNo { get; set; }

    [Column("tshirtSize")]
    public TShirtSize TshirtSize { get; set; }

    [Column("dietaryRestrictions")]
    public string DietaryRestrictions { get; set; }

    public virtual EmployeeType EmployeeType { get; set; }

    public Employee() { }

    public Employee(EmployeeDto employeeDto, EmployeeTypeDto employeeType)
    {
        Id = employeeDto.Id;
        EmployeeNumber = employeeDto.EmployeeNumber;
        TaxNumber = employeeDto.TaxNumber;
        EngagementDate = employeeDto.EngagementDate;
        TerminationDate = employeeDto.TerminationDate;
        ReportingLine = employeeDto.ReportingLine;
        HighestQualification = employeeDto.HighestQualification;
        Disability = employeeDto.Disability;
        DisabilityNotes = employeeDto.DisabilityNotes;
        CountryOfBirth = employeeDto.CountryOfBirth;
        Nationality = employeeDto.Nationality;
        Level = employeeDto.Level;
        EmployeeTypeId = employeeType.Id;
        Title = employeeDto.Title;
        Initials = employeeDto.Initials;
        Name = employeeDto.Name;
        Surname = employeeDto.Surname;
        DateOfBirth = employeeDto.DateOfBirth;
        IdNumber = employeeDto.IdNumber;
        PassportNumber = employeeDto.PassportNumber;
        PassportExpirationDate = employeeDto.PassportExpirationDate;
        PassportCountryIssue = employeeDto.PassportCountryIssue;
        Race = employeeDto.Race;
        Gender = employeeDto.Gender;
        KnownAs = employeeDto.KnownAs;
        Pronouns = employeeDto.Pronouns;
        PersonalEmail = employeeDto.PersonalEmail;
        CellphoneNo = employeeDto.CellphoneNo;
        TshirtSize = employeeDto.TshirtSize;
        DietaryRestrictions = employeeDto.DietaryRestrictions;
    }

    public EmployeeDto ToDto()
    {
        return new EmployeeDto(
            Id,
            EmployeeNumber,
            TaxNumber,
            EngagementDate,
            TerminationDate,
            ReportingLine,
            HighestQualification,
            Disability,
            DisabilityNotes,
            CountryOfBirth,
            Nationality,
            Level,
            EmployeeType.Name,
            Title,
            Name,
            Initials,
            Surname,
            DateOfBirth,
            IdNumber,
            PassportNumber,
            PassportExpirationDate,
            PassportCountryIssue,
            Race,
            Gender,
            KnownAs,
            Pronouns,
            PersonalEmail,
            CellphoneNo,
            TshirtSize,
            DietaryRestrictions);
    }
}
