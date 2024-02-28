namespace HRIS.Models;

public class MonthlyEmployeeTotalDto
{
    public int Id { get; set; }
    public int EmployeeTotal { get; set; }
    public int DeveloperTotal { get; set; }
    public int DesignerTotal { get; set; }
    public int ScrumMasterTotal { get; set; }
    public int BusinessSupportTotal { get; set; }
    public string Month { get; set; }
    public int Year { get; set; }
}
