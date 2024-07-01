using HRIS.Models;

namespace RR.Tests.Data.Models.HRIS;

public class EmployeeEvaluationTemplateItemTestData
{
    public static EmployeeEvaluationTemplateItemDto EmployeeEvaluationTemplateItemOne = new()
    {
        Id = 1,
        Template = new EmployeeEvaluationTemplateDto { Id = 101, Description = "Template 1" },
        Section = "Example Test Section",
        Question = "Question 1"
    };

    public static EmployeeEvaluationTemplateItemDto EmployeeEvaluationTemplateItemTwo = new()
    {
        Id = 2,
        Template = new EmployeeEvaluationTemplateDto { Id = 102, Description = "Template 2" },
        Section = "Example Test Section",
        Question = "Question 2"
    };
}