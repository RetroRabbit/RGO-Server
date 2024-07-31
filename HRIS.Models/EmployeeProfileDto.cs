using Org.BouncyCastle.Bcpg;

namespace HRIS.Models;

public class EmployeeProfileDto
{
    public EmployeeProfileDetailsDto EmployeeProfileDetails { get; set; }
    public EmployeeProfilePersonalDto EmployeeProfilePersonal { get; set; }
    public EmployeeProfileContactDto EmployeeProfileContact { get; set; }
    public EmployeeProfileSalaryDto EmployeeProfileSalary { get; set; }
    public EmployeeDataDto EmployeeData { get; set; }
    public EmployeeQualificationDto EmployeeQualification { get; set; }
    public List<WorkExperienceDto> WorkExperience { get; set; }
    public List<EmployeeCertificationDto> EmployeeCertifications { get; set; }
    public List<EmployeeBankingDto> EmployeeBanking { get; set; }
    public string? AuthUserId { get; set; }
    public string? EmployeeNumber { get; set; }
    public DateTime? TerminationDate { get; set; }
    public string? Notes { get; set; }
    public string? PassportNumber { get; set; }
    public DateTime? PassportExpirationDate { get; set; }
    public string? PassportCountryIssue { get; set; }
    public string? Photo { get; set; }
    public bool Active { get; set; }
    public string? InactiveReason { get; set; }
}
