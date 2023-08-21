namespace RGO.Models;

public record AuthRoleResult(
    string Role,
    string Action,
    bool View,
    bool Edit,
    bool Delete);