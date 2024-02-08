namespace RGO.Models;

public class PropertyAccessDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public PropertyAccessDto(int Id,
        RoleDto? Role,
        int Condition,
        FieldCodeDto? FieldCode)
    {
        this.Id = Id;
        this.Role = Role;
        this.Condition = Condition;
        this.FieldCode = FieldCode;
    }

    public int Id { get; set; }
    public RoleDto? Role { get; set; }
    public int Condition { get; set; }
    public FieldCodeDto? FieldCode { get; set; }
}
