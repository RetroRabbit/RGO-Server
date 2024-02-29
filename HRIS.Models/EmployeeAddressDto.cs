namespace HRIS.Models;

public class EmployeeAddressDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeAddressDto(int Id,
                              string UnitNumber,
                              string ComplexName,
                              string StreetNumber,
                              string SuburbOrDistrict,
                              string City,
                              string Country,
                              string Province,
                              string PostalCode)
    {
        this.Id = Id;
        this.UnitNumber = UnitNumber;
        this.ComplexName = ComplexName;
        this.StreetNumber = StreetNumber;
        this.SuburbOrDistrict = SuburbOrDistrict;
        this.City = City;
        this.Country = Country;
        this.Province = Province;
        this.PostalCode = PostalCode;
    }

    public EmployeeAddressDto()
    {
    }

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