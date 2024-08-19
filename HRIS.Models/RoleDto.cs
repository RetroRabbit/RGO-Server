using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class RoleDto
{
    [Required(ErrorMessage = "Role 'NQFLevel' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Role 'Description' field is missing.")]
    public string? Description { get; set; }
    [Required(ErrorMessage = "Role 'AuthRoleId' field is missing.")]
    public string? AuthRoleId { get; set; }
}