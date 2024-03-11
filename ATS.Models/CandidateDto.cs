using ATS.Models.Enums;
using HRIS.Models.Enums;

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
    public required string CellphoneNumber { get; set; }
    public string? Location { get; set; }
    public required string CV { get; set; }
    public string? PortfolioLink { get; set; }
    public string? PortfolioPdf { get; set; }
    public Gender Gender { get; set; }
    public Race Race { get; set; }
    public string? IdNumber { get; set; }
    public int Referral { get; set; }
    public string? HighestQualification { set; get; }
    public string? School { get; set; }
    public string? Degree { get; set; }
    public string? FieldOfStudy { get; set; }
    public DateOnly? QualificationStartDate { get; set; }
    public DateOnly? QualificationEndDate { get; set; }
}