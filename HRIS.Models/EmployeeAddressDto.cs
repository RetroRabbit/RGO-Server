using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class EmployeeAddressDto
{
    [Required (ErrorMessage = "Employee Address 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Employee Address 'EmployeeId' field is missing.")]
    public int EmployeeId { get; set; }
    public string? UnitNumber { get; set; }
    public string? ComplexName { get; set; }
    [Required(ErrorMessage = "Employee Address 'StreetName' field is missing.")]
    public string? StreetName { get; set; }
    [Required(ErrorMessage = "Employee Address 'StreetNumber' field is missing.")]
    public string? StreetNumber { get; set; }
    public string? SuburbOrDistrict { get; set; }
    [Required(ErrorMessage = "Employee Address 'City' field is missing.")]
    public string? City { get; set; }
    [Required(ErrorMessage = "Employee Address 'Country' field is missing.")]
    public string? Country { get; set; }
    [Required(ErrorMessage = "Employee Address 'Province' field is missing.")]
    public string? Province { get; set; }
    [Required(ErrorMessage = "Employee Address 'PostalCode' field is missing.")]
    public string? PostalCode { get; set; }
}
