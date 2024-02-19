namespace HRIS.Models;

public record MonthlyEmployeeTotalDto(
    int Id,
    int EmployeeTotal,
    int DeveloperTotal,
    int DesignerTotal,
    int ScrumMasterTotal,
    int BusinessSupportTotal,
    string Month,
    int Year
);