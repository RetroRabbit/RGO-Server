﻿using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeEvaluationAudienceUnitTests
{
    private readonly EmployeeDto _employee;
    private readonly EmployeeEvaluationDto _evaluation;
    private readonly EmployeeEvaluationTemplateDto _template;

    public EmployeeEvaluationAudienceUnitTests()
    {
        var employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        _employee = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                                    null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dorothy",
                                    "D",
                                    "Mahoko", new DateTime(), "South Africa", "South African", "0000080000000", " ",
                                    new DateTime(), null, Race.Black, Gender.Male, null,
                                    "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null,
                                    employeeAddressDto, employeeAddressDto, null, null, null);

        _template = new EmployeeEvaluationTemplateDto(1, "Template");

        _evaluation = new EmployeeEvaluationDto(0, _employee, _template, _employee, "Subject",
                                                DateOnly.FromDateTime(DateTime.Now), null);
    }

    private EmployeeEvaluationAudienceDto CreateEmployeeEvaluationAudience(
        EmployeeEvaluationDto? evaluation = null,
        EmployeeDto? employee = null)
    {
        var entity = new EmployeeEvaluationAudience
        {
            Id = 0
        };

        if (evaluation != null)
            entity.Evaluation = new EmployeeEvaluation(evaluation);

        if (employee != null)
            entity.Employee = new Employee(employee, employee.EmployeeType);

        return entity.ToDto();
    }

    [Fact]
    public void InitialoizationTest()
    {
        var employeeEvaluationAudience = new EmployeeEvaluationAudience();
        Assert.NotNull(employeeEvaluationAudience);
    }

    [Fact]
    public void InitialoizationWithDtoTest()
    {
        var employeeEvaluationAudienceDto = CreateEmployeeEvaluationAudience(_evaluation, _employee);
        var employeeEvaluationAudience = new EmployeeEvaluationAudience(employeeEvaluationAudienceDto);
        var dto = employeeEvaluationAudience.ToDto();

        Assert.Null(dto.Evaluation);
        Assert.Null(dto.Employee);
    }
}