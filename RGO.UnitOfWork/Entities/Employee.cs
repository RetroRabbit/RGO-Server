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
    public DateOnly EngagementDate { get; set; }

    [Column("terminationDate")]
    public DateOnly? TerminationDate { get; set; }

    [Column("reportingLine")]
    [ForeignKey("ReportingEmployee")]
    public int? ReportingLine { get; set; }

    [Column("disability")]
    public bool Disability { get; set; }

    [Column("disabilityNotes")]
    public string DisabilityNotes { get; set; }

    [Column("level")]
    public int Level { get; set; }

    [Column("employeeTypeId")]
    [ForeignKey("EmployeeType")]
    public int EmployeeTypeId { get; set; }

    [Column("notes")]
    public string Notes { get; set; }

    [Column("leaveInterval")]
    public float LeaveInterval { get; set; }

    [Column("salaryDays")]
    public float SalaryDays { get; set; }

    [Column("payRate")]
    public float PayRate { get; set; }

    [Column("salary")]
    public int Salary{ get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("initials")]
    public string Initials { get; set; }

    [Column("surname")]
    public string Surname { get; set; }

    [Column("dateOfBirth")]
    public DateOnly DateOfBirth { get; set; }

    [Column("countryOfBirth")]
    public string CountryOfBirth { get; set; }

    [Column("nationality")]
    public string Nationality { get; set; }

    [Column("idNumber")]
    public string? IdNumber { get; set; }

    [Column("passportNumber")]
    public string? PassportNumber { get; set; }

    [Column("passportExpirationDate")]
    public DateOnly? PassportExpirationDate { get; set; }

    [Column("passportCountryIssue")]
    public string? PassportCountryIssue { get; set; }

    [Column("race")]
    public Race Race { get; set; }

    [Column("gender")]
    public Gender Gender { get; set; }

    [Column("photo")]
    public string Photo { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("personalEmail")]
    public string PersonalEmail { get; set; }

    [Column("cellphoneNo")]
    public string CellphoneNo { get; set; }

    [Column("clientAllocated")]
    [ForeignKey("ClientAssigned")]
    public int? ClientAllocated { get; set; }

    [Column("teamLead")]
    [ForeignKey("TeamLeadAssigned")]
    public int? TeamLead { get; set; }

    public virtual EmployeeType EmployeeType { get; set; }
    public virtual Employee ReportingEmployee { get; set; }
    public virtual Employee TeamLeadAssigned { get; set; }
    public virtual Client ClientAssigned { get; set; }

    public Employee() { }

    public Employee(EmployeeDto employeeDto, EmployeeTypeDto employeeType)
    {
        Id = employeeDto.Id;
        EmployeeNumber = employeeDto.EmployeeNumber;
        TaxNumber = employeeDto.TaxNumber;
        EngagementDate = employeeDto.EngagementDate;
        TerminationDate = employeeDto.TerminationDate;
        ReportingLine = employeeDto.ReportingLine?.Id;
        Disability = employeeDto.Disability;
        DisabilityNotes = employeeDto.DisabilityNotes;
        Level = employeeDto.Level;
        EmployeeTypeId = employeeType.Id;
        Notes = employeeDto.Notes;
        LeaveInterval = employeeDto.LeaveInterval;
        SalaryDays = employeeDto.SalaryDays;
        PayRate = employeeDto.PayRate;
        Salary = employeeDto.Salary;
        Title = employeeDto.Title;
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
        ClientAllocated = employeeDto.ClientAllocated?.Id;
        TeamLead = employeeDto.TeamLead?.Id;
    }

    public EmployeeDto ToDto()
     {
        return new EmployeeDto(
            Id,
            EmployeeNumber,
            TaxNumber,
            EngagementDate,
            TerminationDate,
            ReportingEmployee?.ToDto(),
            Disability,
            DisabilityNotes,
            Level,
            EmployeeType?.ToDto(),
            Notes,
            LeaveInterval,
            SalaryDays,
            PayRate,
            Salary,
            Title,
            Name,
            Initials,
            Surname,
            DateOfBirth,
            CountryOfBirth,
            Nationality,
            IdNumber,
            PassportNumber,
            PassportExpirationDate,
            PassportCountryIssue,
            Race,
            Gender,
            Photo,
            Email,
            PersonalEmail,
            CellphoneNo,
            ClientAssigned?.ToDto(),
            TeamLeadAssigned?.ToDto());
    }
}
