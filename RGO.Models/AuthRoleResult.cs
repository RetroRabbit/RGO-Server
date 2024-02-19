namespace HRIS.Models;

public class AuthRoleResult
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public AuthRoleResult(string Role,
                          string Action,
                          bool View,
                          bool Edit,
                          bool Delete)
    {
        this.Role = Role;
        this.Action = Action;
        this.View = View;
        this.Edit = Edit;
        this.Delete = Delete;
    }

    public string Role { get; set; }
    public string Action { get; set; }
    public bool View { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
}