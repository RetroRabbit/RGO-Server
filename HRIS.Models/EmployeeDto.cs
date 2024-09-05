using HRIS.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class EmployeeDto
{
    
    public string? AuthUserId { get; set; }
    [Required (ErrorMessage = "Employee 'Id' field is missing.")]
    public int Id { get; set; }
    [Required (ErrorMessage = "Employee 'EmployeeNumber' field is missing.")]
    public string? EmployeeNumber { get; set; }

    [Required (ErrorMessage = "Employee 'EngagementDate' field is missing.")]
    [DataType(DataType.DateTime)]
    public DateTime EngagementDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public int? PeopleChampion { get; set; }
    [Required (ErrorMessage = "Employee 'Disability' field is missing.")]
    public bool Disability { get; set; }
    public string? DisabilityNotes { get; set; }
    [Required (ErrorMessage = "Employee 'Level' field is missing.")]
    public int? Level { get; set; }
    [Required (ErrorMessage = "Employee 'EmployeeType' feld is missing.")]
    public EmployeeTypeDto? EmployeeType { get; set; }
    public string? Notes { get; set; }
    public float? LeaveInterval { get; set; }
    public float? SalaryDays { get; set; }
    public float? PayRate { get; set; }
    public int? Salary { get; set; }
    [Required (ErrorMessage = "Employee 'Name' field is missing.")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Employee 'Initials' field is missing.")]
    public string? Initials { get; set; }
    [Required(ErrorMessage = "Employee 'Surname' field is missing.")]
    public string? Surname { get; set; }
    [Required(ErrorMessage = "Employee 'DateOfBirth' field is missing.")]
    public DateTime DateOfBirth { get; set; }
    [Required(ErrorMessage = "Employee 'CountryOfBirth' field is missing.")]
    public string? CountryOfBirth { get; set; }
    [Required(ErrorMessage = "Employee 'Nationality' field is missing.")]
    public string? Nationality { get; set; }
    [Required(ErrorMessage = "Employee 'IdNumber' field is missing.")]
    public string? IdNumber { get; set; }
    public string? PassportNumber { get; set; }
    public DateTime? PassportExpirationDate { get; set; }
    public string? PassportCountryIssue { get; set; }
    public Race? Race { get; set; }
    [Required(ErrorMessage = "Employee 'Gender' field is missing.")]
    public Gender? Gender { get; set; }
    public byte[] Photo { get; set; } = Array.Empty<byte>();
    [Required (ErrorMessage = "Employee 'Email' field is missing.")]
    [EmailAddress]
    public string? Email { get; set; }
    [Required (ErrorMessage = "Employee 'PersonalEmail' field is missing.")]
    [EmailAddress]
    public string? PersonalEmail { get; set; }
    [Required (ErrorMessage = "Employee 'CellphoneNo' field is missing.")]
    [Phone]
    public string? CellphoneNo { get; set; }
    public int? ClientAllocated { get; set; }
    public int? TeamLead { get; set; }
    public string? HouseNo { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactNo { get; set; }
    [Required(ErrorMessage = "Employee 'Active' field is missing.")]
    public bool Active { get; set; }
    public string? InactiveReason { get;set; }
}
