using RGO.Models;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

public class EmployeeEvaluationUnitTests
{
    private EmployeeDto _employee;
    private EmployeeEvaluationTemplateDto _template;

    public EmployeeEvaluationUnitTests()
    {
        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");

        _employee = new EmployeeDto(1, "001", "34434434", new DateOnly(), new DateOnly(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Ms", "Dorothy", "D",
            "Mahoko", new DateOnly(), "South Africa", "South African", "0000080000000", " ",
            new DateOnly(), null, Models.Enums.Race.Black, Models.Enums.Gender.Male, null,
            "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null);

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
