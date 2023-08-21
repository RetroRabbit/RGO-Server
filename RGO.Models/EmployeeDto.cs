﻿using RGO.Models.Enums;

namespace RGO.Models;

public record EmployeeDto(
    int Id,
    string EmployeeNumber,
    string TaxNumber,
    DateTime EngagementDate,
    DateTime TerminationDate,
    int? ReportingLine,
    string HighestQualification,
    bool Disability,
    string DisabilityNotes,
    string CountryOfBirth,
    string Nationality,
    int Level,
    string EmployeeType,
    string Title,
    string Name,
    string Initials,
    string Surname,
    DateTime DateOfBirth,
    string IdNumber,
    string PassportNumber,
    DateTime PassportExpirationDate,
    string PassportCountryIssue,
    Race Race,
    Gender Gender,
    string KnownAs,
    string Pronouns,
    string PersonalEmail,
    string CellphoneNo,
    TShirtSize TshirtSize,
    string DietaryRestrictions);
