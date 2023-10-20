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
        bool exists = await _db.EmployeeAddress.Any(x =>
            x.EmployeeId == employeeAddressDto.EmployeeId &&
            x.UnitNumber == employeeAddressDto.UnitNumber &&
            x.ComplexName == employeeAddressDto.ComplexName &&
            x.StreetNumber == employeeAddressDto.StreetNumber &&
            x.SuburbOrDistrict == employeeAddressDto.SuburbOrDistrict &&
            x.Country == employeeAddressDto.Country &&
            x.Province == employeeAddressDto.Province &&
            x.PostalCode == employeeAddressDto.PostalCode);

        return exists;
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

        var addresses = from address in _db.EmployeeAddress.Get()
                        where address.EmployeeId == employeeAddressDto.EmployeeId &&
                        address.UnitNumber == employeeAddressDto.UnitNumber &&
                        address.ComplexName == employeeAddressDto.ComplexName &&
                        address.StreetNumber == employeeAddressDto.StreetNumber &&
                        address.SuburbOrDistrict == employeeAddressDto.SuburbOrDistrict &&
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
                            address.EmployeeId,
                            address.UnitNumber,
                            address.ComplexName,
                            address.StreetNumber,
                            address.SuburbOrDistrict,
                            address.Country,
                            address.Province,
                            address.PostalCode);
        
        return await addresses.ToListAsync();
    }

    public async Task<List<EmployeeAddressDto>> GetAllByEmployee(string email)
    {
        var addresses = from address in _db.EmployeeAddress.Get()
                        join employee in _db.Employee.Get() on address.EmployeeId equals employee.Id
                        where employee.Email == email
                        select new EmployeeAddressDto(
                            address.Id,
                            address.EmployeeId,
                            address.UnitNumber,
                            address.ComplexName,
                            address.StreetNumber,
                            address.SuburbOrDistrict,
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

        var existingAddress = await Get(employeeAddressDto);

        var addressToUpdate = new EmployeeAddressDto(
            existingAddress.Id,
            existingAddress.EmployeeId,
            employeeAddressDto.UnitNumber,
            employeeAddressDto.ComplexName,
            employeeAddressDto.StreetNumber,
            employeeAddressDto.SuburbOrDistrict,
            employeeAddressDto.Country,
            employeeAddressDto.Province,
            employeeAddressDto.PostalCode);

        var address = await _db.EmployeeAddress.Update(new EmployeeAddress(addressToUpdate));

        return address;
    }
}
