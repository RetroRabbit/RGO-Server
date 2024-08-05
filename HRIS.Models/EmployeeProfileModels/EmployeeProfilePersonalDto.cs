using HRIS.Models.Enums;

namespace HRIS.Models.EmployeeProfileModels;

public class EmployeeProfilePersonalDto
{
    public int Id { get; set; }
    public bool Disability { get; set; }
    public string? DisabilityNotes { get; set; }
    public string? CountryOfBirth { get; set; }
    public string? Nationality { get; set; }
    public Race? Race { get; set; }
    public Gender? Gender { get; set; }
}
