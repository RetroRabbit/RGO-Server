using HRIS.Models.Enums;

namespace HRIS.Models;

public class EmployeeProfileContactDto
{
   // public string? AuthUserId { get; set; }
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? PersonalEmail { get; set; }
    public string? CellphoneNo { get; set; }
    public string? HouseNo { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactNo { get; set; }
}