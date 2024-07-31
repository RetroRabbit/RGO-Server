using HRIS.Models.Enums;

namespace HRIS.Models;

public class EmployeeProfilePersonalDto
{
    //public string? AuthUserId { get; set; }
    public int Id { get; set; }
    public bool Disability { get; set; }
    public string? DisabilityNotes { get; set; }
    public string? CountryOfBirth { get; set; }
    public string? Nationality { get; set; }
    public Race? Race { get; set; }
    public Gender? Gender { get; set; }
}
