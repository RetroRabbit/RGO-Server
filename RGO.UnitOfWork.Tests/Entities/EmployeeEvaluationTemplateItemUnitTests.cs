using RGO.Models;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

public class EmployeeEvaluationTemplateItemUnitTests
{
    private EmployeeEvaluationTemplateDto _template;

    public EmployeeEvaluationTemplateItemUnitTests()
    {
        _template = new EmployeeEvaluationTemplateDto(1, "Template");
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
        var employeeEvaluationTemplateItemDto = new EmployeeEvaluationTemplateItemDto(1, _template, "Section", "Question");
        var employeeEvaluationTemplateItem = new EmployeeEvaluationTemplateItem(employeeEvaluationTemplateItemDto);
        var dto = employeeEvaluationTemplateItem.ToDto();

        Assert.Null(dto.Template);
    }
}
