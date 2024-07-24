using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class FieldCodeService : IFieldCodeService
{
    private readonly IUnitOfWork _db;
    private readonly IFieldCodeOptionsService _fieldCodeOptionsService;
    private readonly AuthorizeIdentity _identity;

    public FieldCodeService(IUnitOfWork db, IFieldCodeOptionsService fieldCodeOptionsService, AuthorizeIdentity identity)
    {
        _db = db;
        _fieldCodeOptionsService = fieldCodeOptionsService;
        _identity = identity;
    }

    public async Task<FieldCodeDto> CreateFieldCode(FieldCodeDto fieldCodeDto)
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        if (fieldCodeDto.Id != 0)
        {
            return await UpdateFieldCode(fieldCodeDto);
        }

        var ifFieldCode = await GetFieldCode(fieldCodeDto.Name!);
        if (ifFieldCode != null) throw new CustomException("Field with that name found");

        var newFieldCode = await _db.FieldCode.Add(new FieldCode(fieldCodeDto));
        if (newFieldCode != null && fieldCodeDto.Options!.Count > 0)
        {
            foreach (var option in fieldCodeDto.Options)
            {
                var fieldCodeOptionsDto = new FieldCodeOptionsDto
                {
                    Id = 0,
                    FieldCodeId = newFieldCode.Id,
                    Option = option.Option
                };
                await _fieldCodeOptionsService.SaveFieldCodeOptions(fieldCodeOptionsDto);
            }
        }

        var options = await _fieldCodeOptionsService.GetFieldCodeOptions(newFieldCode!.Id);
        var dto = newFieldCode.ToDto();
        dto.Options = options;
        return dto;
    }

    public async Task<FieldCodeDto?> GetFieldCode(string name)
    {
        var fieldCodes = await _db.FieldCode.GetAll();
        var fieldCode = fieldCodes
                        .Where(fieldCode => fieldCode.Name == name && fieldCode.Status == ItemStatus.Active)
                        .Select(fieldCode => fieldCode.ToDto())
                        .FirstOrDefault();

        if (fieldCode != null)
        {
            var options = await _fieldCodeOptionsService.GetFieldCodeOptions(fieldCode.Id);
            fieldCode.Options = options;
        }

        return fieldCode;
    }

    public async Task<List<FieldCodeDto>?> GetAllFieldCodes()
    {
        var fieldCodes = await _db.FieldCode.GetAll();
        var fieldCode = fieldCodes
                        .Select(fieldCode => fieldCode.ToDto())
                        .ToList();
        if (fieldCodes == null)
            throw new CustomException("No field codes found.");

        if (fieldCode.Count != 0)
            foreach (var item in fieldCode)
            {
                var options = await _fieldCodeOptionsService.GetFieldCodeOptions(item.Id);
                item.Options = options != null ? options : null;
            }

        return fieldCode;
    }

    public async Task<FieldCodeDto> UpdateFieldCode(FieldCodeDto fieldCodeDto)
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        await _db.FieldCode.Update(new FieldCode(fieldCodeDto));
        if (fieldCodeDto.Options!.Count > 0) await _fieldCodeOptionsService.UpdateFieldCodeOptions(fieldCodeDto.Options);

        var getUpdatedFieldCode = await GetFieldCode(fieldCodeDto.Name!);
        return getUpdatedFieldCode!;
    }

    public async Task<FieldCodeDto> DeleteFieldCode(FieldCodeDto fieldCodeDto)
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var ifFieldCode = await GetFieldCode(fieldCodeDto.Name!);
        if (ifFieldCode == null)
            throw new CustomException("No field with that name found");

        var newFieldCodeDto = new FieldCodeDto
        {
            Id = ifFieldCode.Id,
            Code = ifFieldCode.Code,
            Name = ifFieldCode.Name,
            Description = ifFieldCode.Description,
            Regex = ifFieldCode.Regex,
            Type = ifFieldCode.Type,
            Status = ItemStatus.Archive,
            Internal = ifFieldCode.Internal,
            InternalTable = ifFieldCode.InternalTable,
            Category = ifFieldCode.Category,
            Required = ifFieldCode.Required
        };

        var fieldCode = (await _db.FieldCode.Update(new FieldCode(newFieldCodeDto))).ToDto();
        var options = await _fieldCodeOptionsService.GetFieldCodeOptions(fieldCode.Id);
        fieldCode.Options = options ?? new List<FieldCodeOptionsDto>();
        return fieldCode;
    }

    public async Task<List<FieldCodeDto>> GetByCategory(int categoryIndex)
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        if (categoryIndex < 0 || categoryIndex > 3)
            throw new CustomException("Invalid Index");

        var type = FieldCodeCategory.Profile;
        switch (categoryIndex)
        {
            case 0:
                type = FieldCodeCategory.Profile;
                break;
            case 1:
                type = FieldCodeCategory.Banking;
                break;
            case 2:
                type = FieldCodeCategory.Career;
                break;
            case 3:
                type = FieldCodeCategory.Documents;
                break;
        }

        var fields = await _db.FieldCode
                              .Get(field => field.Category == type)
                              .Select(field => field.ToDto())
                              .ToListAsync();
        return fields;
    }
}
