using RGO.Models;
using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Interfaces
{
    public interface IFieldCodeOptionsService
    {
        Task<FieldCodeOptionsDto> SaveFieldCodeOption(FieldCodeOptionsDto fieldCodeOptionsDto);
        Task<List<FieldCodeOptionsDto>> GetFieldCodeOptions(int id);
        Task<List<FieldCodeOptionsDto>> GetAllFieldCodeOptions();
        Task<FieldCodeOptions> UpdateFieldCodeOptions(FieldCodeOptionsDto fieldCodeOptionsDto);
        Task<FieldCodeOptions> DeleteFieldCodeOptions(FieldCodeOptionsDto fieldCodeOptionsDto);
        Task CheckUpdate(List<FieldCodeOptionsDto> fieldCodeOptionsDto);
    }

}
