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
            var ifFieldCode = await GetFieldCode(fieldCodeDto.Name);

            if (ifFieldCode != null) { throw new Exception("Field with that name found"); }

            var newFieldCode = await _db.FieldCode.Add(new FieldCode(fieldCodeDto));
            return newFieldCode;
        }

        public async Task<FieldCodeDto?> GetFieldCode(string name)
        {
            var fieldCodes = await GetAllFieldCodes();
            var fieldCode = fieldCodes
                .Where(fieldCode => fieldCode.Name == name)
                .Select(fieldCode => fieldCode)
                .FirstOrDefault();

            return fieldCode;
        }

        public async Task<List<FieldCodeDto>> GetAllFieldCodes() 
        {
            return await _db.FieldCode.GetAll();
        }

        public async Task<FieldCodeDto> UpdateFieldCode(FieldCodeDto fieldCodeDto)
        {
            var updatedFieldCode = await _db.FieldCode.Update(new FieldCode(fieldCodeDto));
            return updatedFieldCode;
        }

        public async Task<FieldCodeDto> DeleteFieldCode(FieldCodeDto fieldCodeDto)
        {
            var ifFieldCode = await GetFieldCode(fieldCodeDto.Name);
            if (ifFieldCode == null) { throw new Exception("No field with that name found"); }

            var fieldCode = await _db.FieldCode.Delete(new FieldCode(fieldCodeDto).Id);
            return fieldCode;
        }
    }
}