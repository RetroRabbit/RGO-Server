using HRIS.Models.Enums;

namespace HRIS.Models;

public class PropertyAccessDto
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public RoleDto? Role { get; set; }
    public string Table { get; set; }

    public string Field { get; set; }

    public PropertyAccessLevel AccessLevel { get; set; }
}