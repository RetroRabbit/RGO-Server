using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class PolicySettings
{
    [Required(ErrorMessage = "Policy Settings 'Name' field is missing.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Policy Settings 'Roles' field is missing.")]
    public List<string> Roles { get; set; }
    [Required(ErrorMessage = "Policy Settings 'Permissions' field is missing.")]
    public List<string> Permissions { get; set; }
}