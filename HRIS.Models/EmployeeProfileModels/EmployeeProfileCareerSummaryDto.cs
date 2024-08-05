namespace HRIS.Models.EmployeeProfileModels;

public class EmployeeProfileCareerSummaryDto
{
    public EmployeeProfileSalaryDto EmployeeProfileSalary { get; set; }
    public EmployeeQualificationDto EmployeeQualification { get; set; }
    public List<WorkExperienceDto> WorkExperience { get; set; }
    public List<EmployeeCertificationDto> EmployeeCertifications { get; set; }
    public EmployeeDataDto EmployeeData { get; set; }
}