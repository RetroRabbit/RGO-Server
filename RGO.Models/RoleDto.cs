namespace RGO.Models;

public class RoleDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public RoleDto(int Id,
        string Description)
    {
        this.Id = Id;
        this.Description = Description;
    }

    public int Id { get; set; }
    public string Description { get; set; }
}