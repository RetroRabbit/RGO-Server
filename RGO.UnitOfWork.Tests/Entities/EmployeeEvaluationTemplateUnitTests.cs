using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeEvaluationTemplateUnitTests
{
    [Fact]
    public void InitialoizationTest()
    {
        var employeeEvaluationTemplate = new EmployeeEvaluationTemplate();
        Assert.NotNull(employeeEvaluationTemplate);
    }

    [Fact]
    public void InitialoizationWithDtoTest()
    {
        var employeeEvaluationTemplateDto = new EmployeeEvaluationTemplateDto(1, "Template");
        var employeeEvaluationTemplate = new EmployeeEvaluationTemplate(employeeEvaluationTemplateDto);
        var dto = employeeEvaluationTemplate.ToDto();

        Assert.Equal(employeeEvaluationTemplateDto.Id, dto.Id);
        Assert.Equal(employeeEvaluationTemplateDto.Description, dto.Description);
    }
}