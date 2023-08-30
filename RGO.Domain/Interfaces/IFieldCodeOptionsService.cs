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
        /// <summary>
        /// Save field code options
        /// </summary>
        /// <param name="fieldCodeOptionsDto"></param>
        /// <returns></returns>
        Task<FieldCodeOptionsDto> SaveFieldCodeOptions(FieldCodeOptionsDto fieldCodeOptionsDto);

        /// <summary>
        /// Get field code options by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<FieldCodeOptionsDto>> GetFieldCodeOptions(int id);

        /// <summary>
        /// Get all field code options 
        /// </summary>
        /// <returns></returns>
        Task<List<FieldCodeOptionsDto>> GetAllFieldCodeOptions();

        /// <summary>
        /// Update field code options
        /// </summary>
        /// <param name="fieldCodeOptionsDto"></param>
        /// <returns></returns>
        Task UpdateFieldCodeOptions(List<FieldCodeOptionsDto> fieldCodeOptionsDto);

        /// <summary>
        /// Delete field code options
        /// </summary>
        /// <param name="fieldCodeOptionsDto"></param>
        /// <returns></returns>
        Task<FieldCodeOptions> DeleteFieldCodeOptions(FieldCodeOptionsDto fieldCodeOptionsDto);
    }

}
