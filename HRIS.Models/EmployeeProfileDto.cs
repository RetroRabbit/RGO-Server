using HRIS.Models.Enums;

namespace HRIS.Models;
public class EmployeeProfileDto
{
    public string? AuthUserId { get; set; }
    public bool Active { get; set; }
    public string? InactiveReason { get; set; }
    public int Id { get; set; }
    public string? EmployeeNumber { get; set; }
    public string? TaxNumber { get; set; }
    public DateTime EngagementDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public string? PeopleChampionName { get; set; }
    public int? PeopleChampionId { get; set; }
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
    public byte[] Photo { get; set; } = Array.Empty<byte>();
    public string? Email { get; set; }
    public string? PersonalEmail { get; set; }
    public string? CellphoneNo { get; set; }
    public string? ClientAllocatedName { get; set; }
    public int? ClientAllocatedId { get; set; }
    public string? TeamLeadName { get; set; }
    public int? TeamLeadId { get; set; }
    public string? HouseNo { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactNo { get; set; }

    public EmployeeDto ToEmployeeDto()
    {
        return new EmployeeDto
        {
            AuthUserId = this.AuthUserId,
            Id = this.Id,
            EmployeeNumber = this.EmployeeNumber,
            EngagementDate = this.EngagementDate,
            TerminationDate = this.TerminationDate,
            PeopleChampion = this.PeopleChampionId,
            Disability = this.Disability,
            DisabilityNotes = this.DisabilityNotes,
            Level = this.Level,
            EmployeeType = this.EmployeeType,
            Notes = this.Notes,
            LeaveInterval = this.LeaveInterval,
            SalaryDays = this.SalaryDays,
            PayRate = this.PayRate,
            Salary = this.Salary,
            Name = this.Name,
            Initials = this.Initials,
            Surname = this.Surname,
            DateOfBirth = this.DateOfBirth,
            CountryOfBirth = this.CountryOfBirth,
            Nationality = this.Nationality,
            IdNumber = this.IdNumber,
            PassportNumber = this.PassportNumber,
            PassportExpirationDate = this.PassportExpirationDate,
            PassportCountryIssue = this.PassportCountryIssue,
            Race = this.Race,
            Gender = this.Gender,
            Photo = this.Photo,
            Email = this.Email,
            PersonalEmail = this.PersonalEmail,
            CellphoneNo = this.CellphoneNo,
            HouseNo = this.HouseNo,
            EmergencyContactName = this.EmergencyContactName,
            EmergencyContactNo = this.EmergencyContactNo,
            Active = this.Active,
            InactiveReason = this.InactiveReason,
            ClientAllocated = this.ClientAllocatedId,
            TeamLead = this.TeamLeadId
        };
    }
}