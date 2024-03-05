namespace HRIS.Models;

public class PropertyAccessDto
{
    public int Id { get; set; }
    public RoleDto? Role { get; set; }
    public int Condition { get; set; }
    public FieldCodeDto? FieldCode { get; set; }
}