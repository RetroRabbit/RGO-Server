namespace HRIS.Models.EmployeeProfileModels;

public class EmployeeProfileDetailsDto
{
    public EmployeeProfileEmployeeDetailsDto EmployeeProfileDetails { get; set; }
    public EmployeeProfilePersonalDto EmployeeProfilePersonal { get; set; }
    public EmployeeProfileContactDto EmployeeProfileContact { get; set; }
    public EmployeeDataDto EmployeeData { get; set; }
    public string? Photo { get; set; }
    public bool Active { get; set; }
    public EmployeeAddressDto? PhysicalAddress { get; set; }
}