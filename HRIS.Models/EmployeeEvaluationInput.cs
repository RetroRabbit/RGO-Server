namespace HRIS.Models;

public class EmployeeEvaluationInput
{
    public int? Id { get; set; }
    public string OwnerEmail { get; set; }
    public string EmployeeEmail { get; set; }
    public string Template { get; set; }
    public string Subject { get; set; }
}