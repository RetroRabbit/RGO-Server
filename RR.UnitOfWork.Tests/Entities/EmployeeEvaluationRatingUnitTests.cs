﻿using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeEvaluationRatingUnitTests
{
    private readonly EmployeeDto _employee;
    private readonly EmployeeEvaluationDto _evaluation;
    private readonly EmployeeEvaluationTemplateDto _template;

    public EmployeeEvaluationRatingUnitTests()
    {
        var employeeTypeDto = new EmployeeTypeDto{ Id = 1, Name = "Developer" };
        var employeeAddressDto =
            new EmployeeAddressDto{ Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };

        _employee = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = DateTime.Now,
            TerminationDate = DateTime.Now,
            PeopleChampion = null,
            Disability = false,
            DisabilityNotes = "None",
            Level = 4,
            EmployeeType = employeeTypeDto,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Dorothy",
            Initials = "D",
            Surname = "Mahoko",
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
            Email = "texample@retrorabbit.co.za",
            PersonalEmail = "test.example@gmail.com",
            CellphoneNo = "0000000000",
            ClientAllocated = null,
            TeamLead = null,
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto,
            HouseNo = null,
            EmergencyContactName = null,
            EmergencyContactNo = null
        };


        _template = new EmployeeEvaluationTemplateDto{ Id = 1, Description = "Template" };

        _evaluation = new EmployeeEvaluationDto
        {
            Id = 0,
            Employee = _employee,
            Template = _template,
            Owner = _employee,
            Subject = "Subject",
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = null
        };

    }

    private EmployeeEvaluationRatingDto CreateEmployeeEvaluationRating(
        EmployeeEvaluationDto? evaluation = null,
        EmployeeDto? employee = null)
    {
        var entity = new EmployeeEvaluationRating
        {
            Id = 0,
            Score = 1,
            Description = "Description",
            Comment = "Comment"
        };

        if (evaluation != null)
            entity.Evaluation = new EmployeeEvaluation(evaluation);

        if (employee != null)
            entity.Employee = new Employee(employee, employee.EmployeeType!);

        return entity.ToDto();
    }

    [Fact]
    public void InitialoizationTest()
    {
        var employeeEvaluationRating = new EmployeeEvaluationRating();
        Assert.NotNull(employeeEvaluationRating);
    }

    [Fact]
    public void InitialoizationWithDtoTest()
    {
        var employeeEvaluationRatingDto = CreateEmployeeEvaluationRating(_evaluation, _employee);
        var employeeEvaluationRating = new EmployeeEvaluationRating(employeeEvaluationRatingDto);
        var dto = employeeEvaluationRating.ToDto();

        Assert.Null(dto.Evaluation);
        Assert.Null(dto.Employee);
    }
}
