using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class ChartRoleLinkDto
{
    [Required(ErrorMessage = "Chart Role Link 'Id' field is missing.")]
    public int Id { get; set; }
    public ChartDto? Chart { get; set; }
    public RoleDto? Role { get; set; }
}
