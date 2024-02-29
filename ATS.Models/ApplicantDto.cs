namespace ATS.Models;

public class ApplicantDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Surname {  get; set; }
    public required string PersonalEmail { get; set; }
    public required string PotentialLevel { get; set; }
    public required string JobPosition {  get; set; }
    public string? LinkedIn { get; set; }
    public string? ProfilePicture { get; set; }
}