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
            "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000");

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
        
        Assert.Equal(employeeEvaluationDto.Id, employeeEvaluation.Id);
        Assert.Equal(employeeEvaluationDto.Subject, employeeEvaluation.Subject);
        Assert.Equal(employeeEvaluationDto.StartDate, employeeEvaluation.StartDate);
        Assert.Equal(employeeEvaluationDto.EndDate, employeeEvaluation.EndDate);
        Assert.Equal(employeeEvaluationDto.Employee!.Id, employeeEvaluation.EmployeeId);
        Assert.Equal(employeeEvaluationDto.Template!.Id, employeeEvaluation.TemplateId);
        Assert.Equal(employeeEvaluationDto.Owner!.Id, employeeEvaluation.OwnerId);
    }

    [Fact]
    public void InitializationWithDtoNullTest()
    {
        var employeeEvaluationDto = CreateEmployeeEvaluation(_employee, _template, _employee);
        var employeeEvaluation = new EmployeeEvaluation(employeeEvaluationDto);

        Assert.Null(employeeEvaluation.Employee);
        Assert.Null(employeeEvaluation.Template);
        Assert.Null(employeeEvaluation.Owner);
    }

    [Fact]
    public void ToDtoTest()
    {
        var employeeEvaluationDto = CreateEmployeeEvaluation(_employee, _template, _employee);
        var employeeEvaluation = new EmployeeEvaluation(employeeEvaluationDto);

        employeeEvaluation.Employee = new Employee(_employee, _employee.EmployeeType);
        employeeEvaluation.Template = new EmployeeEvaluationTemplate(employeeEvaluationDto.Template!);
        employeeEvaluation.Owner = new Employee(_employee, _employee.EmployeeType);

        var employeeEvaluationDto2 = employeeEvaluation.ToDto();

        Assert.Equal(employeeEvaluation.Employee.Email, employeeEvaluationDto2.Employee!.Email);
        Assert.Equal(employeeEvaluation.Template.Description, employeeEvaluationDto2.Template!.Description);
        Assert.Equal(employeeEvaluation.Owner.Email, employeeEvaluationDto2.Owner!.Email);
    }
}
