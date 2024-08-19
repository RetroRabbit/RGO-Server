using HRIS.Models.Employee.Commons;

namespace HRIS.Models.Employee.Profile;

public class BankingInformationDto
{
    public List<EmployeeBankingDto> EmployeeBanking { get; set; }
    public EmployeeDataDto EmployeeData { get; set; }
}