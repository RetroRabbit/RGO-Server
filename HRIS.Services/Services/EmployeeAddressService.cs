using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeAddressService : IEmployeeAddressService
{
    private readonly IUnitOfWork _db;
    private readonly AuthorizeIdentity _identity;

    public EmployeeAddressService(IUnitOfWork db, AuthorizeIdentity identity)
    {
        _db = db;
        _identity = identity;
    }

    public async Task<bool> CheckIfExists(int employeeId)
    {
        var returnVal = await _db.EmployeeAddress.Any(ea => ea.EmployeeId == employeeId);
        return returnVal;
    }

    public async Task<EmployeeAddressDto> Delete(int employeeId)
    {
        var exists = await CheckIfExists(employeeId);

        if (!exists)
        {
            throw new CustomException("Employee Address Does Not Exist");
        }

        var employee = await _db.Employee.FirstOrDefault(e => e.Id == employeeId);

        if (_identity.IsSupport == false && _identity.EmployeeId != employee!.Id)
            throw new CustomException("Unauthorized Access.");

        var address = await _db.EmployeeAddress.Delete(employeeId);
        return address.ToDto();
    }

    public async Task<EmployeeAddressDto> GetById(int employeeId)
    {
        var exists = await CheckIfExists(employeeId);

        if (!exists)
        {
            throw new CustomException("Employee Address Does Not Exist");
        }

        var employee = await _db.Employee.FirstOrDefault(e => e.Id == employeeId);

        if (_identity.IsSupport == false && _identity.EmployeeId != employee!.Id)
            throw new CustomException("Unauthorized Access.");

        var address = await _db.EmployeeAddress.FirstOrDefault(address => address.EmployeeId == employeeId);
        return address!.ToDto();
    }

    public async Task<List<EmployeeAddressDto>> GetAll()
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        return (await _db.EmployeeAddress.GetAll()).Select(x => x.ToDto()).ToList();
    }

    public async Task<EmployeeAddressDto> Create(EmployeeAddressDto employeeAddressDto)
    {
        var exists = await CheckIfExists(employeeAddressDto.EmployeeId);

        if (exists)
        {
            throw new CustomException("Employee Address Already Exists");
        }

        var employee = await _db.Employee.FirstOrDefault(e => e.Id == employeeAddressDto.EmployeeId);

        if (_identity.IsSupport == false && _identity.EmployeeId != employee!.Id)
            throw new CustomException("Unauthorized Access.");

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

        var employee = await _db.Employee.FirstOrDefault(e => e.Id == employeeAddressDto.EmployeeId);

        if (_identity.IsSupport == false && _identity.EmployeeId != employee!.Id)
            throw new CustomException("Unauthorized Access.");

        var address = await _db.EmployeeAddress.Update(new EmployeeAddress(employeeAddressDto));

        return address.ToDto();
    }
}
