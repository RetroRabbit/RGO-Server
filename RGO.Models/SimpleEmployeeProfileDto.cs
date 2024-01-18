using RGO.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Models;

public record SimpleEmployeeProfileDto(
    int Id,
    string? EmployeeNumber,
    string? TaxNumber,
    DateTime EngagementDate,
    DateTime? TerminationDate,
    string? PeopleChampionName,
    int? PeopleChampionId,
    bool Disability,
    string DisabilityNotes,
    int? Level,
    EmployeeTypeDto? EmployeeType,
    string? Notes,
    float? LeaveInterval,
    float? SalaryDays,
    float? PayRate,
    int? Salary,
    string Name,
    string Initials,
    string Surname,
    DateTime DateOfBirth,
    string? CountryOfBirth,
    string? Nationality,
    string IdNumber,
    string? PassportNumber,
    DateTime? PassportExpirationDate,
    string? PassportCountryIssue,
    Race? Race,
    Gender? Gender,
    string? Photo,
    string Email,
    string PersonalEmail,
    string CellphoneNo,
    string? ClientAllocatedName,
    int? ClientAllocatedId,
    string? TeamLeadName,
    int? TeamLeadId,
    EmployeeAddressDto? PhysicalAddress,
    EmployeeAddressDto? PostalAddress,
    string? HouseNo,
    string? EmergencyContactName,
    string? EmergencyContactNo
    );

