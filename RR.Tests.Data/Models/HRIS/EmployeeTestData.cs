﻿using HRIS.Models;
using HRIS.Models.Enums;

namespace RR.Tests.Data.Models.HRIS;

public class EmployeeTestData
{
    public static EmployeeDto EmployeeDto = new EmployeeDto
    {
        Id = 1,
        EmployeeNumber = "001",
        TaxNumber = "34434434",
        EngagementDate = new DateTime(),
        TerminationDate = new DateTime(),
        PeopleChampion = null,
        Disability = false,
        DisabilityNotes = "None",
        Level = 4,
        EmployeeType = EmployeeTypeTestData.DeveloperType,
        Notes = "Notes",
        LeaveInterval = 1,
        SalaryDays = 28,
        PayRate = 128,
        Salary = 100000,
        Name = "Matt",
        Initials = "MT",
        Surname = "Smith",
        DateOfBirth = new DateTime(),
        CountryOfBirth = "South Africa",
        Nationality = "South African",
        IdNumber = "0000080000000",
        PassportNumber = " ",
        PassportExpirationDate = new DateTime(),
        PassportCountryIssue = null,
        Race = Race.Black,
        Gender = Gender.Male,
        Photo = null,
        Email = "test@retrorabbit.co.za",
        PersonalEmail = "test.example@gmail.com",
        CellphoneNo = "0000000000",
        ClientAllocated = null,
        TeamLead = null,
        PhysicalAddress = EmployeeAddressTestData.EmployeeAddressDto,
        PostalAddress = EmployeeAddressTestData.EmployeeAddressDto,
        HouseNo = null,
        EmergencyContactName = null,
        EmergencyContactNo = null
    };

    public static EmployeeDto EmployeeDto2 = new EmployeeDto
    {
        Id = 2,
        EmployeeNumber = "001",
        TaxNumber = "34434434",
        EngagementDate = new DateTime(),
        TerminationDate = new DateTime(),
        PeopleChampion = null,
        Disability = false,
        DisabilityNotes = "None",
        Level = 4,
        EmployeeType = EmployeeTypeTestData.DeveloperType,
        Notes = "Notes",
        LeaveInterval = 1,
        SalaryDays = 28,
        PayRate = 128,
        Salary = 100000,
        Name = "Matt",
        Initials = "MT",
        Surname = "Smith",
        DateOfBirth = new DateTime(),
        CountryOfBirth = "South Africa",
        Nationality = "South African",
        IdNumber = "0000080000000",
        PassportNumber = " ",
        PassportExpirationDate = new DateTime(),
        PassportCountryIssue = null,
        Race = Race.Black,
        Gender = Gender.Male,
        Photo = null,
        Email = "test@retrorabbit.co.za",
        PersonalEmail = "test.example@gmail.com",
        CellphoneNo = "0000000000",
        ClientAllocated = null,
        TeamLead = null,
        PhysicalAddress = EmployeeAddressTestData.EmployeeAddressDto,
        PostalAddress = EmployeeAddressTestData.EmployeeAddressDto,
        HouseNo = null,
        EmergencyContactName = null,
        EmergencyContactNo = null
    };

    public static EmployeeDto EmployeeDto3 = new EmployeeDto
    {
        Id = 3,
        EmployeeNumber = "001",
        TaxNumber = "34434434",
        EngagementDate = new DateTime(),
        TerminationDate = new DateTime(),
        PeopleChampion = null,
        Disability = false,
        DisabilityNotes = "None",
        Level = 4,
        EmployeeType = EmployeeTypeTestData.DeveloperType,
        Notes = "Notes",
        LeaveInterval = 1,
        SalaryDays = 28,
        PayRate = 128,
        Salary = 100000,
        Name = "Matt",
        Initials = "MT",
        Surname = "Smith",
        DateOfBirth = new DateTime(),
        CountryOfBirth = "South Africa",
        Nationality = "South African",
        IdNumber = "0000080000000",
        PassportNumber = " ",
        PassportExpirationDate = new DateTime(),
        PassportCountryIssue = null,
        Race = Race.Black,
        Gender = Gender.Male,
        Photo = null,
        Email = "test@retrorabbit.co.za",
        PersonalEmail = "test.example@gmail.com",
        CellphoneNo = "0000000000",
        ClientAllocated = null,
        TeamLead = null,
        PhysicalAddress = EmployeeAddressTestData.EmployeeAddressDto,
        PostalAddress = EmployeeAddressTestData.EmployeeAddressDto,
        HouseNo = null,
        EmergencyContactName = null,
        EmergencyContactNo = null
    };

    public static EmployeeDto EmployeeDto4 = new EmployeeDto
    {
        Id = 4,
        EmployeeNumber = "001",
        TaxNumber = "34434434",
        EngagementDate = new DateTime(),
        TerminationDate = new DateTime(),
        PeopleChampion = 2,
        Disability = false,
        DisabilityNotes = "None",
        Level = 4,
        EmployeeType = EmployeeTypeTestData.DeveloperType,
        Notes = "Notes",
        LeaveInterval = 1,
        SalaryDays = 28,
        PayRate = 128,
        Salary = 100000,
        Name = "Dotty",
        Initials = "D",
        Surname = "Missile",
        DateOfBirth = new DateTime(),
        CountryOfBirth = "South Africa",
        Nationality = "South African",
        IdNumber = "5522522655",
        PassportNumber = " ",
        PassportExpirationDate = new DateTime(),
        PassportCountryIssue = null,
        Race = Race.Black,
        Gender = Gender.Male,
        Photo = null,
        Email = "dm@retrorabbit.co.za",
        PersonalEmail = "test@gmail.com",
        CellphoneNo = "0123456789",
        ClientAllocated = 1,
        TeamLead = 3,
        PhysicalAddress = EmployeeAddressTestData.EmployeeAddressDto,
        PostalAddress = EmployeeAddressTestData.EmployeeAddressDto,
        HouseNo = null,
        EmergencyContactName = null,
        EmergencyContactNo = null
    };
}
