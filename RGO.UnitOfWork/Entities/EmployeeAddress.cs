using RGO.Models;
using RGO.Models.Enums;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("EmployeeAddress")]
public class EmployeeAddress : IModel<EmployeeAddressDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("addressType")]
    public AddressType AddressType { get; set; }

    [Column("unitNumber")]
    public string UnitNumber { get; set; }

    [Column("complexName")]
    public string ComplexName { get; set; }

    [Column("streetNumber")]
    public string StreetNumber { get; set; }

    [Column("streetName")]
    public string StreetName { get; set; }

    [Column("suburb")]
    public string Suburb { get; set; }

    [Column("city")]
    public string City { get; set; }

    [Column("postalCode")]
    public string PostalCode { get; set; }

    public virtual Employee Employee { get; set; }

    public EmployeeAddress() { }

    public EmployeeAddress(EmployeeAddressDto employeeAddressDto)
    {
        Id = employeeAddressDto.Id;
        EmployeeId = employeeAddressDto.Employee.Id;
        AddressType = employeeAddressDto.AddressType;
        UnitNumber = employeeAddressDto.UnitNumber;
        ComplexName = employeeAddressDto.ComplexName;
        StreetNumber = employeeAddressDto.StreetNumber;
        StreetName = employeeAddressDto.StreetName;
        Suburb = employeeAddressDto.Suburb;
        City = employeeAddressDto.City;
        PostalCode = employeeAddressDto.PostalCode;
    }

    public EmployeeAddressDto ToDto()
    {
        return new EmployeeAddressDto(
            Id,
            Employee.ToDto(),
            AddressType,
            UnitNumber,
            ComplexName,
            StreetNumber,
            StreetName,
            Suburb,
            City,
            PostalCode);
    }
}
