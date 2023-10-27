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

    public async Task<bool> CheckIfExitsts(EmployeeAddressDto employeeAddressDto)
    {
        var exists = await _db.EmployeeAddress.GetById(employeeAddressDto.Id);
        return exists != null;
    }

    public async Task<EmployeeAddressDto> Delete(EmployeeAddressDto employeeAddressDto)
    {
        EmployeeAddressDto foundAddress = await Get(employeeAddressDto);

        var address = await _db.EmployeeAddress.Delete(foundAddress.Id);

        return address;
    }

    public async Task<EmployeeAddressDto> Get(EmployeeAddressDto employeeAddressDto)
    {
        bool exists = await CheckIfExitsts(employeeAddressDto);

        if (!exists)
            throw new Exception("Employee Address does not exist");

        var addresses = from address in _db.EmployeeAddress.Get().AsNoTracking()
                        where address.UnitNumber == employeeAddressDto.UnitNumber &&
                        address.ComplexName == employeeAddressDto.ComplexName &&
                        address.StreetNumber == employeeAddressDto.StreetNumber &&
                        address.SuburbOrDistrict == employeeAddressDto.SuburbOrDistrict &&
                        address.City == employeeAddressDto.City &&
                        address.Country == employeeAddressDto.Country &&
                        address.Province == employeeAddressDto.Province &&
                        address.PostalCode == employeeAddressDto.PostalCode
                        select address;

        var foundAddress = await addresses.FirstAsync();

        return foundAddress.ToDto();
    }

    public async Task<List<EmployeeAddressDto>> GetAll()
    {
        var addresses = from address in _db.EmployeeAddress.Get()
                        select new EmployeeAddressDto(
                            address.Id,
                            address.UnitNumber,
                            address.ComplexName,
                            address.StreetNumber,
                            address.SuburbOrDistrict,
                            address.City,
                            address.Country,
                            address.Province,
                            address.PostalCode);
        
        return await addresses.ToListAsync();
    }

    public async Task<EmployeeAddressDto> Save(EmployeeAddressDto employeeAddressDto)
    {
        bool exists = await CheckIfExitsts(employeeAddressDto);
        
        if (exists) throw new Exception("Employee Address already exists");

        var address = await _db.EmployeeAddress.Add(new EmployeeAddress(employeeAddressDto));

        return address;
    }

    public async Task<EmployeeAddressDto> Update(EmployeeAddressDto employeeAddressDto)
    {
        bool exists = await CheckIfExitsts(employeeAddressDto);

        if (!exists) throw new Exception("Employee Address does not exist");

        var existingAddress = await _db.EmployeeAddress.GetById(employeeAddressDto.Id);

        var addressToUpdate = new EmployeeAddressDto(
            existingAddress.Id,
            employeeAddressDto.UnitNumber,
            employeeAddressDto.ComplexName,
            employeeAddressDto.StreetNumber,
            employeeAddressDto.SuburbOrDistrict,
            employeeAddressDto.City,
            employeeAddressDto.Country,
            employeeAddressDto.Province,
            employeeAddressDto.PostalCode);

        var address = await _db.EmployeeAddress.Update(new EmployeeAddress(addressToUpdate));

        return address;
    }
}
