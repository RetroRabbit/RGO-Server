using HRIS.Models;
using HRIS.Services.Interfaces;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeAddressService : IEmployeeAddressService
{
    private readonly IUnitOfWork _db;
    private readonly IErrorLoggingService _errorLoggingService;

    public EmployeeAddressService(IUnitOfWork db, IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _errorLoggingService = errorLoggingService;
    }

    public async Task<bool> CheckIfExists(EmployeeAddressDto employeeAddressDto)
    {
        if(employeeAddressDto.Id == 0) {
            return false;
        }
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

        if (!exists) 
        {
            var exception = new Exception("Employee Address does not exist");
            throw _errorLoggingService.LogException(exception);
        }

        return await _db.EmployeeAddress.FirstOrDefault(address => address.Id == employeeAddressDto.Id);
    }

    public async Task<List<EmployeeAddressDto>> GetAll()
    {
        return await _db.EmployeeAddress.GetAll();
    }

    public async Task<EmployeeAddressDto> Save(EmployeeAddressDto employeeAddressDto)
    {
        var exists = await CheckIfExists(employeeAddressDto);

        if (exists)
        {
            var exception = new Exception("Employee Address already exists");
            throw _errorLoggingService.LogException(exception);
        }

        var address = await _db.EmployeeAddress.Add(new EmployeeAddress(employeeAddressDto));

        return address;
    }

    public async Task<EmployeeAddressDto> Update(EmployeeAddressDto employeeAddressDto)
    {
        var exists = await CheckIfExists(employeeAddressDto);

        if (!exists)
        {
            var exception = new Exception("Employee Address does not exist");
            throw _errorLoggingService.LogException(exception);
        }

        var address = await _db.EmployeeAddress.Update(new EmployeeAddress(employeeAddressDto));

        return address;
    }
}
