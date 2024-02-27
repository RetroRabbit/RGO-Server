namespace HRIS.Models;

public class AuthRoleResult
{
    public string Role { get; set; }
    public string Action { get; set; }
    public bool View { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
}
