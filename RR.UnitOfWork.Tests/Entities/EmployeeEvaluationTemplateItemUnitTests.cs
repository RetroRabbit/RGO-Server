using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeEvaluationTemplateItemUnitTests
{
    private readonly EmployeeEvaluationTemplateDto _template;

    public EmployeeEvaluationTemplateItemUnitTests()
    {
        _template = new EmployeeEvaluationTemplateDto{ Id = 1, Description = "Template" };
    }

    [Fact]
    public void InitialoizationTest()
    {
        var employeeEvaluationTemplateItem = new EmployeeEvaluationTemplateItem();
        Assert.NotNull(employeeEvaluationTemplateItem);
    }

    [Fact]
    public void InitialoizationWithDtoTest()
    {
        var employeeEvaluationTemplateItemDto =
            new EmployeeEvaluationTemplateItemDto { Id = 1, Template = _template, Section = "Section", Question = "Question"};
        var employeeEvaluationTemplateItem = new EmployeeEvaluationTemplateItem(employeeEvaluationTemplateItemDto);
        var dto = employeeEvaluationTemplateItem.ToDto();

        Assert.Null(dto.Template);
    }
}
