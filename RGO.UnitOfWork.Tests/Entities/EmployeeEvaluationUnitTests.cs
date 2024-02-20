using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeEvaluationUnitTests
{
    private readonly EmployeeDto _employee;
    private readonly EmployeeEvaluationTemplateDto _template;

    public EmployeeEvaluationUnitTests()
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
    }

    private EmployeeEvaluationDto CreateEmployeeEvaluation(
        EmployeeDto? employee = null,
        EmployeeEvaluationTemplateDto? template = null,
        EmployeeDto? owner = null)
    {
        var entity = new EmployeeEvaluation
        {
            Id = 0,
            Subject = "Subject",
            StartDate = DateOnly.FromDateTime(DateTime.Now)
        };

        if (employee != null)
            entity.Employee = new Employee(employee, employee.EmployeeType);

        if (template != null)
            entity.Template = new EmployeeEvaluationTemplate(template);

        if (owner != null)
            entity.Owner = new Employee(owner, owner.EmployeeType);

        return entity.ToDto();
    }

    [Fact]
    public void InitialoizationTest()
    {
        var employeeEvaluation = new EmployeeEvaluation();
        Assert.NotNull(employeeEvaluation);
    }

    [Fact]
    public void InitialoizationWithDtoTest()
    {
        var employeeEvaluationDto = CreateEmployeeEvaluation(_employee, _template, _employee);
        var employeeEvaluation = new EmployeeEvaluation(employeeEvaluationDto);
        var dto = employeeEvaluation.ToDto();

        Assert.Null(dto.Employee);
        Assert.Null(dto.Template);
        Assert.Null(dto.Owner);
    }
}