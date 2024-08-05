namespace HRIS.Models.EmployeeProfileModels;

public class EmployeeProfileBankingInformationDto
{
    public EmployeeProfileSalaryDto EmployeeProfileSalary { get; set; }
    public List<EmployeeBankingDto> EmployeeBanking { get; set; }
    public EmployeeDataDto EmployeeData { get; set; }
}