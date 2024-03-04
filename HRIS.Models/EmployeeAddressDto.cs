namespace HRIS.Models;

public class EmployeeAddressDto
{
    public int Id { get; set; }
    public string? UnitNumber { get; set; }
    public string? ComplexName { get; set; }
    public string? StreetNumber { get; set; }
    public string? SuburbOrDistrict { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? PostalCode { get; set; }
}
