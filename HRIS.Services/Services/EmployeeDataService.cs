using AutoMapper;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeDataService : IEmployeeDataService
{
    private readonly IUnitOfWork _db;
    private readonly AuthorizeIdentity _identity;
    private readonly IMapper _mapper;

    public EmployeeDataService(IUnitOfWork db, AuthorizeIdentity identity, IMapper mapper)
    {
        _db = db;
        _identity = identity;
        _mapper = mapper;
    }

    public async Task<bool> EmployeeDataExists(int id)
    {
        return await _db.EmployeeData.Any(x => x.Id == id);
    }

    public async Task<EmployeeDataDto> CreateEmployeeData(EmployeeDataDto employeeDataDto)
    {
        var modelExists = await EmployeeDataExists(employeeDataDto.Id);

        if (modelExists)
            throw new CustomException("This model already exists");

        if (!_identity.IsSupport && employeeDataDto.EmployeeId != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        var newEmployeeData = _mapper.Map<EmployeeDataDto>(await _db.EmployeeData.Add(_mapper.Map<EmployeeData>(employeeDataDto)));

        return newEmployeeData;
    }

    public async Task<EmployeeDataDto> GetEmployeeData(int employeeId)
    {
        var modelExists = await EmployeeDataExists(employeeId);
        if (!modelExists)
            throw new CustomException("Employee data does not exist");

        if (!_identity.IsSupport && employeeId != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        var employeeData = _mapper.Map<EmployeeDataDto>(await _db.EmployeeData.GetById(employeeId));

        if (employeeData == null)
            throw new CustomException("No employee data record found");

        return employeeData;
    }

    public async Task<EmployeeDataDto> UpdateEmployeeData(EmployeeDataDto employeeDataDto)
    {
        var modelExists = await EmployeeDataExists(employeeDataDto.Id);
        if (!modelExists) throw new CustomException("This model does not exist yet");

        if (!_identity.IsSupport && employeeDataDto.EmployeeId != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        var employeeData = await _db.EmployeeData.GetById(employeeDataDto.EmployeeId);

        if (employeeData == null)
            throw new CustomException("No employee data record found");

        var updatedEmployeeData = _mapper.Map<EmployeeDataDto>(await _db.EmployeeData.Update(new EmployeeData(employeeDataDto)));

        return updatedEmployeeData;
    }

    public async Task<EmployeeDataDto> DeleteEmployeeData(int employeeDataId)
    {
        var modelExists = await EmployeeDataExists(employeeDataId);
        if (!modelExists) throw new CustomException("This model does not exist");

        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var deletedData = _mapper.Map<EmployeeDataDto>(await _db.EmployeeData.Delete(employeeDataId));
        return deletedData;
    }
}