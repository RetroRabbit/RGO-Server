using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeAddress")]
public class EmployeeAddress : IModel<EmployeeAddressDto>
{
    public EmployeeAddress()
    {
    }

    public EmployeeAddress(EmployeeAddressDto dto)
    {
        Id = dto.Id;
        UnitNumber = dto.UnitNumber;
        ComplexName = dto.ComplexName;
        StreetNumber = dto.StreetNumber;
        SuburbOrDistrict = dto.SuburbOrDistrict;
        City = dto.City;
        Country = dto.Country;
        Province = dto.Province;
        PostalCode = dto.PostalCode;
    }

    [Column("unitNumber")] public string? UnitNumber { get; set; }

    [Column("complexName")] public string? ComplexName { get; set; }

    [Column("streetNumber")] public string? StreetNumber { get; set; }

    [Column("suburbOrDistrict")] public string? SuburbOrDistrict { get; set; }

    [Column("city")] public string? City { get; set; }

    [Column("country")] public string? Country { get; set; }

    [Column("province")] public string? Province { get; set; }

    [Column("postalCode")] public string? PostalCode { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public EmployeeAddressDto ToDto()
    {
        return new EmployeeAddressDto
        {
            Id = Id,
            UnitNumber = UnitNumber,
            ComplexName = ComplexName,
            StreetNumber = StreetNumber,
            SuburbOrDistrict = SuburbOrDistrict,
            City = City,
            Country = Country,
            Province = Province,
            PostalCode = PostalCode
        };
    }
}