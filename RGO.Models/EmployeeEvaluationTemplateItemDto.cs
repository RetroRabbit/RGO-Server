namespace RGO.Models;

public class EmployeeEvaluationTemplateItemDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeEvaluationTemplateItemDto(int Id,
        EmployeeEvaluationTemplateDto? Template,
        string Section,
        string Question)
    {
        this.Id = Id;
        this.Template = Template;
        this.Section = Section;
        this.Question = Question;
    }

    public int Id { get; set; }
    public EmployeeEvaluationTemplateDto? Template { get; set; }
    public string Section { get; set; }
    public string Question { get; set; }
}
