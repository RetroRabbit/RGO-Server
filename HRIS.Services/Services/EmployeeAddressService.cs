using HRIS.Models;
using HRIS.Services.Interfaces;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeAddressService : IEmployeeAddressService
{
    private readonly IUnitOfWork _db;

    public EmployeeAddressService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<bool> CheckIfExists(int id)
    {
        if (id == 0)
        {
            return false;
        }

        var returnVal = await _db.EmployeeAddress.Any(ea => ea.Id == id);
        return returnVal;
    }

    public async Task<EmployeeAddressDto> Delete(int addressId)
    {
        var exists = await CheckIfExists(addressId);

        if (!exists)
        {
            throw new CustomException("Employee Address Does Not Exist");
        }

        var address = await _db.EmployeeAddress.Delete(addressId);
        return address.ToDto();
    }

    public async Task<EmployeeAddressDto> Get(int id)
    {
        var exists = await CheckIfExists(id);

        if (!exists)
        {
            throw new CustomException("Employee Address Does Not Exist");
        }

        var address = await _db.EmployeeAddress.FirstOrDefault(address => address.Id == id);
        return address!.ToDto();
    }

    public async Task<List<EmployeeAddressDto>> GetAll()
    {
        return (await _db.EmployeeAddress.GetAll()).Select(x => x.ToDto()).ToList();
    }

    public async Task<EmployeeAddressDto> Create(EmployeeAddressDto employeeAddressDto)
    {
        var exists = await CheckIfExists(employeeAddressDto.Id);

        if (exists)
        {
            throw new CustomException("Employee Address Already Exists");
        }

        var address = await _db.EmployeeAddress.Add(new EmployeeAddress(employeeAddressDto));

        return address.ToDto();
    }

    public async Task<EmployeeAddressDto> Update(EmployeeAddressDto employeeAddressDto)
    {
        var exists = await CheckIfExists(employeeAddressDto.Id);

        if (!exists)
        {
            throw new CustomException("Employee Address Does Not Exist");
        }

        var address = await _db.EmployeeAddress.Update(new EmployeeAddress(employeeAddressDto));

        return address.ToDto();
    }
}
