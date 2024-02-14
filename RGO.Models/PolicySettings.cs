namespace RGO.Models;

public class PolicySettings
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public PolicySettings(string Name,
        List<string> Roles,
        List<string> Permissions)
    {
        this.Name = Name;
        this.Roles = Roles;
        this.Permissions = Permissions;
    }

    public string Name { get; set; }
    public List<string> Roles { get; set; }
    public List<string> Permissions { get; set; }
}