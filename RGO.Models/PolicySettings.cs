namespace RGO.Models;

public record PolicySettings(
    string Name,
    List<string> Roles,
    List<string> Permissions);