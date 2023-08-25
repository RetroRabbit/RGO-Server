using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Services
{
    public class FieldCodeService
    {
        private readonly IUnitOfWork _db;

        public FieldCodeService(IUnitOfWork db)
        {
            _db = db;
        }

        //TODO: COMPLETE 
        public async Task SaveFieldCode(FieldCodeDto fieldCodeDto)
        {
            var ifFieldCode = CheckFieldExist(fieldCodeDto.Name);
        }


        public async Task<FieldCodeDto> GetFieldCode(string name)
        {
            var ifFieldCode = CheckFieldExist(name);
            if (ifFieldCode == null) { throw new Exception("No field with that name found"); }

            var fieldCode = await _db.FieldCode
                .Get(fieldCode => fieldCode.Name == name)
                .Select(fieldCode => fieldCode.ToDto())
                .Take(1)
                .FirstOrDefaultAsync();

            return fieldCode;
        }

        //TODO: GETALLFIELDCODES
        //TODO: UPDATEFIELDCODES
        //TODO: DELETEFIELDCODES

        public async Task<bool> CheckFieldExist(string name)
        {
            return await _db.FieldCode
                .Get(fieldCode => fieldCode.Name == name)
                .AnyAsync();
        }

    }
}
