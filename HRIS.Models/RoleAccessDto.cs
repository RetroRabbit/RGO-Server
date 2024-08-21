using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class RoleAccessDto
{
    [Required(ErrorMessage = "Role Access 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Role Access 'Permission' field is missing.")]
    public string Permission { get; set; }
    [Required(ErrorMessage = "Role Access 'Grouping' field is missing.")]
    public string Grouping { get; set; }
}