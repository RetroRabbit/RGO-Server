using HRIS.Models.Employee.Commons;

namespace HRIS.Models.Employee.Profile;

public class CareerSummaryDto
{
    public SalaryDetailsDto EmployeeProfileSalary { get; set; } // TODO: Endpoint for accordion
    public EmployeeQualificationDto EmployeeQualification { get; set; }
    public List<WorkExperienceDto> WorkExperience { get; set; }
    public List<EmployeeCertificationDto> EmployeeCertifications { get; set; }
    public EmployeeDataDto EmployeeData { get; set; }
}