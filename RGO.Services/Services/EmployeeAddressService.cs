using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

public class EmployeeAddressService : IEmployeeAddressService
{
    private readonly IUnitOfWork _db;

    public EmployeeAddressService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<bool> CheckIfExists(EmployeeAddressDto employeeAddressDto)
    {
        var exists = await _db.EmployeeAddress.GetById(employeeAddressDto.Id);
        return exists != null;
    }

    public async Task<EmployeeAddressDto> Delete(int addressId)
    {
        var address = await _db.EmployeeAddress.Delete(addressId);
        return address;
    }

    public async Task<EmployeeAddressDto> Get(EmployeeAddressDto employeeAddressDto)
    {
        var exists = await CheckIfExists(employeeAddressDto);

        if (!exists) throw new Exception("Employee Address does not exist");

        return await _db.EmployeeAddress.FirstOrDefault(address =>
                            address.UnitNumber == employeeAddressDto.UnitNumber &&
                            address.ComplexName == employeeAddressDto.ComplexName &&
                            address.StreetNumber == employeeAddressDto.StreetNumber &&
                            address.SuburbOrDistrict == employeeAddressDto.SuburbOrDistrict &&
                            address.City == employeeAddressDto.City &&
                            address.Country == employeeAddressDto.Country &&
                            address.Province == employeeAddressDto.Province &&
                            address.PostalCode == employeeAddressDto.PostalCode);
    }

    public async Task<List<EmployeeAddressDto>> GetAll()
    {
        return await _db.EmployeeAddress.GetAll();
    }

    public async Task<EmployeeAddressDto> Save(EmployeeAddressDto employeeAddressDto)
    {
        var exists = await CheckIfExists(employeeAddressDto);

        if (exists) throw new Exception("Employee Address already exists");

        var address = await _db.EmployeeAddress.Add(new EmployeeAddress(employeeAddressDto));

        return address;
    }

    public async Task<EmployeeAddressDto> Update(EmployeeAddressDto employeeAddressDto)
    {
        var exists = await CheckIfExists(employeeAddressDto);

        if (!exists) throw new Exception("Employee Address does not exist");

        var address = await _db.EmployeeAddress.Update(new EmployeeAddress(employeeAddressDto));

        return address;
    }
}
