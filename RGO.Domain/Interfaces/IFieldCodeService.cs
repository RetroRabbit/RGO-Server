using RGO.Models;
using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Interfaces
{
    public interface IFieldCodeService
    {
        Task<FieldCodeDto> SaveFieldCode(FieldCodeDto fieldCodeDto);
        Task<FieldCodeDto?> GetFieldCode(string name);
        Task<List<FieldCodeDto>> GetAllFieldCodes();
        Task<FieldCode> UpdateFieldCodes(FieldCodeDto fieldCodeDto);
        Task<FieldCode> DeleteFieldCodes(FieldCodeDto fieldCodeDto);
    }
}
