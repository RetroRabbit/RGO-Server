using Auth0.ManagementApi.Models;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class FieldCodeOptionsService : IFieldCodeOptionsService
{
    private readonly IUnitOfWork _db;
    private readonly AuthorizeIdentity _identity;

    public FieldCodeOptionsService(IUnitOfWork db, AuthorizeIdentity identity)
    {
        _db = db;
        _identity = identity;
    }

    public async Task<bool> FieldCodeOptionsExists(int id)
    {
        return await _db.FieldCodeOptions.Any(x => x.Id == id);
    }

    public async Task<FieldCodeOptionsDto> CreateFieldCodeOptions(FieldCodeOptionsDto fieldCodeOptionsDto)
    {
        var fieldCodeOptionExist = await FieldCodeOptionsExists(fieldCodeOptionsDto.Id);

        if (fieldCodeOptionExist)
            throw new CustomException("Field option with that name found");

        if (_identity.IsSupport == false && fieldCodeOptionsDto.Id != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        var fieldCodes = await GetAllFieldCodeOptions();
        var fieldCode = fieldCodes
                        .Where(fieldCode =>
                                   fieldCode.Id == fieldCodeOptionsDto.FieldCodeId && fieldCode.Option.ToLower() ==
                                   fieldCodeOptionsDto.Option.ToLower())
                        .Select(fieldCode => fieldCode)
                        .FirstOrDefault();

        var newFieldCodeOption = await _db.FieldCodeOptions.Add(new FieldCodeOptions(fieldCodeOptionsDto));
        return newFieldCodeOption.ToDto();
    }

    public async Task<List<FieldCodeOptionsDto>> GetFieldCodeOptionsById(int id)
    {
        var fieldCodeOptionExist = await FieldCodeOptionsExists(id);

        if (!fieldCodeOptionExist)
            throw new CustomException("Field Code Option does not exist");

        if (_identity.IsSupport == false && id != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        var fieldCodes = await GetAllFieldCodeOptions();
        var fieldCode = fieldCodes
                        .Where(fieldCode => fieldCode.FieldCodeId == id)
                        .ToList();

        return fieldCode;
    }

    public async Task<List<FieldCodeOptionsDto>> GetAllFieldCodeOptions()
    {
        return (await _db.FieldCodeOptions.GetAll()).Select(x => x.ToDto()).ToList();
    }

    public async Task<List<FieldCodeOptionsDto>> UpdateFieldCodeOptions(List<FieldCodeOptionsDto> fieldCodeOptionsDto)
    {
        var fieldCodeOptionExist = await FieldCodeOptionsExists(fieldCodeOptionsDto[0].FieldCodeId);

        if (!fieldCodeOptionExist)
            throw new CustomException("Field Code Option does not exist");

        if (_identity.IsSupport == false && fieldCodeOptionsDto[0].FieldCodeId != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        foreach (var option in fieldCodeOptionsDto)
        {
            var field = await GetAllFieldCodeOptions();
            var existingOptions = field.Where(fieldCodeOption => fieldCodeOption.FieldCodeId == option.FieldCodeId
                                                                 && fieldCodeOption.Option.ToLower() ==
                                                                 option.Option.ToLower())
                                       .FirstOrDefault();

            var addFieldCode = new FieldCodeOptions(option);
            await _db.FieldCodeOptions.Add(addFieldCode);
        }

        var existingFieldCodeOptions = await GetFieldCodeOptionsById(fieldCodeOptionsDto[0].FieldCodeId);
        var check = true;
        foreach (var option in existingFieldCodeOptions)
        {
            foreach (var fieldCode in fieldCodeOptionsDto)
            {
                if (option.FieldCodeId == fieldCode.FieldCodeId &&
                    option.Option.ToLower() == fieldCode.Option.ToLower())
                {
                    check = true;
                    break;
                }

                check = false;
            }

            if (!check) _ = await _db.FieldCodeOptions.Delete(option.Id);
        }

        var updatedFieldCodeOptions = await GetFieldCodeOptionsById(fieldCodeOptionsDto[0].FieldCodeId);
        return updatedFieldCodeOptions;
    }

    public async Task<FieldCodeOptionsDto> DeleteFieldCodeOptions(FieldCodeOptionsDto fieldCodeOptionsDto)
    {
        var fieldCodeOptionExist = await FieldCodeOptionsExists(fieldCodeOptionsDto.Id);

        if (!fieldCodeOptionExist)
            throw new CustomException("Field Code Option does not exist");

        if (_identity.IsSupport == false && fieldCodeOptionsDto.Id != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        var deleteFieldCodeOptions = await _db.FieldCodeOptions.Delete(new FieldCodeOptions(fieldCodeOptionsDto).Id);
        return deleteFieldCodeOptions.ToDto();
    }
}