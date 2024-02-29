using HRIS.Models.Enums;

namespace HRIS.Models;

public class EmployeeDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeDto(int Id,
                       string? EmployeeNumber,
                       string? TaxNumber,
                       DateTime EngagementDate,
                       DateTime? TerminationDate,
                       int? PeopleChampion,
                       bool Disability,
                       string? DisabilityNotes,
                       int? Level,
                       EmployeeTypeDto? EmployeeType,
                       string? Notes,
                       float? LeaveInterval,
                       float? SalaryDays,
                       float? PayRate,
                       int? Salary,
                       string? Name,
                       string? Initials,
                       string? Surname,
                       DateTime DateOfBirth,
                       string? CountryOfBirth,
                       string? Nationality,
                       string? IdNumber,
                       string? PassportNumber,
                       DateTime? PassportExpirationDate,
                       string? PassportCountryIssue,
                       Race? Race,
                       Gender? Gender,
                       string? Photo,
                       string? Email,
                       string? PersonalEmail,
                       string? CellphoneNo,
                       int? ClientAllocated,
                       int? TeamLead,
                       EmployeeAddressDto? PhysicalAddress,
                       EmployeeAddressDto? PostalAddress,
                       string? HouseNo,
                       string? EmergencyContactName,
                       string? EmergencyContactNo)
    {
        this.Id = Id;
        this.EmployeeNumber = EmployeeNumber;
        this.TaxNumber = TaxNumber;
        this.EngagementDate = EngagementDate;
        this.TerminationDate = TerminationDate;
        this.PeopleChampion = PeopleChampion;
        this.Disability = Disability;
        this.DisabilityNotes = DisabilityNotes;
        this.Level = Level;
        this.EmployeeType = EmployeeType;
        this.Notes = Notes;
        this.LeaveInterval = LeaveInterval;
        this.SalaryDays = SalaryDays;
        this.PayRate = PayRate;
        this.Salary = Salary;
        this.Name = Name;
        this.Initials = Initials;
        this.Surname = Surname;
        this.DateOfBirth = DateOfBirth;
        this.CountryOfBirth = CountryOfBirth;
        this.Nationality = Nationality;
        this.IdNumber = IdNumber;
        this.PassportNumber = PassportNumber;
        this.PassportExpirationDate = PassportExpirationDate;
        this.PassportCountryIssue = PassportCountryIssue;
        this.Race = Race;
        this.Gender = Gender;
        this.Photo = Photo;
        this.Email = Email;
        this.PersonalEmail = PersonalEmail;
        this.CellphoneNo = CellphoneNo;
        this.ClientAllocated = ClientAllocated;
        this.TeamLead = TeamLead;
        this.PhysicalAddress = PhysicalAddress;
        this.PostalAddress = PostalAddress;
        this.HouseNo = HouseNo;
        this.EmergencyContactName = EmergencyContactName;
        this.EmergencyContactNo = EmergencyContactNo;
    }

    public int Id { get; set; }
    public string? EmployeeNumber { get; set; }
    public string? TaxNumber { get; set; }
    public DateTime EngagementDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public int? PeopleChampion { get; set; }
    public bool Disability { get; set; }
    public string? DisabilityNotes { get; set; }
    public int? Level { get; set; }
    public EmployeeTypeDto? EmployeeType { get; set; }
    public string? Notes { get; set; }
    public float? LeaveInterval { get; set; }
    public float? SalaryDays { get; set; }
    public float? PayRate { get; set; }
    public int? Salary { get; set; }
    public string? Name { get; set; }
    public string? Initials { get; set; }
    public string? Surname { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? CountryOfBirth { get; set; }
    public string? Nationality { get; set; }
    public string? IdNumber { get; set; }
    public string? PassportNumber { get; set; }
    public DateTime? PassportExpirationDate { get; set; }
    public string? PassportCountryIssue { get; set; }
    public Race? Race { get; set; }
    public Gender? Gender { get; set; }
    public string? Photo { get; set; }
    public string? Email { get; set; }
    public string? PersonalEmail { get; set; }
    public string? CellphoneNo { get; set; }
    public int? ClientAllocated { get; set; }
    public int? TeamLead { get; set; }
    public EmployeeAddressDto? PhysicalAddress { get; set; }
    public EmployeeAddressDto? PostalAddress { get; set; }
    public string? HouseNo { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactNo { get; set; }
}