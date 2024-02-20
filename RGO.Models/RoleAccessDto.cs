namespace HRIS.Models;

public class RoleAccessDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public RoleAccessDto(int Id,
                         string Permission,
                         string Grouping)
    {
        this.Id = Id;
        this.Permission = Permission;
        this.Grouping = Grouping;
    }

    public int Id { get; set; }
    public string Permission { get; set; }
    public string Grouping { get; set; }
}