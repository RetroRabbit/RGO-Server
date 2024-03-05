namespace HRIS.Models;

public class PolicySettings
{
    public string Name { get; set; }
    public List<string> Roles { get; set; }
    public List<string> Permissions { get; set; }
}