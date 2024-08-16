using HRIS.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class TerminationDto
{
    [Required(ErrorMessage = "Termination 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Termination 'EmployeeId' field is missing.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Termination 'TerminationOption' field is missing.")]
    public TerminationOption TerminationOption { get; set; }
    [Required(ErrorMessage = "Termination 'DayOfNotice' field is missing.")]
    public DateTime DayOfNotice { get; set; }
    [Required(ErrorMessage = "Termination 'LastDayOfEmployment' field is missing.")]
    public DateTime LastDayOfEmployment { get; set; }
    [Required(ErrorMessage = "Termination 'ReemploymentStatus' field is missing.")]
    public bool ReemploymentStatus { get; set; }
    [Required(ErrorMessage = "Termination 'EquipmentStatus' field is missing.")]
    public bool EquipmentStatus { get; set; }
    [Required(ErrorMessage = "Termination 'AccountsStatus' field is missing.")]
    public bool AccountsStatus { get; set; }
    [Required(ErrorMessage = "Termination 'TerminationDocument' field is missing.")]
    public string TerminationDocument {  get; set; }
    [Required(ErrorMessage = "Termination 'DocumentName' field is missing.")]
    public string DocumentName { get; set; }
    [Required(ErrorMessage = "Termination 'TerminationComments' field is missing.")]
    public string TerminationComments { get; set; }
}