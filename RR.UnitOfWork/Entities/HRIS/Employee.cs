using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces.HRIS;

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
    public int EmployeeTypeId { get; set; }

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

    public virtual EmployeeType? EmployeeType { get; set; }
    public virtual Employee? ChampionEmployee { get; set; }
    public virtual Employee? TeamLeadAssigned { get; set; }
    public virtual Client? ClientAssigned { get; set; }
    public virtual EmployeeAddress? PhysicalAddress { get; set; }
    public virtual EmployeeAddress? PostalAddress { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public EmployeeDto ToDto()
    {
        return new EmployeeDto(
                               Id,
                               EmployeeNumber,
                               TaxNumber,
                               EngagementDate,
                               TerminationDate,
                               PeopleChampion,
                               Disability,
                               DisabilityNotes,
                               Level,
                               EmployeeType?.ToDto(),
                               Notes,
                               LeaveInterval,
                               SalaryDays,
                               PayRate,
                               Salary,
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
                               ClientAllocated,
                               TeamLead,
                               PhysicalAddress?.ToDto(),
                               PostalAddress?.ToDto(),
                               HouseNo,
                               EmergencyContactName,
                               EmergencyContactNo);
    }
}