using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IFieldCodeOptionsService
{
    /// <summary>
    ///     Save field code options
    /// </summary>
    /// <param name="fieldCodeOptionsDto"></param>
    /// <returns></returns>
    Task<FieldCodeOptionsDto> CreateFieldCodeOptions(FieldCodeOptionsDto fieldCodeOptionsDto);

    /// <summary>
    ///     Get field code options by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<List<FieldCodeOptionsDto>> GetFieldCodeOptionsById(int id);

    /// <summary>
    ///     Get all field code options
    /// </summary>
    /// <returns></returns>
    Task<List<FieldCodeOptionsDto>> GetAllFieldCodeOptions();

    /// <summary>
    ///     Update field code options
    /// </summary>
    /// <param name="fieldCodeOptionsDto"></param>
    /// <returns></returns>
    Task<List<FieldCodeOptionsDto>> UpdateFieldCodeOptions(List<FieldCodeOptionsDto> fieldCodeOptionsDto);

    /// <summary>
    ///     Delete field code options
    /// </summary>
    /// <param name="fieldCodeOptionsDto"></param>
    /// <returns></returns>
    Task<FieldCodeOptionsDto> DeleteFieldCodeOptions(FieldCodeOptionsDto fieldCodeOptionsDto);
}