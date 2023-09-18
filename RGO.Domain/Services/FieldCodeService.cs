using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Xml.Linq;

namespace RGO.Services.Services
{
    public class FieldCodeService : IFieldCodeService
    {
        private readonly IUnitOfWork _db;
        private readonly IFieldCodeOptionsService _fieldCodeOptionsService;

        public FieldCodeService(IUnitOfWork db, IFieldCodeOptionsService fieldCodeOptionsService)
        {
            _db = db;
            _fieldCodeOptionsService = fieldCodeOptionsService;
        }

        public async Task<FieldCodeDto> SaveFieldCode(FieldCodeDto fieldCodeDto)
        {
            var ifFieldCode = await GetFieldCode(fieldCodeDto.Name);

            if (ifFieldCode != null) { throw new Exception("Field with that name found"); }

            var newFieldCode = await _db.FieldCode.Add(new FieldCode(fieldCodeDto));
            if (newFieldCode != null && fieldCodeDto.Options.Count > 0)
            {
                foreach(var option in fieldCodeDto.Options)
                {
                    var fieldCodeOptionsDto = new FieldCodeOptionsDto(
                            Id: 0,
                            FieldCodeId: newFieldCode.Id,
                            Option: option.Option);
                    await _fieldCodeOptionsService.SaveFieldCodeOptions(fieldCodeOptionsDto);
                }
            }
            var options = await _fieldCodeOptionsService.GetFieldCodeOptions(newFieldCode.Id);
            newFieldCode.Options = options;
            return newFieldCode;
        }

        //TODO: Update this method to only get all field codes with a status of "active"
        public async Task<FieldCodeDto?> GetFieldCode(string name)
        {
            var fieldCodes = await _db.FieldCode.GetAll(); ;
            var fieldCode = fieldCodes
                .Where(fieldCode => fieldCode.Name == name)
                .Select(fieldCode => fieldCode)
                .FirstOrDefault();

            if(fieldCode != null)
            {
                var options = await _fieldCodeOptionsService.GetFieldCodeOptions(fieldCode.Id);
                fieldCode.Options = options;
            }

            return fieldCode;
        }

        //TODO: Update this method to only get all field codes with a status of "active"
        public async Task<List<FieldCodeDto>> GetAllFieldCodes() 
        {
            var fieldCodes = await _db.FieldCode.GetAll();
            if(fieldCodes.Count != 0)
            {
                foreach(var fieldCode in fieldCodes)
                {
                    var options = await _fieldCodeOptionsService.GetFieldCodeOptions(fieldCode.Id);
                    fieldCode.Options = (options != null) ? options : null;
                }
            }
            return fieldCodes;
        }

        //TODO: Update this method to incorporate the model change
        public async Task<FieldCodeDto> UpdateFieldCode(FieldCodeDto fieldCodeDto)
        {
            var updatedFieldCode = await _db.FieldCode.Update(new FieldCode(fieldCodeDto));
            return updatedFieldCode;
        }

        //TODO: Update this method to incorporate the model change and instead of deleting, change status to archived
        public async Task<FieldCodeDto> DeleteFieldCode(FieldCodeDto fieldCodeDto)
        {
            var ifFieldCode = await GetFieldCode(fieldCodeDto.Name);
            if (ifFieldCode == null) { throw new Exception("No field with that name found"); }

            var fieldCode = await _db.FieldCode.Delete(new FieldCode(fieldCodeDto).Id);
            return fieldCode;
        }
    }
}