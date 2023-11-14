using RGO.Models;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

public class EmployeeEvaluationRatingUnitTests
{
    private EmployeeDto _employee;
    private EmployeeEvaluationDto _evaluation;
    private EmployeeEvaluationTemplateDto _template;

    public EmployeeEvaluationRatingUnitTests()
    {
        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        _employee = new EmployeeDto(1, "001", "34434434", new DateOnly(), new DateOnly(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dorothy", "D",
            "Mahoko", new DateOnly(), "South Africa", "South African", "0000080000000", " ",
            new DateOnly(), null, Models.Enums.Race.Black, Models.Enums.Gender.Male, null!,
            "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        _template = new EmployeeEvaluationTemplateDto(1, "Template");

        _evaluation = new EmployeeEvaluationDto(0, _employee, _template, _employee, "Subject", DateOnly.FromDateTime(DateTime.Now), null);
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
            entity.Employee = new Employee(employee, employee.EmployeeType);

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
