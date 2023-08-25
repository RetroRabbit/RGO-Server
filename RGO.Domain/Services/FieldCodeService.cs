using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services
{
    public class FieldCodeService
    {
        private readonly IUnitOfWork _db;

        public FieldCodeService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task SaveFieldCode(FieldCodeDto fieldCodeDto)
        {
            var ifFieldCodeExists = await CheckFieldExist(fieldCodeDto.Name);

            if (!ifFieldCodeExists)
            {
                var fieldCode = new FieldCode
                {
                    Name = fieldCodeDto.Name,

                };
                await _db.FieldCode.AddAsync(fieldCode);
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Field code with that name already exists!");
            }
        }

        public async Task<FieldCodeDto> GetFieldCode(string name)
        {
            var ifFieldCode = await CheckFieldExist(name);
            if (!ifFieldCode) { throw new Exception("No field with that name found"); }

            var fieldCode = await _db.FieldCode
                .Where(fieldCode => fieldCode.Name == name)
                .Select(fieldCode => fieldCode.ToDto())
                .FirstOrDefaultAsync();

            return fieldCode;
        }

        public async Task<bool> CheckFieldExist(string name)
        {
            return await _db.FieldCode
                .AnyAsync(fieldCode => fieldCode.Name == name);
        }

        public async Task<List<FieldCodeDto>> GetAllFieldCodes() 
        { 
            return await _db.FieldCode
                .Select(fieldCode => fieldCode.ToDto())
                .ToListAsync(); 
        }

        public async Task UpdateFieldCodes(FieldCodeDto fieldCodeDto)
        {
            var fieldCode = await _db.FieldCode
                .FirstOrDefaultAsync(fc => fc.Name == fieldCodeDto.Name);

            if (fieldCode != null)
            {
                fieldCode.Name = fieldCodeDto.Name;

                _db.FieldCode.Update(fieldCode);
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Field code with that name not found.");
            }
        }

        public async Task DeleteFieldCodes(string name)
        {
            var fieldCode = await _db.FieldCode
                .FirstOrDefaultAsync(fc => fc.Name == name);

            if (fieldCode != null)
            {
                _db.FieldCode.Remove(fieldCode);
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Field code with that name not found.");
            }
        }

        public async Task<bool> CheckFieldExist(string name)
        {
            return await _db.FieldCode
                .Get(fieldCode => fieldCode.Name == name)
                .AnyAsync();
        }
    }
}