﻿using HRIS.Models;
using HRIS.Services.Interfaces;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class FieldCodeOptionsService : IFieldCodeOptionsService
{
    private readonly IUnitOfWork _db;

    public FieldCodeOptionsService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<FieldCodeOptionsDto> SaveFieldCodeOptions(FieldCodeOptionsDto fieldCodeOptionsDto)
    {
        var fieldCodes = await GetAllFieldCodeOptions();
        var fieldCode = fieldCodes
                        .Where(fieldCode =>
                                   fieldCode.Id == fieldCodeOptionsDto.FieldCodeId && fieldCode.Option.ToLower() ==
                                   fieldCodeOptionsDto.Option.ToLower())
                        .Select(fieldCode => fieldCode)
                        .FirstOrDefault();


        if (fieldCode != null)
            throw new CustomException("Field option with that name found");

        var newFieldCodeOption = await _db.FieldCodeOptions.Add(new FieldCodeOptions(fieldCodeOptionsDto));
        return newFieldCodeOption.ToDto();
    }

    public async Task<List<FieldCodeOptionsDto>> GetFieldCodeOptions(int id)
    {
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
        foreach (var option in fieldCodeOptionsDto)
        {
            var field = await GetAllFieldCodeOptions();
            var existingOptions = field.Where(fieldCodeOption => fieldCodeOption.FieldCodeId == option.FieldCodeId
                                                                 && fieldCodeOption.Option.ToLower() ==
                                                                 option.Option.ToLower())
                                       .FirstOrDefault();

            if (existingOptions == null)
            {
                var addFieldCode = new FieldCodeOptions(option);
                await _db.FieldCodeOptions.Add(addFieldCode);
            }
        }

        var existingFieldCodeOptions = await GetFieldCodeOptions(fieldCodeOptionsDto[0].FieldCodeId);
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

        var updatedFieldCodeOptions = await GetFieldCodeOptions(fieldCodeOptionsDto[0].FieldCodeId);
        return updatedFieldCodeOptions;
    }

    public async Task<FieldCodeOptionsDto> DeleteFieldCodeOptions(FieldCodeOptionsDto fieldCodeOptionsDto)
    {
        var ifFieldCodeOption = await GetFieldCodeOptions(fieldCodeOptionsDto.Id);
        if (ifFieldCodeOption == null)
            throw new CustomException("No field with that name found");

        var deleteFieldCodeOptions = await _db.FieldCodeOptions.Delete(new FieldCodeOptions(fieldCodeOptionsDto).Id);
        return deleteFieldCodeOptions.ToDto();
    }
}