using HRIS.Models.Enums;

namespace HRIS.Models;

public class TerminationDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public TerminationOption TerminationOption { get; set; }
    public DateOnly DayOfNotice { get; set; }
    public DateOnly LastDayOfEmployment { get; set; }
    public bool ReemploymentStatus { get; set; }
    public bool EquipmentStatus { get; set; }
    public bool AccountsStatus { get; set; }
    public string TerminationDocument {  get; set; }
    public string DocumentName { get; set; }
    public string TerminationComments { get; set; }
}