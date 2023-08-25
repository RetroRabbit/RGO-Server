using RGO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Interfaces
{
    public interface IFieldCodeService
    {
        Task SaveFieldCode(FieldCodeDto fieldCodeDto);
        Task<FieldCodeDto> GetFieldCode(string name);
        Task<FieldCodeDto> GetFieldCodeAsync(string name);
        Task<bool> CheckFieldExist(string name);
        Task<List<FieldCodeDto>> GetAllFieldCodes();
        Task UpdateFieldCodes(FieldCodeDto fieldCodeDto);
        Task DeleteFieldCodes(string name);
    }
}
