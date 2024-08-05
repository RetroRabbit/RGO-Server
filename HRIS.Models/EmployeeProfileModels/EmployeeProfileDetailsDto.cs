namespace HRIS.Models.EmployeeProfileModels;

public class EmployeeProfileDetailsDto
{
    public EmployeeDetailsDto EmployeeProfileDetails { get; set; }
    public PersonalDetailsDto EmployeeProfilePersonal { get; set; } // TODO: Endpoint for accordion
    public ContactDetailsDto EmployeeProfileContact { get; set; } // TODO: Endpoint for accordion
    public EmployeeDataDto EmployeeData { get; set; }
    public string? Photo { get; set; } 
    public bool Active { get; set; }
    public EmployeeAddressDto? PhysicalAddress { get; set; } // TODO: Endpoint for accordion 
}