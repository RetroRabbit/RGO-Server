using RGO.Models;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

public class EmployeeEvaluationAudienceUnitTests
{
    private EmployeeDto _employee;
    private EmployeeEvaluationDto _evaluation;
    private EmployeeEvaluationTemplateDto _template;

    public EmployeeEvaluationAudienceUnitTests()
    {
        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");

        _employee = new EmployeeDto(1, "001", "34434434", new DateOnly(), new DateOnly(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Ms", "Dorothy", "D",
            "Mahoko", new DateOnly(), "South Africa", "South African", "0000080000000", " ",
            new DateOnly(), null, Models.Enums.Race.Black, Models.Enums.Gender.Male, null,
            "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null);

        _template = new EmployeeEvaluationTemplateDto(1, "Template");

        _evaluation = new EmployeeEvaluationDto(0, _employee, _template, _employee, "Subject", DateOnly.FromDateTime(DateTime.Now), null);
    }

    private EmployeeEvaluationAudienceDto CreateEmployeeEvaluationAudience(
        EmployeeEvaluationDto? evaluation = null,
        EmployeeDto? employee = null)
    {
        var entity = new EmployeeEvaluationAudience
        {
            Id = 0,
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
