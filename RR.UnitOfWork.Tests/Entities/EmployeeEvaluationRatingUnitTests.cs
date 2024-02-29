using HRIS.Models;
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
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        _employee = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                                    null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dorothy",
                                    "D",
                                    "Mahoko", new DateTime(), "South Africa", "South African", "0000080000000", " ",
                                    new DateTime(), null, Race.Black, Gender.Male, null!,
                                    "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null,
                                    employeeAddressDto, employeeAddressDto, null, null, null);

        _template = new EmployeeEvaluationTemplateDto(1, "Template");

        _evaluation = new EmployeeEvaluationDto(0, _employee, _template, _employee, "Subject",
                                                DateOnly.FromDateTime(DateTime.Now), null);
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
