using HRIS.Models.Enums;

namespace HRIS.Models;

public class EmployeeDto
{
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
    public bool Active { get; set; }
    public string? InactiveReason { get;set; }
}
