using HRIS.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class PropertyAccessDto
{
    [Required(ErrorMessage = "Property Access 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Property Access 'RoleId' field is missing.")]
    public int RoleId { get; set; }
    public RoleDto? Role { get; set; }
    public string? Table { get; set; }
    public string? Field { get; set; }
    [Required(ErrorMessage = "Property Access 'AccessLevel' field is missing.")]
    public PropertyAccessLevel AccessLevel { get; set; }
}