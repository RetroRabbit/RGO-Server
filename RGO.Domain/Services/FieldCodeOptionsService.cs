using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Services
{
    public class FieldCodeOptionsService : IFieldCodeOptionsService
    {
        private readonly IUnitOfWork _db;

        public FieldCodeOptionsService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<FieldCodeOptionsDto> SaveFieldCodeOptions(FieldCodeOptionsDto fieldCodeOptionsDto)
        {
            var ifFieldCodeOption = await CheckFieldCodeOption(fieldCodeOptionsDto);
            if (ifFieldCodeOption) { throw new Exception("Field option with that name found"); }

            FieldCodeOptions fieldCodeOptions = new FieldCodeOptions(fieldCodeOptionsDto);
            var newFieldCodeOption = await _db.FieldCodeOptions.Add(fieldCodeOptions);
            return newFieldCodeOption;
        }

        public async Task<List<FieldCodeOptionsDto>> GetFieldCodeOptions(int id)
        {
            return await _db.FieldCodeOptions.GetAll(fieldCodeOptions => fieldCodeOptions.FieldCode.Id == id);
        }


        public async Task<List<FieldCodeOptionsDto>> GetAllFieldCodeOptions()
        {
            return await _db.FieldCodeOptions.GetAll();
        }

        public async Task UpdateFieldCodeOptions(List<FieldCodeOptionsDto> fieldCodeOptionsDto)
        {
            foreach (var option in fieldCodeOptionsDto)
            {
                var existingOptions = await _db.FieldCodeOptions
                    .Get(fieldCodeOption => fieldCodeOption.FieldCode.Id == option.FieldCodeId && fieldCodeOption.Option.ToLower() == option.Option.ToLower())
                    .FirstOrDefaultAsync();

                if (existingOptions == null)
                {
                    FieldCodeOptions addFieldCode = new FieldCodeOptions(option);
                    await _db.FieldCodeOptions.Add(addFieldCode);
                }
            }

            var existingFieldCodeOptions = await GetFieldCodeOptions(fieldCodeOptionsDto[0].FieldCodeId);
            bool check = true;
            foreach (var option in existingFieldCodeOptions)
            {
                foreach (var fieldCode in fieldCodeOptionsDto)
                {
                    if (option.FieldCodeId == fieldCode.FieldCodeId && option.Option.ToLower() == fieldCode.Option.ToLower())
                    {
                        check = true;
                        break;
                    }
                    check = false;
                }
                if (!check)
                {
                    await _db.FieldCodeOptions.Delete(option.Id);
                }
            }
        }

        public async Task<FieldCodeOptions> DeleteFieldCodeOptions(FieldCodeOptionsDto fieldCodeOptionsDto)
        {
            var ifFieldCodeOption = await CheckFieldCodeOption(fieldCodeOptionsDto);
            if (!ifFieldCodeOption) { throw new Exception("No field with that name found"); }

            FieldCodeOptions deleteFieldCodeOptions = new FieldCodeOptions(fieldCodeOptionsDto);
            await _db.FieldCodeOptions.Delete(deleteFieldCodeOptions.Id);
            return deleteFieldCodeOptions;
        }

        private async Task<bool> CheckFieldCodeOption(FieldCodeOptionsDto fieldCodeOptionsDto)
        {
            var fieldCodeOption = await _db.FieldCodeOptions
            .Get(fieldCodeOption => fieldCodeOption.Id == fieldCodeOptionsDto.Id && fieldCodeOption.Option.ToLower() == fieldCodeOptionsDto.Option.ToLower() )
            .FirstOrDefaultAsync();

            if (fieldCodeOption == null) { return false; }
            else { return true; }
        }
    }
}
