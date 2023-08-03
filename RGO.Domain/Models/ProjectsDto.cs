namespace RGO.Domain.Models;

public record ProjectsDto
(
  int Id,
  int UserId,
  string Name,
  string Description,
  string Role
);
