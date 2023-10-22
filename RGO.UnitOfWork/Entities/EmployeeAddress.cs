using RGO.Models;
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

    [Column("unitNumber")]
    public string UnitNumber { get; set; }

    [Column("complexName")]
    public string ComplexName { get; set; }

    [Column("streetNumber")]
    public string StreetNumber { get; set; }

    [Column("suburbOrDistrict")]
    public string SuburbOrDistrict { get; set; }

    [Column("country")]
    public string Country { get; set; }

    [Column("province")]
    public string Province { get; set; }

    [Column("postalCode")]
    public string PostalCode { get; set; }

    public EmployeeAddress() { }

    public EmployeeAddress(EmployeeAddressDto dto)
    {
        Id = dto.Id;
        UnitNumber = dto.UnitNumber;
        ComplexName = dto.ComplexName;
        StreetNumber = dto.StreetNumber;
        SuburbOrDistrict = dto.SuburbOrDistrict;
        Country = dto.Country;
        Province = dto.Province;
        PostalCode = dto.PostalCode;
    }

    public EmployeeAddressDto ToDto()
    {
        return new EmployeeAddressDto(
            Id,
            UnitNumber,
            ComplexName,
            StreetNumber,
            SuburbOrDistrict,
            Country,
            Province,
            PostalCode);
    }
}
