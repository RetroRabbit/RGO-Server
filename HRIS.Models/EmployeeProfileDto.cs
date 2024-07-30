namespace HRIS.Models;

public class EmployeeProfileDto
{
    public EmployeeDto Employee { get; set; }
    public EmployeeDataDto EmployeeData { get; set; }
    public EmployeeQualificationDto EmployeeQualification { get; set; }
    public List<WorkExperienceDto> WorkExperience { get; set; }
    public List<EmployeeCertificationDto> EmployeeCertifications { get; set; }
    public List<EmployeeBankingDto> EmployeeBanking { get; set; }
}
