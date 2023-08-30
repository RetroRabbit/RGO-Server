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

        public FieldCodeService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<FieldCodeDto> SaveFieldCode(FieldCodeDto fieldCodeDto)
        {
            var ifFieldCode = await CheckFieldCode(fieldCodeDto.Name);

            if (ifFieldCode) { throw new Exception("Field with that name found"); }

            FieldCode fieldCode = new FieldCode(fieldCodeDto);
            var newFieldCode = await _db.FieldCode.Add(fieldCode);
            return newFieldCode;
        }

        public async Task<FieldCodeDto?> GetFieldCode(string name)
        {
            var ifFieldCode = await CheckFieldCode(name);
            if (!ifFieldCode) { throw new Exception("No field with that name found"); }

            var fieldCode = await _db.FieldCode
                .Get(fieldCode => fieldCode.Name == name)
                .Select(fieldCode => fieldCode.ToDto())
                .FirstOrDefaultAsync();

            return fieldCode;
        }

        public async Task<List<FieldCodeDto>> GetAllFieldCodes() 
        {
            return await _db.FieldCode.GetAll();
        }

        public async Task<FieldCode> UpdateFieldCode(FieldCodeDto fieldCodeDto)
        {
            var ifFieldCode = await _db.FieldCode
            .Get(fieldCode => fieldCode.Id == fieldCodeDto.Id)
            .FirstOrDefaultAsync();
            if (ifFieldCode == null) { throw new Exception("No field with that id found"); }

            FieldCode updatedFieldCode = new FieldCode(fieldCodeDto);
            await _db.FieldCode.Update(updatedFieldCode);
            return updatedFieldCode;
        }

        public async Task<FieldCode> DeleteFieldCode(FieldCodeDto fieldCodeDto)
        {
            var ifFieldCode = await CheckFieldCode(fieldCodeDto.Name);
            if (!ifFieldCode) { throw new Exception("No field with that name found"); }

            FieldCode fieldCode = new FieldCode(fieldCodeDto);
            await _db.FieldCode.Delete(fieldCode.Id);
            return fieldCode;
        }

        private async Task<bool> CheckFieldCode(string name)
        {
            var fieldCode = await _db.FieldCode
            .Get(fieldCode => fieldCode.Name.ToLower() == name.ToLower())
            .FirstOrDefaultAsync();

            if (fieldCode == null) { return false; }
            else { return true; }
        }
    }
}