
namespace HRIS.Models;

public class EmployeeEvaluationTemplateDto
{
    public int Id { get; set; }
    public string Description { get; set; }

    public static implicit operator EmployeeEvaluationTemplateDto(EmployeeEvaluationTemplateItemDto v)
    {
        throw new NotImplementedException();
    }
}
