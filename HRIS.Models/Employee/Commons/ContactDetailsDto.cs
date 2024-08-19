using HRIS.Models.Enums;

namespace HRIS.Models.Employee.Commons;

public class ContactDetailsDto
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? PersonalEmail { get; set; }
    public string? CellphoneNo { get; set; }
    public string? HouseNo { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactNo { get; set; }
}