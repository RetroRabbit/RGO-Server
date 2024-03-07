using ATS.Models.Enums;

namespace ATS.Models;

public class CandidateDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Surname {  get; set; }
    public required string PersonalEmail { get; set; }
    public required int PotentialLevel { get; set; }
    public required PositionType JobPosition {  get; set; }
    public string? LinkedIn { get; set; }
    public string? ProfilePicture { get; set; }
}