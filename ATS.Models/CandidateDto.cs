using HRIS.Models.Enums;

namespace ATS.Models;

public class CandidateDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Surname {  get; set; }
    public required string PersonalEmail { get; set; }
    public required int PotentialLevel { get; set; }
    public  PositionType JobPosition {  get; set; }
    public string? LinkedIn { get; set; }
    public string? ProfilePicture { get; set; }
    public string? CellphoneNumber { get; set; }
    public string? Location { get; set; }
    public string? CV { get; set; }
    public string? PortfolioLink { get; set; }
    public string? PortfolioPdf { get; set; }
    public Gender Gender { get; set; }
    public Race Race { get; set; }
    public string? IdNumber { get; set; }
    public int Referral { get; set; }
    public string? HighestQualification { set; get; }
    public string? School { get; set; }
    public int? QualificationEndDate { get; set; }
    public BlacklistStatus BlacklistedStatus { get; set;}
    public string? BlacklistedReason { get; set; }
}
