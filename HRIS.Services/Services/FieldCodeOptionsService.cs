﻿using HRIS.Models;
using HRIS.Services.Interfaces;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class FieldCodeOptionsService : IFieldCodeOptionsService
{
    private readonly IUnitOfWork _db;
    private readonly IErrorLoggingService _errorLoggingService;

    public FieldCodeOptionsService(IUnitOfWork db, IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _errorLoggingService = errorLoggingService;
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
        {
            var exception = new Exception("Field option with that name found");
            throw _errorLoggingService.LogException(exception);
        }

        var newFieldCodeOption = await _db.FieldCodeOptions.Add(new FieldCodeOptions(fieldCodeOptionsDto));
        return newFieldCodeOption;
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
        return await _db.FieldCodeOptions.GetAll();
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
        {
            var exception = new Exception("No field with that name found");
            throw _errorLoggingService.LogException(exception);
        }

        var deleteFieldCodeOptions = await _db.FieldCodeOptions.Delete(new FieldCodeOptions(fieldCodeOptionsDto).Id);
        return deleteFieldCodeOptions;
    }
}