﻿using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.Tests.Data.Models.HRIS;

public class EmployeeTestData
{
    public static Employee EmployeeOne = new()
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
        PhysicalAddress = EmployeeAddressTestData.EmployeeAddressOne,
        PostalAddress = EmployeeAddressTestData.EmployeeAddressOne,
        HouseNo = null,
        EmergencyContactName = null,
        EmergencyContactNo = null,
        Active = false
    };

    public static Employee EmployeeTwo = new()
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
        EmployeeType = EmployeeTypeTestData.DesignerType,
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
        Email = "test1@retrorabbit.co.za",
        PersonalEmail = "test.example@gmail.com",
        CellphoneNo = "0000000000",
        ClientAllocated = null,
        TeamLead = null,
        PhysicalAddress = EmployeeAddressTestData.EmployeeAddressOne,
        PostalAddress = EmployeeAddressTestData.EmployeeAddressOne,
        HouseNo = null,
        EmergencyContactName = null,
        EmergencyContactNo = null,
        Active = true
    };

    public static Employee EmployeeThree = new()
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
        EmployeeType = EmployeeTypeTestData.ScrumType,
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
        PhysicalAddress = EmployeeAddressTestData.EmployeeAddressOne,
        PostalAddress = EmployeeAddressTestData.EmployeeAddressOne,
        HouseNo = null,
        EmergencyContactName = null,
        EmergencyContactNo = null,
        Active = true
    };

    public static Employee EmployeeFour = new()
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
        EmployeeType = EmployeeTypeTestData.OtherType,
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
        PhysicalAddress = EmployeeAddressTestData.EmployeeAddressOne,
        PostalAddress = EmployeeAddressTestData.EmployeeAddressOne,
        HouseNo = null,
        EmergencyContactName = null,
        EmergencyContactNo = null,
        Active = false
    };

    public static Employee EmployeeNew = new()
    {
        Id = 0,
        EmployeeNumber = "001",
        TaxNumber = "34434434",
        EngagementDate = DateTime.UtcNow,
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
        Email = "testintegration23@retrorabbit.co.za",
        PersonalEmail = "test.example@gmail.com",
        CellphoneNo = "0000000000",
        ClientAllocated = null,
        TeamLead = null,
        PhysicalAddress = EmployeeAddressTestData.EmployeeAddressOne,
        PostalAddress = EmployeeAddressTestData.EmployeeAddressOne,
        HouseNo = null,
        EmergencyContactName = null,
        EmergencyContactNo = null,
        Active = true
    };

    public static Employee EmployeeSix = new()
    {
        Id = 1,
        EmployeeNumber = "001",
        TaxNumber = "34434434",
        EngagementDate = DateTime.Now,
        TerminationDate = DateTime.Now,
        PeopleChampion = 1,
        Disability = false,
        DisabilityNotes = "None",
        Level = 3,
        EmployeeType = EmployeeTypeTestData.PeopleChampionType,
        Notes = "Notes",
        LeaveInterval = 1,
        SalaryDays = 28,
        PayRate = 128,
        Salary = 100000,
        Name = "Estiaan",
        Initials = "MT",
        Surname = "Britz",
        DateOfBirth = DateTime.Now,
        CountryOfBirth = "South Africa",
        Nationality = "South African",
        IdNumber = "0000080000000",
        PassportNumber = " ",
        PassportExpirationDate = DateTime.Now,
        PassportCountryIssue = "South Africa",
        Race = Race.Black,
        Gender = Gender.Male,
        Photo = null,
        Email = "test1@retrorabbit.co.za",
        PersonalEmail = "test.example@gmail.com",
        CellphoneNo = "0000000000",
        ClientAllocated = null,
        TeamLead = null,
        PhysicalAddress = EmployeeAddressTestData.EmployeeAddressOne,
        PostalAddress = EmployeeAddressTestData.EmployeeAddressOne,
        HouseNo = null,
        EmergencyContactName = null,
        EmergencyContactNo = null,

    };

    public static Employee EmployeeNullType = new()
    {
        Id = 1,
        EmployeeNumber = "001",
        TaxNumber = "34434434",
        EngagementDate = DateTime.Now,
        TerminationDate = DateTime.Now,
        PeopleChampion = 1,
        Disability = false,
        DisabilityNotes = "None",
        Level = 3,
        EmployeeType = null,
        Notes = "Notes",
        LeaveInterval = 1,
        SalaryDays = 28,
        PayRate = 128,
        Salary = 100000,
        Name = "Estiaan",
        Initials = "MT",
        Surname = "Britz",
        DateOfBirth = DateTime.Now,
        CountryOfBirth = "South Africa",
        Nationality = "South African",
        IdNumber = "0000080000000",
        PassportNumber = " ",
        PassportExpirationDate = DateTime.Now,
        PassportCountryIssue = "South Africa",
        Race = Race.Black,
        Gender = Gender.Male,
        Photo = null,
        Email = "test1@retrorabbit.co.za",
        PersonalEmail = "test.example@gmail.com",
        CellphoneNo = "0000000000",
        ClientAllocated = null,
        TeamLead = null,
        PhysicalAddress = EmployeeAddressTestData.EmployeeAddressOne,
        PostalAddress = EmployeeAddressTestData.EmployeeAddressOne,
        HouseNo = null,
        EmergencyContactName = null,
        EmergencyContactNo = null,

    };
}
