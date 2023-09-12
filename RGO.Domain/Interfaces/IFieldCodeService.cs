using RGO.Models;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Interfaces
{
    public interface IFieldCodeService
    {
        /// <summary>
        /// Save field code
        /// </summary>
        /// <param name="fieldCodeDto"></param>
        /// <returns></returns>
        Task<FieldCodeDto> SaveFieldCode(FieldCodeDto fieldCodeDto);

        /// <summary>
        /// Get field code by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<FieldCodeDto?> GetFieldCode(string name);

        /// <summary>
        /// Get all field codes
        /// </summary>
        /// <returns></returns>
        Task<List<FieldCodeDto>> GetAllFieldCodes();

        /// <summary>
        /// Update field code
        /// </summary>
        /// <param name="fieldCodeDto"></param>
        /// <returns></returns>
        Task<FieldCodeDto> UpdateFieldCode(FieldCodeDto fieldCodeDto);

        /// <summary>
        /// Delete field code
        /// </summary>
        /// <param name="fieldCodeDto"></param>
        /// <returns></returns>
        Task<FieldCodeDto> DeleteFieldCode(FieldCodeDto fieldCodeDto);
    }
}
